using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aurora.Api.Utils
{
    public static class ByteUtilsMethodEx
    {
        public static string GetMd5Checksum(this byte[] value)
        {
            var stream = new MemoryStream();
            stream.Write(value, 0, value.Length);

            //important: get back to start of stream
            stream.Seek(0, SeekOrigin.Begin);

            //get a string value for the MD5 hash.
            using var md5Instance = System.Security.Cryptography.MD5.Create();
            var hashResult = md5Instance.ComputeHash(stream);

            //***I did some formatting here, you may not want to remove the dashes, or use lower case depending on your application
            return BitConverter.ToString(hashResult).Replace("-", "").ToLowerInvariant();
        }
    }
}
