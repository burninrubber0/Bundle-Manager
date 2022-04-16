using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Physicsvehicleengineattribs : IAttribute
    {
        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }

        public Vector3I TorqueScales2 { get; set; }
        public Vector3I TorqueScales1 { get; set; }
        public Vector3I GearUpRPMs2 { get; set; }
        public Vector3I GearUpRPMs1 { get; set; }
        public Vector3I GearRatios2 { get; set; }
        public Vector3I GearRatios1 { get; set; }
        public float TransmissionEfficiency { get; set; }
        public float TorqueFallOffRPM { get; set; }
        public float MaxTorque { get; set; }
        public float MaxRPM { get; set; }
        public float LSDMGearUpSpeed { get; set; }
        public float GearChangeTime { get; set; }
        public float FlyWheelInertia { get; set; }
        public float FlyWheelFriction { get; set; }
        public float EngineResistance { get; set; }
        public float EngineLowEndTorqueFactor { get; set; }
        public float EngineBraking { get; set; }
        public float Differential { get; set; }

        public Physicsvehicleengineattribs(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(TorqueScales2.toBytes());
            bytes.Add(TorqueScales1.toBytes());
            bytes.Add(GearUpRPMs2.toBytes());
            bytes.Add(GearUpRPMs1.toBytes());
            bytes.Add(GearRatios2.toBytes());
            bytes.Add(GearRatios1.toBytes());
            bytes.Add(BitConverter.GetBytes(TransmissionEfficiency));
            bytes.Add(BitConverter.GetBytes(TorqueFallOffRPM));
            bytes.Add(BitConverter.GetBytes(MaxTorque));
            bytes.Add(BitConverter.GetBytes(MaxRPM));
            bytes.Add(BitConverter.GetBytes(LSDMGearUpSpeed));
            bytes.Add(BitConverter.GetBytes(GearChangeTime));
            bytes.Add(BitConverter.GetBytes(FlyWheelInertia));
            bytes.Add(BitConverter.GetBytes(FlyWheelFriction));
            bytes.Add(BitConverter.GetBytes(EngineResistance));
            bytes.Add(BitConverter.GetBytes(EngineLowEndTorqueFactor));
            bytes.Add(BitConverter.GetBytes(EngineBraking));
            bytes.Add(BitConverter.GetBytes(Differential));
            return CountingUtilities.AddPadding(bytes);
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(TorqueScales2.toBytes());
            wr.Write(TorqueScales1.toBytes());
            wr.Write(GearUpRPMs2.toBytes());
            wr.Write(GearUpRPMs1.toBytes());
            wr.Write(GearRatios2.toBytes());
            wr.Write(GearRatios1.toBytes());
            wr.Write(TransmissionEfficiency);
            wr.Write(TorqueFallOffRPM);
            wr.Write(MaxTorque);
            wr.Write(MaxRPM);
            wr.Write(LSDMGearUpSpeed);
            wr.Write(GearChangeTime);
            wr.Write(FlyWheelInertia);
            wr.Write(FlyWheelFriction);
            wr.Write(EngineResistance);
            wr.Write(EngineLowEndTorqueFactor);
            wr.Write(EngineBraking);
            wr.Write(Differential);
        }

        public void Read(ILoader loader, BinaryReader2 br)
        {
            TorqueScales2 = br.ReadVector3I();
            TorqueScales1 = br.ReadVector3I();
            GearUpRPMs2 = br.ReadVector3I();
            GearUpRPMs1 = br.ReadVector3I();
            GearRatios2 = br.ReadVector3I();
            GearRatios1 = br.ReadVector3I();
            TransmissionEfficiency = br.ReadSingle();
            TorqueFallOffRPM = br.ReadSingle();
            MaxTorque = br.ReadSingle();
            MaxRPM = br.ReadSingle();
            LSDMGearUpSpeed = br.ReadSingle();
            GearChangeTime = br.ReadSingle();
            FlyWheelInertia = br.ReadSingle();
            FlyWheelFriction = br.ReadSingle();
            EngineResistance = br.ReadSingle();
            EngineLowEndTorqueFactor = br.ReadSingle();
            EngineBraking = br.ReadSingle();
            Differential = br.ReadSingle();
        }

        public AttributeHeader getHeader()
        {
            return header;
        }

        public SizeAndPositionInformation getInfo()
        {
            return info;
        }

    }
}
