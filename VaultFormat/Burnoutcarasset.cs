using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BundleUtilities;

namespace VaultFormat
{
    public class Burnoutcarasset : IAttribute
    {
        public RefSpec[] Offences { get; set; } //RefSpec[12]
        public RefSpec SoundExhaustAsset { get; set; }
        public RefSpec SoundEngineAsset { get; set; }
        public RefSpec PhysicsVehicleHandlingAsset { get; set; }
        public RefSpec GraphicsAsset { get; set; }
        public RefSpec CarUnlockShot { get; set; }
        public RefSpec CameraExternalBehaviourAsset { get; set; }
        public RefSpec CameraBumperBehaviourAsset { get; set; }
        public string VehicleID { get; set; } //See StrE	
        public long PhysicsAsset { get; set; }
        public long MasterSceneMayaBinaryFile { get; set; }
        public string InGameName { get; set; } // See StrE
        public long GameplayAsset { get; set; }
        public string ExhaustName { get; set; } //See StrE
        public long ExhaustEntityKey { get; set; }
        public string EngineName { get; set; } //See StrE
        public long EngineEntityKey { get; set; }
        public long DefaultWheel { get; set; }
        public bool BuildThisVehicle { get; set; }

        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }

        public Burnoutcarasset(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>();
            foreach (RefSpec r in Offences)
            {
                bytes.Add(r.ToBytes()); //RefSpec[12]
            }
            bytes.Add(SoundExhaustAsset.ToBytes());
            bytes.Add(SoundEngineAsset.ToBytes());
            bytes.Add(PhysicsVehicleHandlingAsset.ToBytes());
            bytes.Add(GraphicsAsset.ToBytes());
            bytes.Add(CarUnlockShot.ToBytes());
            bytes.Add(CameraExternalBehaviourAsset.ToBytes());
            bytes.Add(CameraBumperBehaviourAsset.ToBytes());
            bytes.Add(Encoding.UTF8.GetBytes(VehicleID));
            bytes.Add(BitConverter.GetBytes(PhysicsAsset));
            bytes.Add(BitConverter.GetBytes(MasterSceneMayaBinaryFile));
            bytes.Add(Encoding.UTF8.GetBytes(InGameName));
            bytes.Add(BitConverter.GetBytes(GameplayAsset));
            bytes.Add(Encoding.UTF8.GetBytes(ExhaustName));
            bytes.Add(BitConverter.GetBytes(ExhaustEntityKey));
            bytes.Add(Encoding.UTF8.GetBytes(EngineName));
            bytes.Add(BitConverter.GetBytes(EngineEntityKey));
            bytes.Add(BitConverter.GetBytes(DefaultWheel));
            bytes.Add(BitConverter.GetBytes(BuildThisVehicle));
            // To-Do: The program want 3 bytes for some reason
            Console.WriteLine("Before " + bytes.SelectMany(i => i).Count());
            int newValue = AddPadding(bytes).Count();
            Console.WriteLine("After " + newValue);
            return newValue;
        }

        private byte[] AddPadding(List<byte[]> bytes)
        {
            List<byte> padding = new List<byte>();
            padding.Add((byte)0);
            padding.Add((byte)0);
            padding.Add((byte)0);
            bytes.Add(padding.ToArray());
            return bytes.SelectMany(i => i).ToArray();
        }

        public AttributeHeader getHeader()
        {
            return header;
        }

        public SizeAndPositionInformation getInfo()
        {
            return info;
        }

        public void Read(ILoader loader, BinaryReader2 br)
        {
            Offences = new RefSpec[12];
            for (int i = 0; i < 12; i++)
            {
                Offences[i] = new RefSpec(loader, br);
            }
            SoundExhaustAsset = new RefSpec(loader, br);
            SoundEngineAsset = new RefSpec(loader, br);
            PhysicsVehicleHandlingAsset = new RefSpec(loader, br);
            GraphicsAsset = new RefSpec(loader, br);
            CarUnlockShot = new RefSpec(loader, br);
            CameraExternalBehaviourAsset = new RefSpec(loader, br);
            CameraBumperBehaviourAsset = new RefSpec(loader, br);
            VehicleID = br.ReadCStr();
            PhysicsAsset = br.ReadInt64();
            MasterSceneMayaBinaryFile = br.ReadInt64();
            InGameName = br.ReadCStr();
            GameplayAsset = br.ReadInt64();
            ExhaustName = br.ReadCStr();
            ExhaustEntityKey = br.ReadInt64();
            EngineName = br.ReadCStr();
            EngineEntityKey = br.ReadInt64();
            DefaultWheel = br.ReadInt64();
            BuildThisVehicle = br.ReadBoolean();
            br.SkipUniquePadding(3);
        }

        public void Write(BinaryWriter wr)
        {
            foreach (RefSpec r in Offences)
            {
                r.Write(wr);
            }
            SoundExhaustAsset.Write(wr);
            SoundEngineAsset.Write(wr);
            PhysicsVehicleHandlingAsset.Write(wr);
            GraphicsAsset.Write(wr);
            CarUnlockShot.Write(wr);
            CameraExternalBehaviourAsset.Write(wr);
            CameraBumperBehaviourAsset.Write(wr);
            wr.WriteCStr(VehicleID);
            wr.Write(PhysicsAsset);
            wr.Write(MasterSceneMayaBinaryFile);
            wr.WriteCStr(InGameName);
            wr.Write(GameplayAsset);
            wr.WriteCStr(ExhaustName); 
            wr.Write(ExhaustEntityKey);
            wr.WriteCStr(EngineName); 
            wr.Write(EngineEntityKey);
            wr.Write(DefaultWheel);
            wr.Write(BuildThisVehicle);
            wr.WriteUniquePadding(3);
        }
    }
}
