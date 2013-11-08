using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.InCommon.InCert.Engine.Logging;
using Org.InCommon.InCert.Engine.Results.Misc;
using log4net;
using X509Extension = System.Security.Cryptography.X509Certificates.X509Extension;

namespace Org.InCommon.InCert.Engine.Utilities
{
    static class CertificateUtilities
    {
        private const string AuthorityKeyOid = "2.5.29.35";
        private const string SubjectKeyOid = "2.5.29.14";

        private static readonly ILog Log = Logger.Create();

        internal static class NativeMethods
        {
            internal const Int32 ImportFromFile = 1;
            internal const Int32 ImportFromContext = 2;
            internal const Int32 NoUserInterface = 0x0001;
            internal const Int32 ImportToCurrentuser = 0x00200000;
            internal const Int32 ImportToLocalMachine = 0x00100000;
            internal const Int32 AsnEcoding = 0x00000001;
            internal const Int32 Pkcs7AsnEncoding = 0x00010000;
            internal const int UserKeyset = 4096;
            internal const uint CertKeyProvInfoPropId = 0x00000002;
            internal const int CertFriendlyNamePropId = 11;

            internal struct WinTrustFileInfo : IDisposable
            {
                public int StructSize;
                private readonly IntPtr _filePathPointer;
                // required, file name to be verified
                public IntPtr FileHandle;
                // optional, open handle to FilePath
                internal IntPtr KnownSubjectPointer;
                // optional, subject type if it is known
                public WinTrustFileInfo(String filePath)
                {
                    StructSize = Marshal.SizeOf(typeof(WinTrustFileInfo));
                    FileHandle = IntPtr.Zero;
                    KnownSubjectPointer = IntPtr.Zero;
                    _filePathPointer = Marshal.StringToCoTaskMemAuto(filePath);
                }

                public void Dispose()
                {
                    Marshal.FreeCoTaskMem(_filePathPointer);
                }
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
            internal class WinTrustData : IDisposable
            {
                internal int StructSize = Marshal.SizeOf(typeof(WinTrustData));
                internal IntPtr PolicyCallbackData = IntPtr.Zero;
                internal IntPtr SIPClientData = IntPtr.Zero;

                // required: UI choice
                internal WinTrustDataUiChoice UIChoice = WinTrustDataUiChoice.None;
                // required: certificate revocation check options
                internal WinTrustDataRevocationChecks RevocationChecks = WinTrustDataRevocationChecks.None;
                // required: which structure is being passed in?
                internal WinTrustDataChoice UnionChoice = WinTrustDataChoice.File;
                // individual file
                internal IntPtr FileInfoPtr;
                internal WinTrustDataStateAction StateAction = WinTrustDataStateAction.Ignore;
                internal IntPtr StateData = IntPtr.Zero;
                [MarshalAs(UnmanagedType.LPWStr)]
                internal String URLReference = null;
                internal WinTrustDataProvFlags ProvFlags = WinTrustDataProvFlags.SaferFlag;

                internal WinTrustDataUiContext UIContext = WinTrustDataUiContext.Execute;

                // constructor for silent WinTrustDataChoice.File check
                internal WinTrustData(String fileName, WinTrustDataRevocationChecks checks)
                {
                    var wtfiData = new WinTrustFileInfo(fileName);
                    FileInfoPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(WinTrustFileInfo)));
                    Marshal.StructureToPtr(wtfiData, FileInfoPtr, false);
                    RevocationChecks = checks;
                }

                ~WinTrustData()
                {
                    Dispose();
                }

                public void Dispose()
                {
                    Marshal.FreeCoTaskMem(FileInfoPtr);
                }
            }

            internal enum WinTrustDataUiChoice : uint
            {
                All = 1,
                None = 2,
                NoBad = 3,
                NoGood = 4,
            }

            internal enum WinTrustDataUiContext : uint
            {
                Execute = 0,
                Install = 1
            }

