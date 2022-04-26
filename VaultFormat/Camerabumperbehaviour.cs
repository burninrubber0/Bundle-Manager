using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Camerabumperbehaviour : IAttribute
    {
        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }

        public float ZOffset { get; set; }
        public float YOffset { get; set; }
        public float YawSpring { get; set; }
        public float RollSpring { get; set; }
        public float PitchSpring { get; set; }
        public float FieldOfView { get; set; }
        public float BoostFieldOfView { get; set; }
        public float BodyRollScale { get; set; }
        public float BodyPitchScale { get; set; }
        public float AccelerationResponse { get; set; }
        public float AccelerationDampening { get; set; }
        public Camerabumperbehaviour(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(BitConverter.GetBytes(ZOffset));
            bytes.Add(BitConverter.GetBytes(YOffset));
            bytes.Add(BitConverter.GetBytes(YawSpring));
            bytes.Add(BitConverter.GetBytes(RollSpring));
            bytes.Add(BitConverter.GetBytes(PitchSpring));
            bytes.Add(BitConverter.GetBytes(FieldOfView));
            bytes.Add(BitConverter.GetBytes(BoostFieldOfView));
            bytes.Add(BitConverter.GetBytes(BodyRollScale));
            bytes.Add(BitConverter.GetBytes(BodyPitchScale));
            bytes.Add(BitConverter.GetBytes(AccelerationResponse));
            bytes.Add(BitConverter.GetBytes(AccelerationDampening));
            //To-Do: again here we have not padding, but usually we should have it 
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
            ZOffset = br.ReadSingle();
            YOffset = br.ReadSingle();
            YawSpring = br.ReadSingle();
            RollSpring = br.ReadSingle();
            PitchSpring = br.ReadSingle();
            FieldOfView = br.ReadSingle();
            BoostFieldOfView = br.ReadSingle();
            BodyRollScale = br.ReadSingle();
            BodyPitchScale = br.ReadSingle();
            AccelerationResponse = br.ReadSingle();
            AccelerationDampening = br.ReadSingle();
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(ZOffset);
            wr.Write(YOffset);
            wr.Write(YawSpring);
            wr.Write(RollSpring);
            wr.Write(PitchSpring);
            wr.Write(FieldOfView);
            wr.Write(BoostFieldOfView);
            wr.Write(BodyRollScale);
            wr.Write(BodyPitchScale);
            wr.Write(AccelerationResponse);
            wr.Write(AccelerationDampening);
        }
    }
}
