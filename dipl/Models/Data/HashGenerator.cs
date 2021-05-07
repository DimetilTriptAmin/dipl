using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dipl.Models.Data
{
    public class HashGenerator
    {
        private static SHA256 _generator = SHA256.Create();

        public static string GetHash(SecureString input)
        {
            IntPtr bstr = IntPtr.Zero;
            try
            {
                bstr = Marshal.SecureStringToBSTR(input);
                int length = Marshal.ReadInt32(bstr, -4);
                byte[] inputBytes = new byte[length];
                for (int x = 0; x < length; ++x)
                {
                    byte b = Marshal.ReadByte(bstr, x);
                    inputBytes[x] = b;
                }
                return (input != null && input.Length > 0) ? ByteArrayToString(GenerateByteArrayHash(inputBytes)) : "";
            }
            finally
            {
                if (bstr != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr);
            }

        }

        private static byte[] HashIteration(byte[] bytes)
        {
            return _generator.ComputeHash(bytes);
        }

        private static string ByteArrayToString(byte[] arr)
        {
            if (arr == null || arr.Length == 0) return "";

            string result = "";

            foreach (byte byteValue in arr)
            {
                result += byteValue.ToString("X2"); // X2 - hex
            }

            return result;
        }

        private static byte[] GenerateByteArrayHash(byte[] inputArray)
        {
            byte[] resultArray = inputArray;

            for (int i = 0; i < 8; i++)
            {
                resultArray = HashIteration(resultArray);
            }

            return resultArray;
        }
    }
}
