using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BundleFormat
{
    public static class Util
    {
        // reverse byte order (16-bit)
        public static UInt16 ReverseBytes(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }

        // reverse byte order (32-bit)
        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }

        // reverse byte order (64-bit)
        public static UInt64 ReverseBytes(UInt64 value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
                   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }

        // reverse byte order (16-bit)
        public static Int16 ReverseBytes(Int16 value)
        {
            UInt16 v = (UInt16)value;
            return (Int16)((v & 0xFFU) << 8 | (v & 0xFF00U) >> 8);
        }

        // reverse byte order (32-bit)
        public static Int32 ReverseBytes(Int32 value)
        {
            UInt32 v = (UInt32)value;
            return (Int32)((v & 0x000000FFU) << 24 | (v & 0x0000FF00U) << 8 |
                   (v & 0x00FF0000U) >> 8 | (v & 0xFF000000U) >> 24);
        }

        // reverse byte order (64-bit)
        public static Int64 ReverseBytes(Int64 value)
        {
            UInt64 v = (UInt64)value;
            return (Int64)((v & 0x00000000000000FFUL) << 56 | (v & 0x000000000000FF00UL) << 40 |
                   (v & 0x0000000000FF0000UL) << 24 | (v & 0x00000000FF000000UL) << 8 |
                   (v & 0x000000FF00000000UL) >> 8 | (v & 0x0000FF0000000000UL) >> 24 |
                   (v & 0x00FF000000000000UL) >> 40 | (v & 0xFF00000000000000UL) >> 56);
        }
    }
}
