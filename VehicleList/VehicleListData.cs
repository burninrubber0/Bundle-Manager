using System;
using System.Collections.Generic;
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

    public class VehicleListData
    {
        public int Unknown1;
        public int Unknown2;
        public List<Vehicle> Entries;

        public VehicleListData()
        {
            Entries = new List<Vehicle>();
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
        public byte Unknown7;
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
            Unknown7 = copy.Unknown7;
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