            internal enum WinTrustDataRevocationChecks : uint
            {
                None = 0,
                WholeChain = 1
            }

            internal enum WinTrustDataChoice : uint
            {
                File = 1,
                Catalog = 2,
                Blob = 3,
                Signer = 4,
                Certificate = 5
            }

            internal enum WinTrustDataStateAction : uint
            {
                Ignore = 0,
                Verify = 1,
                Close = 2,
                AutoCache = 3,
                AutoCacheFlush = 4
            }

            [Flags]
            internal enum WinTrustDataProvFlags : uint
            {
                UseIe4TrustFlag = 0x1,
                NoIe4ChainFlag = 0x2,
                NoPolicyUsageFlag = 0x4,
                RevocationCheckNone = 0x10,
                RevocationCheckEndCert = 0x20,
                RevocationCheckChain = 0x40,
                RevocationCheckChainExcludeRoot = 0x80,
                SaferFlag = 0x100,
                HashOnlyFlag = 0x200,
                UseDefaultOsverCheck = 0x400,
                LifetimeSigningFlag = 0x800,
                CacheOnlyUrlRetrieval = 0x1000
            }

            public delegate int PfnCertEnumSystemStore(IntPtr pvSystemStore, uint dwFlags, ref CertSystemStoreInfo pStoreInfo, IntPtr pvReserved, IntPtr pvArg);

            [StructLayoutAttribute(LayoutKind.Sequential)]
            public struct CertSystemStoreInfo
            {

                /// DWORD->unsigned int
                public uint cbSize;
            }

            internal enum WinVerifyTrustResult : uint
            {
                Success = 0,
                ProviderUnknown = 0x800b0001, // The trust provider is not recognized on this system
                ActionUnknown = 0x800b0002, // The trust provider does not support the specified action
                SubjectFormUnknown = 0x800b0003, // The trust provider does not support the form specified for the subject
                SubjectNotTrusted = 0x800b0004 // The subject failed the specified verification action
            }

