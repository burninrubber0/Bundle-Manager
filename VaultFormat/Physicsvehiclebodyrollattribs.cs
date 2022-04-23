using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Physicsvehiclebodyrollattribs : IAttribute
    {
        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }

        public float WheelLongForceHeightOffset { get; set; }
        public float WheelLatForceHeightOffset { get; set; }
        public float WeightTransferDecayZ { get; set; }
        public float WeightTransferDecayX { get; set; }
        public float RollSpringStiffness { get; set; }
        public float RollSpringDampening { get; set; }
        public float PitchSpringStiffness { get; set; }
        public float PitchSpringDampening { get; set; }
        public float FactorOfWeightZ { get; set; }
        public float FactorOfWeightX { get; set; }
        public Physicsvehiclebodyrollattribs(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(BitConverter.GetBytes(WheelLongForceHeightOffset));
            bytes.Add(BitConverter.GetBytes(WheelLatForceHeightOffset));
            bytes.Add(BitConverter.GetBytes(WeightTransferDecayZ));
            bytes.Add(BitConverter.GetBytes(WeightTransferDecayX));
            bytes.Add(BitConverter.GetBytes(RollSpringStiffness));
            bytes.Add(BitConverter.GetBytes(RollSpringDampening));
            bytes.Add(BitConverter.GetBytes(PitchSpringStiffness));
            bytes.Add(BitConverter.GetBytes(PitchSpringDampening));
            bytes.Add(BitConverter.GetBytes(FactorOfWeightZ));
            bytes.Add(BitConverter.GetBytes(FactorOfWeightX));
            //4 bytes padding is Wrong!
            bytes.Add(BitConverter.GetBytes((float)0));
            return bytes.SelectMany(i => i).Count();
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
            WheelLongForceHeightOffset = br.ReadSingle();
            WheelLatForceHeightOffset = br.ReadSingle();
            WeightTransferDecayZ = br.ReadSingle();
            WeightTransferDecayX = br.ReadSingle();
            RollSpringStiffness = br.ReadSingle();
            RollSpringDampening = br.ReadSingle();
            PitchSpringStiffness = br.ReadSingle();
            PitchSpringDampening = br.ReadSingle();
            FactorOfWeightZ = br.ReadSingle();
            FactorOfWeightX = br.ReadSingle();
            br.SkipUniquePadding(4);
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(WheelLongForceHeightOffset);
            wr.Write(WheelLatForceHeightOffset);
            wr.Write(WeightTransferDecayZ);
            wr.Write(WeightTransferDecayX);
            wr.Write(RollSpringStiffness);
            wr.Write(RollSpringDampening);
            wr.Write(PitchSpringStiffness);
            wr.Write(PitchSpringDampening);
            wr.Write(FactorOfWeightZ);
            wr.Write(FactorOfWeightX);
            wr.WriteUniquePadding(4);
        }
    }
}
