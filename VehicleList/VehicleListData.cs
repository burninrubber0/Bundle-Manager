using BundleFormat;
using BundleUtilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleList
{
    public enum BoostType
    {
        SPEED,
        AGGRESSION,
        STUNT,
        NONE,
        LOCKED,
        BIKE = 19
    }

    public enum FinishType
    {
        PARENTAL,
        FINISH,
        BURNING_ROUTE,
        GOLD_FINISH,
        PLATINUM_FINISH,
        COMMUNITY_FINISH
    }

    public enum Colors
    {
        RED,
        ULTRA_MARINE,
        LIME,
        NAVY_BLUE,
        ORANGE,
        WHITE,
        BLACK,
        YELLOW,
        MAROON,
        GREY,
        PURPLE,
        DARK_RED,
        GREEN,
        SAPPHIRE,
        CYAN,
        SCARLET,
        LUCKY_POINT,
        GREENISH_BLUE,
        TUSSOCK,
        EGYPTION_BLUE,
        TICKLE_ME_PINK,
        ELECTRIC_LIME,
        KASHMIR_BLUE,
        DARK_BROWN,
        DARK_GREEN
    }

    public enum ColorType
    {
        GLOSS,
        METALLIC,
        PEARLESCENT,
        GOLD_OR_PLATINUM,
        UNKNOWN
    }

    public struct ColorInfo
    {
        public Colors Color { get; set; }
        public ColorType Type { get; set; }

        public ColorInfo(Colors color, ColorType type)
        {
            Color = color;
            Type = type;
        }

        public override string ToString()
        {
            return Color + " | " + Type;
        }
    }

    public class VehicleListData : IEntryData
	{
        public int Unknown1;
        public int Unknown2;
        public List<Vehicle> Entries;

        public VehicleListData()
        {
            Entries = new List<Vehicle>();
        }

		public IEntryEditor GetEditor(BundleEntry entry)
		{
			VehicleListForm vehicleList = new VehicleListForm();
			vehicleList.List = this;
			vehicleList.Edit += () =>
			{
				Write(entry);
				/*entry = vehicleList.Write(entry.Console);
				CurrentArchive.Entries[index] = entry;
				CurrentArchive.Entries[index].Dirty = true;*/
			};

			return vehicleList;
		}

		public EntryType GetEntryType(BundleEntry entry)
		{
			return EntryType.VehicleListResourceType;
		}

		private void Clear()
		{
			Unknown1 = default;
			Unknown2 = default;
			Entries.Clear();
		}

		public bool Read(BundleEntry entry)
		{
			Clear();

			MemoryStream ms = new MemoryStream(entry.Header);
			BinaryReader2 br = new BinaryReader2(ms);
			br.BigEndian = entry.Console;
			//currentList = mbr.ReadVehicleList();

			int count = br.ReadInt32();
			int startOff = br.ReadInt32();

			Unknown1 = br.ReadInt32();
			Unknown2 = br.ReadInt32();

			for (int i = 0; i < count; i++)
			{
				Vehicle vehicle = new Vehicle();

				vehicle.Index = i;

				vehicle.ID = br.ReadEncryptedString();//ReadInt64();
				vehicle.Unknown3 = br.ReadInt64();
				vehicle.WheelType = Encoding.ASCII.GetString(br.ReadBytes(32));
				vehicle.CarName = Encoding.ASCII.GetString(br.ReadBytes(56));
				vehicle.NewUnknown = br.ReadInt64();
				vehicle.CarBrand = Encoding.ASCII.GetString(br.ReadBytes(32));
				vehicle.Unknown4 = br.ReadSingle();
				vehicle.Flags = br.ReadInt32();
				vehicle.Unknown6 = br.ReadInt16();
				vehicle.DisplayControl = br.ReadByte();
				vehicle.DisplayStrength = br.ReadByte();
				vehicle.Unknown8 = br.ReadInt32();
				vehicle.Unknown9 = br.ReadInt32();
				vehicle.Unknown10 = br.ReadInt32();
				//vehicle.ExhauseID = mbr.ReadInt64();
				vehicle.ExhauseID = br.ReadEncryptedString();
				vehicle.GroupID = br.ReadInt64();
				vehicle.GroupIDAlt = br.ReadInt64();
				//vehicle.EngineID = mbr.ReadInt64();
				vehicle.EngineID = br.ReadEncryptedString();
				vehicle.Unknown15 = br.ReadInt32();
				vehicle.Unknown16 = br.ReadInt32();
				vehicle.Unknown17 = br.ReadInt64();
				vehicle.Unknown18 = br.ReadInt64();
				vehicle.Unknown19 = br.ReadInt32();
				vehicle.Unknown20 = br.ReadInt32();
				vehicle.Unknown21 = br.ReadInt32();
				vehicle.Unknown22 = br.ReadInt32();
				vehicle.Unknown23 = br.ReadInt32();
				vehicle.Unknown24 = br.ReadInt32();
				vehicle.Category = br.ReadInt32();
				vehicle.BoostType = (BoostType)br.ReadByte();
				vehicle.FinishType = (FinishType)br.ReadByte();
				vehicle.MaxSpeedNoBoost = br.ReadByte();
				vehicle.MaxSpeedBoost = br.ReadByte();
				vehicle.DisplaySpeed = br.ReadByte();
				vehicle.DisplayBoost = br.ReadByte();
				vehicle.Color = br.ReadColorInfo();
				vehicle.Unknown28 = br.ReadInt32();

				Entries.Add(vehicle);
			}

			br.Close();
			return true;
		}

		public bool Write(BundleEntry entry)
		{
			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);

			bool console = entry.Console;
			// TODO: Implement Console Saving

			bw.Write(console ? Util.ReverseBytes((int)Entries.Count) : (int)Entries.Count);
			bw.Write(console ? Util.ReverseBytes((int)0x10) : (int)0x10);

			bw.Write(console ? Util.ReverseBytes(Unknown1) : Unknown1);
			bw.Write(console ? Util.ReverseBytes(Unknown2) : Unknown2);

			for (int i = 0; i < Entries.Count; i++)
			{
				Vehicle vehicle = Entries[i];

				bw.WriteEncryptedString(vehicle.ID, console);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown3) : vehicle.Unknown3);
				bw.WriteLenString(vehicle.WheelType, 32, console);
				bw.WriteLenString(vehicle.CarName, 56, console);
				bw.Write(console ? Util.ReverseBytes(vehicle.NewUnknown) : vehicle.NewUnknown);
				bw.WriteLenString(vehicle.CarBrand, 32, console);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown4) : vehicle.Unknown4);
				bw.Write(console ? Util.ReverseBytes(vehicle.Flags) : vehicle.Flags);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown6) : vehicle.Unknown6);
				bw.Write(vehicle.DisplayControl);
				bw.Write(vehicle.DisplayStrength);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown8) : vehicle.Unknown8);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown9) : vehicle.Unknown9);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown10) : vehicle.Unknown10);
				//bw.Write(console ? Util.ReverseBytes(vehicle.ExhauseID) : vehicle.ExhauseID);
				bw.WriteEncryptedString(vehicle.ExhauseID, console);
				bw.Write(console ? Util.ReverseBytes(vehicle.GroupID) : vehicle.GroupID);
				bw.Write(console ? Util.ReverseBytes(vehicle.GroupIDAlt) : vehicle.GroupIDAlt);
				//bw.Write(console ? Util.ReverseBytes(vehicle.EngineID) : vehicle.EngineID);
				bw.WriteEncryptedString(vehicle.EngineID, console);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown15) : vehicle.Unknown15);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown16) : vehicle.Unknown16);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown17) : vehicle.Unknown17);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown18) : vehicle.Unknown18);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown19) : vehicle.Unknown19);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown20) : vehicle.Unknown20);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown21) : vehicle.Unknown21);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown22) : vehicle.Unknown22);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown23) : vehicle.Unknown23);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown24) : vehicle.Unknown24);
				bw.Write(console ? Util.ReverseBytes(vehicle.Category) : vehicle.Category);
				bw.Write((byte)vehicle.BoostType);
				bw.Write((byte)vehicle.FinishType);
				bw.Write(vehicle.MaxSpeedNoBoost);
				bw.Write(vehicle.MaxSpeedBoost);
				//bw.Write(console ? Util.ReverseBytes(vehicle.Unknown26) : vehicle.Unknown26);
				bw.Write(vehicle.DisplaySpeed);
				bw.Write(vehicle.DisplayBoost);
				bw.WriteColorInfo(vehicle.Color);
				bw.Write(console ? Util.ReverseBytes(vehicle.Unknown28) : vehicle.Unknown28);
			}

			bw.Flush();
			byte[] data = ms.ToArray();
			bw.Close();
			ms.Close();

			entry.Header = data;
			entry.Dirty = true;

			return true;
		}
	}

    public class Vehicle
    {
        public int Index;
        public EncryptedString ID;
        public long Unknown3;
        public string WheelType;
        public string CarName;
        public long NewUnknown;
        public string CarBrand;
        public float Unknown4; // float
        public int Flags;
        public short Unknown6;
        public byte DisplayControl; // unused in released builds of burnout
        public byte DisplayStrength;
        public int Unknown8;
        public int Unknown9;
        public int Unknown10;
        public EncryptedString ExhauseID;
        public long GroupID;
        public long GroupIDAlt;
        public EncryptedString EngineID;
        public int Unknown15;
        public int Unknown16;
        public long Unknown17;
        public long Unknown18;
        public int Unknown19;
        public int Unknown20;
        public int Unknown21;
        public int Unknown22;
        public int Unknown23;
        public int Unknown24;
        public int Category;
        public BoostType BoostType;
        public FinishType FinishType;
        public byte MaxSpeedNoBoost;
        public byte MaxSpeedBoost;
        public byte DisplaySpeed;
        public byte DisplayBoost;
        public ColorInfo Color;
        public int Unknown28;

        public Vehicle()
        {

        }

        public Vehicle(Vehicle copy)
        {
            Index = copy.Index;
            ID = copy.ID;
            Unknown3 = copy.Unknown3;
            WheelType = copy.WheelType;
            CarName = copy.CarName;
            CarBrand = copy.CarBrand;
            Unknown4 = copy.Unknown4;
            Flags = copy.Flags;
            Unknown6 = copy.Unknown6;
            DisplayControl = copy.DisplayControl;
            DisplayStrength = copy.DisplayStrength;
            Unknown8 = copy.Unknown8;
            Unknown9 = copy.Unknown9;
            Unknown10 = copy.Unknown10;
            ExhauseID = copy.ExhauseID;
            GroupID = copy.GroupID;
            GroupIDAlt = copy.GroupIDAlt;
            EngineID = copy.EngineID;
            Unknown15 = copy.Unknown15;
            Unknown16 = copy.Unknown16;
            Unknown17 = copy.Unknown17;
            Unknown18 = copy.Unknown18;
            Unknown19 = copy.Unknown19;
            Unknown20 = copy.Unknown20;
            Unknown21 = copy.Unknown21;
            Unknown22 = copy.Unknown22;
            Unknown23 = copy.Unknown23;
            Unknown24 = copy.Unknown24;
            Category = copy.Category;
            BoostType = copy.BoostType;
            FinishType = copy.FinishType;
            MaxSpeedNoBoost = copy.MaxSpeedNoBoost;
            MaxSpeedBoost = copy.MaxSpeedBoost;
            Color = copy.Color;
            DisplaySpeed = copy.DisplaySpeed;
            DisplayBoost = copy.DisplayBoost;
            Unknown28 = copy.Unknown28;
        }
    }
}
