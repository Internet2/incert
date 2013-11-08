using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Org.InCommon.InCert.Engine.Utilities
{
    // adapted from http://stackoverflow.com/questions/8533105/minimum-c-sharp-code-to-extract-from-cab-archives-or-infopath-xsn-files-in-mem
    public static class CabArchiveUtilities
    {
        internal static class NativeMethods
        {
            [StructLayout(LayoutKind.Sequential)]
            internal class CabinetInfo //Cabinet API: "FDCABINETINFO"
            {
                public int cbCabinet;
                public Int16 cFolders;
                public Int16 cFiles;
                public Int16 setID;
                public Int16 iCabinet;
                public int fReserve;
                public int hasprev;
                public int hasnext;
            }

            //If any of these classes end up with a different size to its C equivilent, we end up with crash and burn.
            [StructLayout(LayoutKind.Sequential)]
            internal class CabError //Cabinet API: "ERF"
            {
                public int erfOper;
                public int erfType;
                public int fError;
            }

            [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
            internal class FdiNotification //Cabinet API: "FDINOTIFICATION"
            {
                public int cb;
                public string psz1;
                public string psz2;
                public string psz3;
                public IntPtr userData;
                public IntPtr hf;
                public Int16 date;
                public Int16 time;
                public Int16 attribs;
                public Int16 setID;
                public Int16 iCabinet;
                public Int16 iFolder;
                public int fdie;
            }

            internal enum FdiNotificationType
            {
                CabinetInfo,
                PartialFile,
                CopyFile,
                CloseFileInfo,
                NextCabinet,
                Enumerate
            }

            internal class DecompressFile
            {
                public IntPtr Handle { get; set; }
                public string Name { get; set; }
                public bool Found { get; set; }
                public int Length { get; set; }
                public byte[] Data { get; set; }
            }

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate IntPtr FdiMemAllocDelegate(int numBytes);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate void FdiMemFreeDelegate(IntPtr mem);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate IntPtr FdiFileOpenDelegate(string fileName, int oflag, int pmode);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Int32 FdiFileReadDelegate(IntPtr hf,
                                                      [In, Out] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2,
                                                          ArraySubType = UnmanagedType.U1)] byte[] buffer, int cb);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Int32 FdiFileWriteDelegate(IntPtr hf,
                                                       [In] [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2,
                                                           ArraySubType = UnmanagedType.U1)] byte[] buffer, int cb);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Int32 FdiFileCloseDelegate(IntPtr hf);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate Int32 FdiFileSeekDelegate(IntPtr hf, int dist, int seektype);

            [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
            internal delegate IntPtr FdiNotifyDelegate(
                FdiNotificationType fdint, [In] [MarshalAs(UnmanagedType.LPStruct)] FdiNotification fdin);

            [DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FDICreate", CharSet = CharSet.Unicode)]
            internal static extern IntPtr FdiCreate(
                FdiMemAllocDelegate fnMemAlloc,
                FdiMemFreeDelegate fnMemFree,
                FdiFileOpenDelegate fnFileOpen,
                FdiFileReadDelegate fnFileRead,
                FdiFileWriteDelegate fnFileWrite,
                FdiFileCloseDelegate fnFileClose,
                FdiFileSeekDelegate fnFileSeek,
                int cpuType,
                [MarshalAs(UnmanagedType.LPStruct)] CabError erf);

            [DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FDIIsCabinet", CharSet = CharSet.Unicode)]
            internal static extern bool FdiIsCabinet(
                IntPtr hfdi,
                IntPtr hf,
                [MarshalAs(UnmanagedType.LPStruct)] CabinetInfo cabInfo);

            [DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FDIDestroy", CharSet = CharSet.Unicode)]
            internal static extern bool FdiDestroy(IntPtr hfdi);

            [DllImport("cabinet.dll", CallingConvention = CallingConvention.Cdecl, EntryPoint = "FDICopy", CharSet = CharSet.Unicode)]
            internal static extern bool FdiCopy(
                IntPtr hfdi,
                string cabinetName,
                string cabinetPath,
                int flags,
                FdiNotifyDelegate fnNotify,
                IntPtr fnDecrypt,
                IntPtr userData);
        }

        internal class CabArchive : IDisposable
        {
            private const Int16 FileAttributeDirectory = 16;

            private readonly NativeMethods.FdiFileCloseDelegate _fileCloseDelegate;
            private readonly NativeMethods.FdiFileOpenDelegate _fileOpenDelegate;
            private readonly NativeMethods.FdiFileReadDelegate _fileReadDelegate;
            private readonly NativeMethods.FdiFileSeekDelegate _fileSeekDelegate;
            private readonly NativeMethods.FdiFileWriteDelegate _fileWriteDelegate;
            private readonly NativeMethods.FdiMemAllocDelegate _femAllocDelegate;
            private readonly NativeMethods.FdiMemFreeDelegate _memFreeDelegate;

            private readonly NativeMethods.CabError _erf;
            private readonly List<NativeMethods.DecompressFile> _decompressFiles;
            private readonly List<string> _presentFiles;
            private readonly byte[] _inputData;
            private IntPtr _hfdi;
            private bool _disposed;
            private const int CpuTypeUnknown = -1;

            public CabArchive(byte[] data)
            {
                _fileReadDelegate = FileRead;
                _fileOpenDelegate = InputFileOpen;
                _femAllocDelegate = MemAlloc;
                _fileSeekDelegate = FileSeek;
                _memFreeDelegate = MemFree;
                _fileWriteDelegate = FileWrite;
                _fileCloseDelegate = InputFileClose;
                _inputData = data;
                _decompressFiles = new List<NativeMethods.DecompressFile>();
                _presentFiles = new List<string>();
                _erf = new NativeMethods.CabError();
                _hfdi = IntPtr.Zero;
            }

            private static IntPtr FdiCreate(
                NativeMethods.FdiMemAllocDelegate fnMemAlloc,
                NativeMethods.FdiMemFreeDelegate fnMemFree,
                NativeMethods.FdiFileOpenDelegate fnFileOpen,
                NativeMethods.FdiFileReadDelegate fnFileRead,
                NativeMethods.FdiFileWriteDelegate fnFileWrite,
                NativeMethods.FdiFileCloseDelegate fnFileClose,
                NativeMethods.FdiFileSeekDelegate fnFileSeek,
                NativeMethods.CabError erf)
            {
                return NativeMethods.FdiCreate(fnMemAlloc, fnMemFree, fnFileOpen, fnFileRead, fnFileWrite,
                                               fnFileClose, fnFileSeek, CpuTypeUnknown, erf);
            }

            // ReSharper disable UnusedMethodReturnValue.Local
            private static bool FdiCopy(IntPtr hfdi, NativeMethods.FdiNotifyDelegate fnNotify)
            // ReSharper restore UnusedMethodReturnValue.Local
            {
                return NativeMethods.FdiCopy(hfdi, "<notused>", "<notused>", 0, fnNotify, IntPtr.Zero, IntPtr.Zero);
            }

            private IntPtr FdiContext
            {
                get
                {
                    if (_hfdi == IntPtr.Zero)
                    {
                        _hfdi = FdiCreate(_femAllocDelegate, _memFreeDelegate, _fileOpenDelegate, _fileReadDelegate,
                                          _fileWriteDelegate, _fileCloseDelegate, _fileSeekDelegate, _erf);
                        if (_hfdi == IntPtr.Zero)
                            throw new ApplicationException("Failed to create FDI context.");
                    }
                    return _hfdi;
                }
            }

            public void Dispose()
            {
                if (_disposed) return;

                if (_hfdi != IntPtr.Zero)
                {
                    NativeMethods.FdiDestroy(_hfdi);
                    _hfdi = IntPtr.Zero;
                }
                _disposed = true;
            }

            private IntPtr NotifyCallback(NativeMethods.FdiNotificationType fdint, NativeMethods.FdiNotification fdin)
            {
                switch (fdint)
                {
                    case NativeMethods.FdiNotificationType.CopyFile:
                        return OutputFileOpen(fdin);
                    case NativeMethods.FdiNotificationType.CloseFileInfo:
                        return OutputFileClose(fdin);
                    default:
                        return IntPtr.Zero;
                }
            }

            private IntPtr InputFileOpen(string fileName, int oflag, int pmode)
            {
                var stream = new MemoryStream(_inputData);
                var gch = GCHandle.Alloc(stream);
                return (IntPtr)gch;
            }

            private static int InputFileClose(IntPtr hf)
            {
                var stream = StreamFromHandle(hf);
                stream.Close();
                ((GCHandle)(hf)).Free();
                return 0;
            }

            private IntPtr OutputFileOpen(NativeMethods.FdiNotification fdin)
            {
                if (!_decompressFiles.Any())
                {

                    if ((fdin.attribs & FileAttributeDirectory) == 0)
                    {
                        // don't want to add entries with the folder attribute set
                        _presentFiles.Add(fdin.psz1);
                    }

                    return IntPtr.Zero;
                }

                var extractFile = _decompressFiles.SingleOrDefault(ef => ef.Name.Equals(fdin.psz1, StringComparison.InvariantCulture));
                if (extractFile != null)
                {
                    var stream = new MemoryStream();
                    var gch = GCHandle.Alloc(stream);
                    extractFile.Handle = (IntPtr)gch;
                    return extractFile.Handle;
                }

                //Don't extract
                return IntPtr.Zero;
            }

            private IntPtr OutputFileClose(NativeMethods.FdiNotification fdin)
            {
                var extractFile = _decompressFiles.Single(ef => ef.Handle == fdin.hf);
                var stream = StreamFromHandle(fdin.hf);

                extractFile.Found = true;
                extractFile.Length = (int)stream.Length;

                if (stream.Length > 0)
                {
                    extractFile.Data = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(extractFile.Data, 0, (int)stream.Length);
                }

                stream.Close();
                return IntPtr.Zero;
            }

            private static int FileRead(IntPtr hf, byte[] buffer, int cb)
            {
                var stream = StreamFromHandle(hf);
                return stream.Read(buffer, 0, cb);
            }

            private static int FileWrite(IntPtr hf, byte[] buffer, int cb)
            {
                var stream = StreamFromHandle(hf);
                stream.Write(buffer, 0, cb);
                return cb;
            }

            private static Stream StreamFromHandle(IntPtr hf)
            {
                return (Stream)((GCHandle)hf).Target;
            }

            private static IntPtr MemAlloc(int cb)
            {
                return Marshal.AllocHGlobal(cb);
            }

            private static void MemFree(IntPtr mem)
            {
                Marshal.FreeHGlobal(mem);
            }

            private static int FileSeek(IntPtr hf, int dist, int seektype)
            {
                var stream = StreamFromHandle(hf);
                return (int)stream.Seek(dist, (SeekOrigin)seektype);
            }

            public byte[] ExtractFile(string fileName)
            {
                if (_disposed)
                    throw new ObjectDisposedException("CabArchive");

                var fileToDecompress = new NativeMethods.DecompressFile { Found = false, Name = fileName };
                _decompressFiles.Clear();
                _decompressFiles.Add(fileToDecompress);

                FdiCopy(FdiContext, NotifyCallback);

                if (fileToDecompress.Found)
                {
                    _decompressFiles.Remove(fileToDecompress);
                    return fileToDecompress.Data;
                }

                return null;
            }

            internal bool IsCabinetFile()
            {
                if (_disposed)
                    throw new ObjectDisposedException("CabArchive");

                var stream = new MemoryStream(_inputData);
                var gch = GCHandle.Alloc(stream);

                try
                {
                    var info = new NativeMethods.CabinetInfo();
                    return NativeMethods.FdiIsCabinet(FdiContext, (IntPtr)gch, info);
                }
                finally
                {
                    stream.Close();
                    gch.Free();
                }
            }

            internal List<string> GetArchiveFileList()
            {
                if (_disposed)
                    throw new ObjectDisposedException("CabArchive");

                _decompressFiles.Clear();
                _presentFiles.Clear();
                FdiCopy(FdiContext, NotifyCallback);

                return new List<string>(_presentFiles);
            }
        }

        public static bool IsCabinetFile(byte[] inputData)
        {
            using (var decomp = new CabArchive(inputData))
            {
                return decomp.IsCabinetFile();
            }
        }

        public static byte[] ExtractFile(string archivePath, string fileName)
        {
            var buffer = File.ReadAllBytes(archivePath);
            using (var decomp = new CabArchive(buffer))
            {
                return decomp.ExtractFile(fileName);
            }
        }

        public static List<string> GetFilesInArchive(string archivePath)
        {
            var buffer = File.ReadAllBytes(archivePath);
            using (var decomp = new CabArchive(buffer))
            {
                return decomp.GetArchiveFileList();
            }
        }
    }
}