            [DllImport("crypt32.dll", EntryPoint = "CertEnumSystemStore")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CertEnumSystemStore(uint dwFlags, IntPtr pvSystemStoreLocationPara, IntPtr pvArg, PfnCertEnumSystemStore pfnEnum);

            [DllImport("crypt32.dll", EntryPoint = "CertOpenSystemStoreW")]
            public static extern IntPtr CertOpenSystemStoreW(uint hProv, [In] [MarshalAs(UnmanagedType.LPWStr)] string szSubsystemProtocol);


            [StructLayout(LayoutKind.Sequential)]
            internal struct FileImportInfo
            {
                public int Size;
                public int SubjectChoice;
                [MarshalAs(UnmanagedType.LPWStr)]
                public string Filename;
                public int Flags;
                public IntPtr Password;
            }

            [StructLayout(LayoutKind.Sequential)]
            internal struct ContextImportInfo
            {
                public int Size;
                public int SubjectChoice;
                public IntPtr context;
                public int Flags;
                public IntPtr Password;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CertificateContext
            {
                public uint EncodingType;
                public IntPtr data;
                public uint dataSize;
                public IntPtr certifcateInfo;
                public IntPtr certificateStore;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CryptAlgorithmIdentifier
            {
                [MarshalAs(UnmanagedType.LPStr)]
                public string pszObjId;
                public CryptAttributeBlob Parameters;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct Filetime
            {
                public uint dwLowDateTime;
                public uint dwHighDateTime;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CryptBitBlob
            {
                public uint dataSize;
                public IntPtr data;
                public uint unusedBitsCount;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CertificateInfo
            {
                public uint Version;
                public CryptAttributeBlob SerialNumber;
                public CryptAlgorithmIdentifier SignatureAlgorithm;
                public CryptAttributeBlob Issuer;
                public Filetime NotBefore;
                public Filetime NotAfter;
                public CryptAttributeBlob Subject;
                public CryptAlgorithmIdentifier SubjectPublicKeyInfo;
                public CryptBitBlob IssuerUniqueId;
                public CryptBitBlob SubjectUniqueId;
                public uint cExtension;
                public IntPtr rgExtension;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct CryptAttributeBlob : IDisposable
            {
                public int dataSize;
                public IntPtr data;

                public void Dispose()
                {
                    if (data != IntPtr.Zero)
                        Marshal.FreeHGlobal(data);
                }
            }

            [DllImport("CryptUI.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern Boolean CryptUIWizImport(
                Int32 flags,
                IntPtr parentWindowHandle,
                IntPtr wizardTitle,
                ref FileImportInfo importInfo,
                IntPtr certStore);

            [DllImport("CryptUI.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            internal static extern Boolean CryptUIWizImport(
                Int32 flags,
                IntPtr parentWindowHandle,
                IntPtr wizardTitle,
                ref ContextImportInfo importInfo,
                IntPtr certStore);

            [DllImport("Crypt32.DLL", EntryPoint = "CertCreateCertificateContext", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern IntPtr CertCreateCertificateContext(
                Int32 encodingType,
                byte[] data,
                Int32 dataSize);

            [DllImport("wintrust.dll", ExactSpelling = true, SetLastError = false, CharSet = CharSet.Unicode)]
            internal static extern WinVerifyTrustResult WinVerifyTrust(
                [In] IntPtr handle,
                [In] [MarshalAs(UnmanagedType.LPStruct)] Guid actionIdentifier,
                [In] WinTrustData data);

            [DllImport("crypt32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
            internal static extern IntPtr PFXImportCertStore(
                ref CryptAttributeBlob data,
                [In] IntPtr passwordPointer,
                uint flags);

            [DllImport("crypt32.dll", EntryPoint = "CertEnumCertificatesInStore")]
            internal static extern IntPtr CertEnumCertificatesInStore(
                IntPtr hCertStore,
                IntPtr context);

            [DllImport("crypt32.dll", EntryPoint = "CertSetCertificateContextProperty", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CertSetCertificateContextProperty(
                IntPtr context,
                int property,
                int flags,
                IntPtr data);

            [DllImport("crypt32.dll", EntryPoint = "CertCloseStore")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CertCloseStore(IntPtr store, uint flags);

            [DllImport("crypt32.dll", SetLastError = true)]
            internal static extern bool CertGetCertificateContextProperty(
            IntPtr context, uint property, IntPtr data, ref uint dataSize);

            [DllImport("crypt32.dll", EntryPoint = "CertFreeCertificateContext")]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool CertFreeCertificateContext(IntPtr context);


        }
        // ReSharper disable InconsistentNaming
        public enum KnownStores
        {
            MY,
            ROOT,
            SPC
        }
        // ReSharper restore InconsistentNaming

        public static BooleanReason IsFileSigned(string target)
        {
            if (!File.Exists(target))
                return new BooleanReason(false, "the file {0} does not exist", target);

            var data = new NativeMethods.WinTrustData(target, NativeMethods.WinTrustDataRevocationChecks.None);
            var result = NativeMethods.WinVerifyTrust(
                IntPtr.Zero,
                new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}"),
                data);

            if (result != NativeMethods.WinVerifyTrustResult.Success)
            {
                var issue = new Win32Exception(Marshal.GetLastWin32Error());
                return new BooleanReason(false, issue.Message);
            }

            return new BooleanReason(true, "");

        }

        /// <summary>
        /// Returns the X509 certificate for the executing version of the utility
        /// </summary>
        /// <returns>X509Certificate assigned to the utility or null if utility is not signed or an issue occurs</returns>
        public static X509Certificate GetUtilityCertificate()
        {
            return GetCertificateForFile(Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Returns the X509 certificate assigned to a target file
        /// </summary>
        /// <param name="target">The full path to the file in question</param>
        /// <returns>X509Certificate assigned to the file or null if utility is not signed or an issue occurs</returns>
        public static X509Certificate GetCertificateForFile(string target)
        {
            try
            {
                if (!File.Exists(target))
                    return null;

                var signedResult = IsFileSigned(target);
                return !signedResult.Result ? null : X509Certificate.CreateFromSignedFile(target);
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to retrieve the certificate for the file {0}: {1}", target, e);
                return null;
            }
        }

        /// <summary>
        /// Returns a BooleanReason determining whether a file is trusted.  For a file to be trusted, it must be signed with the same code-signing certificate
        /// used to sign the utility
        /// </summary>
        /// <param name="target">The file in question</param>
        /// <returns></returns>
        public static BooleanReason IsFileTrusted(string target)
        {
            var utilityCert = GetUtilityCertificate();
            if (utilityCert == null)
                return new BooleanReason(false, "could not retrieve utility certificate");

            if (!File.Exists(target))
                return new BooleanReason(false, "the file {0} does not exist", target);

            var signedResult = IsFileSigned(target);
            if (!signedResult.Result)
                return signedResult;

            var targetCert = GetCertificateForFile(target);
            if (targetCert == null)
                return new BooleanReason(false, "could not retrieve certificate for file {0}", target);

            return !targetCert.Equals(utilityCert) ? new BooleanReason(false, "the certificate for {0} does not equal that of the utility") : new BooleanReason(true, "");
        }

        public static BooleanReason ImportUserCertificateFromFile(string target, SecureString password)
        {
            var fileNamePointer = IntPtr.Zero;
            var passwordPointer = IntPtr.Zero;
            try
            {
                fileNamePointer = Marshal.StringToHGlobalUni(target);

                if (password != null)
                    passwordPointer = Marshal.SecureStringToGlobalAllocUnicode(password);

                var importInfo = new NativeMethods.FileImportInfo
                    {
                        SubjectChoice = NativeMethods.ImportFromFile,
                        Filename = target,
                        Password = passwordPointer
                    };
                importInfo.Size = Marshal.SizeOf(importInfo);

                var result =
                    NativeMethods.CryptUIWizImport(NativeMethods.NoUserInterface | NativeMethods.ImportToCurrentuser,
                                                   IntPtr.Zero, IntPtr.Zero, ref importInfo, IntPtr.Zero);
                var issue = new Win32Exception(Marshal.GetLastWin32Error());
                return !result
                           ? new BooleanReason(false, issue.Message)
                           : new BooleanReason(true, "");
            }
            finally
            {
                if (fileNamePointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(fileNamePointer);

                if (passwordPointer != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(passwordPointer);
            }
        }

        public static BooleanReason ImportCertificateFromBuffer(IntPtr certHandle, IntPtr storeHandle,
                                                                string friendlyName)
        {
            try
            {
                AssignFriendlyNameToContext(certHandle, friendlyName);

                var importInfo = new NativeMethods.ContextImportInfo
                    {
                        SubjectChoice = NativeMethods.ImportFromContext,
                        context = certHandle,
                    };
                importInfo.Size = Marshal.SizeOf(importInfo);
                var result = NativeMethods.CryptUIWizImport(NativeMethods.NoUserInterface, IntPtr.Zero, IntPtr.Zero,
                                                            ref importInfo, storeHandle);
                if (!result)
                {
                    var issue = new Win32Exception(Marshal.GetLastWin32Error());
                    return new BooleanReason(false, issue.Message);
                }

                return new BooleanReason(true, "");
            }
            catch (Exception e)
            {
                return new BooleanReason(false, e.Message);
            }

        }

        public static BooleanReason ImportUserCertificateFromBuffer(byte[] buffer, SecureString password, string friendlyName)
        {
            var info = new NativeMethods.CryptAttributeBlob();
            var temporaryStore = IntPtr.Zero;
            var context = IntPtr.Zero;
            var passwordPointer = IntPtr.Zero;
            try
            {
                if (buffer == null)
                    return new BooleanReason(false, "The provided certificate buffer is empty.");

                info.data = Marshal.AllocHGlobal(buffer.Length);
                info.dataSize = buffer.Length;

                if (password != null)
                    passwordPointer = Marshal.SecureStringToGlobalAllocUnicode(password);

                Marshal.Copy(buffer, 0, info.data, buffer.Length);

                temporaryStore = NativeMethods.PFXImportCertStore(ref info, passwordPointer, NativeMethods.UserKeyset);
                if (temporaryStore == IntPtr.Zero)
                {
                    var issue = new Win32Exception(Marshal.GetLastWin32Error());
                    return new BooleanReason(false, "Could not import cert data: " + issue.Message);
                }

                context = GetFirstUsableContextFromStore(temporaryStore);
                if (context == IntPtr.Zero)
                    return new BooleanReason(false, "Could not import cert data: no useable certificate in store");

                AssignFriendlyNameToContext(context, friendlyName);

                var importInfo = new NativeMethods.ContextImportInfo
                    {
                        SubjectChoice = NativeMethods.ImportFromContext,
                        context = context,
                        Password = passwordPointer
                    };
                importInfo.Size = Marshal.SizeOf(importInfo);

                var result = NativeMethods.CryptUIWizImport(NativeMethods.NoUserInterface | NativeMethods.ImportToCurrentuser, IntPtr.Zero, IntPtr.Zero, ref importInfo, IntPtr.Zero);
                if (!result)
                {
                    var issue = new Win32Exception(Marshal.GetLastWin32Error());
                    return new BooleanReason(false, issue.Message);
                }

                return new BooleanReason(true, "");
            }
            finally
            {
                if (context != IntPtr.Zero)
                    NativeMethods.CertFreeCertificateContext(context);

                if (info.data != IntPtr.Zero)
                    Marshal.FreeHGlobal(info.data);

                if (temporaryStore != IntPtr.Zero)
                    NativeMethods.CertCloseStore(temporaryStore, 0);

                if (passwordPointer != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(passwordPointer);
            }
        }

        private static void AssignFriendlyNameToContext(IntPtr context, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return;

            var dataPointer = IntPtr.Zero;
            try
            {
                using (var data = new NativeMethods.CryptAttributeBlob { data = Marshal.StringToHGlobalUni(value), dataSize = Encoding.Unicode.GetByteCount(value) })
                {
                    dataPointer = Marshal.AllocHGlobal(Marshal.SizeOf(data));
                    Marshal.StructureToPtr(data, dataPointer, false);

                    if (NativeMethods.CertSetCertificateContextProperty(
                        context, NativeMethods.CertFriendlyNamePropId, 0, dataPointer))
                        return;

                    var issue = new Win32Exception(Marshal.GetLastWin32Error());
                    Log.WarnFormat("Could not set friendly name for certificate: {0}", issue.Message);
                }
            }
            finally
            {
                if (dataPointer != IntPtr.Zero)
                    Marshal.FreeHGlobal(dataPointer);
            }
        }

        private static IntPtr GetFirstUsableContextFromStore(IntPtr store)
        {
            var propertyInfo = IntPtr.Zero;
            try
            {
                var context = IntPtr.Zero;
                while ((context = NativeMethods.CertEnumCertificatesInStore(store, context)) != IntPtr.Zero)
                {
                    uint infoSize = 0;
                    if (NativeMethods.CertGetCertificateContextProperty(context, NativeMethods.CertKeyProvInfoPropId,
                                                                        IntPtr.Zero, ref infoSize))
                        propertyInfo = Marshal.AllocHGlobal((int)infoSize);
                    else
                        continue;

                    if (
                        !NativeMethods.CertGetCertificateContextProperty(context, NativeMethods.CertKeyProvInfoPropId,
                                                                         propertyInfo, ref infoSize))
                        continue;

                    break;
                }

                return context;
            }
            finally
            {
                if (propertyInfo != IntPtr.Zero)
                    Marshal.FreeHGlobal(propertyInfo);
            }
        }

        public static string GetSubjectKeyFromCertificate(X509Certificate2 certificate)
        {
            foreach (var extension in certificate.Extensions.Cast<X509Extension>()
                .Where(extension => extension.Oid.Value.Equals(SubjectKeyOid)))
            {
                var subjectKeyExtension = extension as X509SubjectKeyIdentifierExtension;
                if (subjectKeyExtension == null)
                    continue;

                return subjectKeyExtension.SubjectKeyIdentifier;
            }

            return "";
        }

        public static string GetAuthorityKeyFromCertificate(X509Certificate2 certificate)
        {
            try
            {
                foreach (var extension in certificate.Extensions.Cast<X509Extension>()
                .Where(extension => extension.Oid.Value.Equals(AuthorityKeyOid)))
                {
                    using (var asnStream = new Asn1InputStream(extension.RawData))
                    {
                        var asnObject = asnStream.ReadObject();
                        var taggedObject = new DerTaggedObject(0, asnObject);

                        var authorityKey = AuthorityKeyIdentifier.GetInstance(taggedObject, true);
                        var octetString = new DerOctetString(authorityKey.GetKeyIdentifier());
                        return NormalizeOctetString(octetString.ToString());
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                Log.WarnFormat("An issue occurred while attempting to extract the authority key from a certificate: {0}", e.Message);
                return "";
            }
        }

        private static string NormalizeOctetString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            value = value.Replace("#", "");
            return value;
        }

        public static BooleanReason VerifyCertificate(X509Certificate2 certificate, X509RevocationFlag revocationFlag, X509RevocationMode mode, TimeSpan timeout, X509VerificationFlags verificationFlags)
        {
            try
            {
                var chain = new X509Chain
                    {
                        ChainPolicy =
                            {
                                RevocationFlag = revocationFlag,
                                RevocationMode = mode,
                                VerificationFlags = verificationFlags,
                                UrlRetrievalTimeout = timeout
                            }
                    };

                if (!chain.Build(certificate))
                {
                    if (!IsChainOk(chain))
                        return new BooleanReason(false, "Could not verify certificate");
                }

                return new BooleanReason(true, "certificate verified");
            }
            catch (Exception e)
            {
                return new BooleanReason(false, "An issue occurred while verifying certificate: {0}", e.Message);
            }
        }

        private static bool IsChainOk(X509Chain chain)
        {
            var issuesFound = false;
            foreach (var element in chain.ChainElements)
            {
                foreach (var statusEntry in element.ChainElementStatus)
                {
                    if (statusEntry.Status == X509ChainStatusFlags.NoError)
                        continue;

                    if (statusEntry.Status == X509ChainStatusFlags.RevocationStatusUnknown)
                    {
                        Log.WarnFormat("Ignoring unknown revocation status for cert {0}: {1} ({2})",
                            element.Certificate.PublicKey,
                            statusEntry.StatusInformation,
                            statusEntry.Status);
                        continue;
                    }

                    if (statusEntry.Status == X509ChainStatusFlags.OfflineRevocation)
                    {
                        Log.WarnFormat("Ignoring offline CRL issue for cert {0}: {1} ({2})",
                            element.Certificate.PublicKey,
                            statusEntry.StatusInformation,
                            statusEntry.Status);
                        continue;
                    }

                    Log.WarnFormat("Could not verify certificate {0}: {1} ({2})",
                            element.Certificate.PublicKey,
                            statusEntry.StatusInformation,
                            statusEntry.Status);

                    issuesFound = true;
                }
            }

            return !issuesFound;
        }
    }


}
