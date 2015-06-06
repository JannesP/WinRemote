using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinRemote_Server.Util
{
    static class Utility
    {
        public static void ArrayCopy(byte[] src, ref byte[] dst) { ArrayCopy(src, 0, ref dst, 0, src.Length); }
        public static void ArrayCopy(byte[] src, ref byte[] dst, int dstPos) { ArrayCopy(src, 0, ref dst, dstPos, src.Length); }
        public static void ArrayCopy(byte[] src, int srcPos, ref byte[] dst, int dstPos) { ArrayCopy(src, srcPos, ref dst, dstPos, src.Length); }

        public static void ArrayCopy(byte[] src, int srcPos, ref byte[] dst, int dstPos, int length)
        {
            for (int i = 0; i < length; i++)
            {
                dst[i + dstPos] = src[i + srcPos];
            }
        }

        public static string ArrayToReadableString(byte[] array)
        {
            string res = "{ ";
            for (int i = 0; i < array.Length; i++)
            {
                res += array[i].ToString();
                if (i == (array.Length - 1))
                {
                    res += " }";
                }
                else
                {
                    res += ", ";
                }
            }
            return res;
        }

        public static int ReadIntFromByteArray(byte[] input, int startIndex)
        {
            if (input.Length - 4 < startIndex) throw new ArgumentOutOfRangeException("The startIndex was too big.");

            int result = 0;

            for (int i = 3; i >= 0; i--)
            {
                result |= input[startIndex + i] << (8 * i);
            }

            return result;
        }

        public static byte[] CreateBytesFromInt(int num)
        {
            byte[] byteNum = new byte[4];

            for (int i = 0; i < byteNum.Length; i++)
            {
                byte x = (byte)(num >> (8 * i) & 0xFF);
                byteNum[i] = x;
            }

            return byteNum;
        }

    }
}
