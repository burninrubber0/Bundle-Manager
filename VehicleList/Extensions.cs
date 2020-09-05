using BundleUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleList
{
    public static class Extensions
    {
        public static string MakeString(this byte[] self, int len, bool trim = false)
        {
            StringBuilder sb = new StringBuilder();

            if (len < self.Length)
                len = self.Length;

            for (int i = 0; i < len; i++)
            {
                if (i >= self.Length)
                {
                    sb.Append("00");
                }
                else
                {
                    if ((self[i] == 0x20 || self[i] == 0x00) && trim)
                        sb.Append(" ");
                    else
                        sb.Append(self[i].ToString("X2"));
                }
            }

            return sb.ToString();
        }

        public static EncryptedString ReadEncryptedString(this BinaryReader self)
        {
            ulong value = self.ReadUInt64();
            EncryptedString id = new EncryptedString(value);
            return id;
        }

        public static void WriteEncryptedString(this BinaryWriter self, EncryptedString id, bool xbox = false)
        {
            ulong value = id.Encrypted;
            if (xbox)
                value = Util.ReverseBytes(value);
            self.Write(value);
        }

        public static void WriteLenString(this BinaryWriter self, string s, int len, bool console = false)
        {
            if (console)
            {
                for (int i = len; i >= 0; i--)
                {
                    if (i < s.Length)
                        self.Write((byte)s[i]);
                    else
                        self.Write((byte)0);
                }
            }
            else
            {
                for (int i = 0; i < len; i++)
                {
                    if (i < s.Length)
                        self.Write((byte)s[i]);
                    else
                        self.Write((byte)0);
                }
            }
        }
    }
}
