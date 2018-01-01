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

        public static EncryptedString ReadEncryptedString(this BinaryReader self, bool xbox = false)
        {
            ulong value = self.ReadUInt64();
            if (xbox)
                value = Util.ReverseBytes(value);
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

        public static ColorInfo ReadColorInfo(this BinaryReader self)
        {
            Colors color = (Colors)self.ReadByte();
            ColorType type = (ColorType)self.ReadByte();
            return new ColorInfo(color, type);
        }

        public static void WriteColorInfo(this BinaryWriter self, ColorInfo info)
        {
            self.Write((byte)info.Color);
            self.Write((byte)info.Type);
        }

        public static VehicleListData ReadVehicleList(this BinaryReader self, bool xbox = false)
        {
            VehicleListData list = new VehicleListData();

            int count = self.ReadInt32();
            int startOff = self.ReadInt32();

            list.Unknown1 = self.ReadInt32();
            list.Unknown2 = self.ReadInt32();

            if (xbox)
            {
                count = Util.ReverseBytes(count);
                startOff = Util.ReverseBytes(startOff);
                list.Unknown1 = Util.ReverseBytes(list.Unknown1);
                list.Unknown2 = Util.ReverseBytes(list.Unknown2);
            }

            for (int i = 0; i < count; i++)
            {
                Vehicle vehicle = new Vehicle();

                vehicle.Index = i;

                vehicle.ID = self.ReadEncryptedString(xbox);//ReadInt64();
                vehicle.Unknown3 = self.ReadInt64();
                vehicle.WheelType = Encoding.ASCII.GetString(self.ReadBytes(32));
                vehicle.CarName = Encoding.ASCII.GetString(self.ReadBytes(56));
                vehicle.NewUnknown = self.ReadInt64();
                vehicle.CarBrand = Encoding.ASCII.GetString(self.ReadBytes(32));
                vehicle.Unknown4 = self.ReadSingle();
                vehicle.Flags = self.ReadInt32();
                vehicle.Unknown6 = self.ReadInt16();
                vehicle.Unknown7 = self.ReadByte();
                vehicle.DisplayStrength = self.ReadByte();
                vehicle.Unknown8 = self.ReadInt32();
                vehicle.Unknown9 = self.ReadInt32();
                vehicle.Unknown10 = self.ReadInt32();
                //vehicle.ExhauseID = self.ReadInt64();
                vehicle.ExhauseID = self.ReadEncryptedString(xbox);
                vehicle.GroupID = self.ReadInt64();
                vehicle.GroupIDAlt = self.ReadInt64();
                //vehicle.EngineID = self.ReadInt64();
                vehicle.EngineID = self.ReadEncryptedString(xbox);
                vehicle.Unknown15 = self.ReadInt32();
                vehicle.Unknown16 = self.ReadInt32();
                vehicle.Unknown17 = self.ReadInt64();
                vehicle.Unknown18 = self.ReadInt64();
                vehicle.Unknown19 = self.ReadInt32();
                vehicle.Unknown20 = self.ReadInt32();
                vehicle.Unknown21 = self.ReadInt32();
                vehicle.Unknown22 = self.ReadInt32();
                vehicle.Unknown23 = self.ReadInt32();
                vehicle.Unknown24 = self.ReadInt32();
                vehicle.Category = self.ReadInt32();
                vehicle.BoostType = (BoostType)self.ReadByte();
                vehicle.FinishType = (FinishType)self.ReadByte();
                vehicle.MaxSpeedNoBoost = self.ReadByte();
                vehicle.MaxSpeedBoost = self.ReadByte();
                vehicle.DisplaySpeed = self.ReadByte();
                vehicle.DisplayBoost = self.ReadByte();
                vehicle.Color = self.ReadColorInfo();
                vehicle.Unknown28 = self.ReadInt32();

                if (xbox)
                {
                    //vehicle.ID = Util.ReverseBytes(vehicle.ID);
                    vehicle.Unknown3 = Util.ReverseBytes(vehicle.Unknown3);
                    vehicle.NewUnknown = Util.ReverseBytes(vehicle.NewUnknown);
                    vehicle.Unknown4 = Util.ReverseBytes(vehicle.Unknown4);
                    vehicle.Flags = Util.ReverseBytes(vehicle.Flags);
                    vehicle.Unknown6 = Util.ReverseBytes(vehicle.Unknown6);
                    vehicle.Unknown8 = Util.ReverseBytes(vehicle.Unknown8);
                    vehicle.Unknown9 = Util.ReverseBytes(vehicle.Unknown9);
                    vehicle.Unknown10 = Util.ReverseBytes(vehicle.Unknown10);
                    //vehicle.ExhauseID = Util.ReverseBytes(vehicle.ExhauseID);
                    vehicle.GroupID = Util.ReverseBytes(vehicle.GroupID);
                    vehicle.GroupIDAlt = Util.ReverseBytes(vehicle.GroupIDAlt);
                    //vehicle.EngineID = Util.ReverseBytes(vehicle.EngineID);
                    vehicle.Unknown15 = Util.ReverseBytes(vehicle.Unknown15);
                    vehicle.Unknown16 = Util.ReverseBytes(vehicle.Unknown16);
                    vehicle.Unknown17 = Util.ReverseBytes(vehicle.Unknown17);
                    vehicle.Unknown18 = Util.ReverseBytes(vehicle.Unknown18);
                    vehicle.Unknown19 = Util.ReverseBytes(vehicle.Unknown19);
                    vehicle.Unknown20 = Util.ReverseBytes(vehicle.Unknown20);
                    vehicle.Unknown21 = Util.ReverseBytes(vehicle.Unknown21);
                    vehicle.Unknown22 = Util.ReverseBytes(vehicle.Unknown22);
                    vehicle.Unknown23 = Util.ReverseBytes(vehicle.Unknown23);
                    vehicle.Unknown24 = Util.ReverseBytes(vehicle.Unknown24);
                    vehicle.Category = Util.ReverseBytes(vehicle.Category);
                    //vehicle.Unknown26 = Util.ReverseBytes(vehicle.Unknown26);
                    vehicle.Unknown28 = Util.ReverseBytes(vehicle.Unknown28);
                }

                list.Entries.Add(vehicle);
            }

            return list;
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

        public static void WriteVehicleList(this BinaryWriter self, VehicleListData list, bool console = false)
        {
            // TODO: Implement Console Saving

            self.Write(console ? Util.ReverseBytes((int)list.Entries.Count) : (int)list.Entries.Count);
            self.Write(console ? Util.ReverseBytes((int)0x10) : (int)0x10);

            self.Write(console ? Util.ReverseBytes(list.Unknown1) : list.Unknown1);
            self.Write(console ? Util.ReverseBytes(list.Unknown2) : list.Unknown2);

            for (int i = 0; i < list.Entries.Count; i++)
            {
                Vehicle vehicle = list.Entries[i];

                self.WriteEncryptedString(vehicle.ID, console);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown3) : vehicle.Unknown3);
                self.WriteLenString(vehicle.WheelType, 32, console);
                self.WriteLenString(vehicle.CarName, 56, console);
                self.Write(console ? Util.ReverseBytes(vehicle.NewUnknown) : vehicle.NewUnknown);
                self.WriteLenString(vehicle.CarBrand, 32, console);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown4) : vehicle.Unknown4);
                self.Write(console ? Util.ReverseBytes(vehicle.Flags) : vehicle.Flags);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown6) : vehicle.Unknown6);
                self.Write(vehicle.Unknown7);
                self.Write(vehicle.DisplayStrength);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown8) : vehicle.Unknown8);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown9) : vehicle.Unknown9);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown10) : vehicle.Unknown10);
                //self.Write(console ? Util.ReverseBytes(vehicle.ExhauseID) : vehicle.ExhauseID);
                self.WriteEncryptedString(vehicle.ExhauseID, console);
                self.Write(console ? Util.ReverseBytes(vehicle.GroupID) : vehicle.GroupID);
                self.Write(console ? Util.ReverseBytes(vehicle.GroupIDAlt) : vehicle.GroupIDAlt);
                //self.Write(console ? Util.ReverseBytes(vehicle.EngineID) : vehicle.EngineID);
                self.WriteEncryptedString(vehicle.EngineID, console);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown15) : vehicle.Unknown15);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown16) : vehicle.Unknown16);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown17) : vehicle.Unknown17);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown18) : vehicle.Unknown18);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown19) : vehicle.Unknown19);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown20) : vehicle.Unknown20);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown21) : vehicle.Unknown21);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown22) : vehicle.Unknown22);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown23) : vehicle.Unknown23);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown24) : vehicle.Unknown24);
                self.Write(console ? Util.ReverseBytes(vehicle.Category) : vehicle.Category);
                self.Write((byte)vehicle.BoostType);
                self.Write((byte)vehicle.FinishType);
                self.Write(vehicle.MaxSpeedNoBoost);
                self.Write(vehicle.MaxSpeedBoost);
                //self.Write(console ? Util.ReverseBytes(vehicle.Unknown26) : vehicle.Unknown26);
                self.Write(vehicle.DisplaySpeed);
                self.Write(vehicle.DisplayBoost);
                self.WriteColorInfo(vehicle.Color);
                self.Write(console ? Util.ReverseBytes(vehicle.Unknown28) : vehicle.Unknown28);
            }
        }
    }
}
