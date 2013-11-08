using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Org.InCommon.InCert.Utilities.GenerateFileWrapper
{
    public static class Extensions
    {
        /// <summary>
        /// Converts serializable object to XElement
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="instance">object in question</param>
        /// <returns>XElement version of serialize object</returns>
        /// <remarks>adapted from http://stackoverflow.com/questions/8373552/serialize-an-object-to-xelement-and-deserialize-it-in-memory</remarks>
        public static XElement ToXElement<T>(this object instance)
        {
            var stream = new MemoryStream();
            using (TextWriter writer = new StreamWriter(stream))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(writer, instance);
                return XElement.Parse(Encoding.ASCII.GetString(stream.ToArray()));
            }

        }

        public static bool IsSigned(this FileInfo target)
        {

            try
            {
                if (!File.Exists(target.FullName))
                    throw new Exception("The file does not exist");

                var data = new NativeMethods.WinTrustData(target.FullName, NativeMethods.WinTrustDataRevocationChecks.None);
                var result = NativeMethods.WinVerifyTrust(
                    IntPtr.Zero,
                    new Guid("{00AAC56B-CD44-11d0-8CC2-00C04FC295EE}"),
                    data);

                if (result != NativeMethods.WinVerifyTrustResult.Success)
                    throw new Win32Exception(Marshal.GetLastWin32Error());


                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("An issue occurred while verifying the target signature: {0}", e.Message);
                return false;
            }

        }

    }
}
