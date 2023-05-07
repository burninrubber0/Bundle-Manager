using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Physicsvehiclesteeringattribs : IAttribute
    {
        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }
        public float TimeForLock { get; set; }
        public float StraightReactionBias { get; set; }
        public float SpeedForMinAngle { get; set; }
        public float SpeedForMaxAngle { get; set; }
        public float MinAngle { get; set; }
        public float MaxAngle { get; set; }
        public float AiPidCoefficientP { get; set; }
        public float AiPidCoefficientI { get; set; }
        public float AiPidCoefficientDriftP { get; set; }
        public float AiPidCoefficientDriftI { get; set; }
        public float AiPidCoefficientDriftD { get; set; }
        public float AiPidCoefficientD { get; set; }
        public float AiMinLookAheadDistanceForDrift { get; set; }
        public float AiLookAheadTimeForDrift { get; set; }
        public Physicsvehiclesteeringattribs(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }


        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>
            {
                BitConverter.GetBytes(TimeForLock),
                BitConverter.GetBytes(StraightReactionBias),
                BitConverter.GetBytes(SpeedForMinAngle),
                BitConverter.GetBytes(SpeedForMaxAngle),
                BitConverter.GetBytes(MinAngle),
                BitConverter.GetBytes(MaxAngle),
                BitConverter.GetBytes(AiPidCoefficientP),
                BitConverter.GetBytes(AiPidCoefficientI),
                BitConverter.GetBytes(AiPidCoefficientDriftP),
                BitConverter.GetBytes(AiPidCoefficientDriftI),
                BitConverter.GetBytes(AiPidCoefficientDriftD),
                BitConverter.GetBytes(AiPidCoefficientD),
                BitConverter.GetBytes(AiMinLookAheadDistanceForDrift),
                BitConverter.GetBytes(AiLookAheadTimeForDrift)
            };
            // Hack, because of weird padding of suspension attribs
            bytes.AddRange(Physicsvehiclesuspensionattribs.getDefaultBytes());
            Console.WriteLine(bytes.SelectMany(i => i).Count());
            return CountingUtilities.AddPadding(bytes);
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
            TimeForLock = br.ReadSingle();
            StraightReactionBias = br.ReadSingle();
            SpeedForMinAngle = br.ReadSingle();
            SpeedForMaxAngle = br.ReadSingle();
            MinAngle = br.ReadSingle();
            MaxAngle = br.ReadSingle();
            AiPidCoefficientP = br.ReadSingle();
            AiPidCoefficientI = br.ReadSingle();
            AiPidCoefficientDriftP = br.ReadSingle();
            AiPidCoefficientDriftI = br.ReadSingle();
            AiPidCoefficientDriftD = br.ReadSingle();
            AiPidCoefficientD = br.ReadSingle();
            AiMinLookAheadDistanceForDrift = br.ReadSingle();
            AiLookAheadTimeForDrift = br.ReadSingle();
            br.SkipPadding();
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(TimeForLock);
            wr.Write(StraightReactionBias);
            wr.Write(SpeedForMinAngle);
            wr.Write(SpeedForMaxAngle);
            wr.Write(MinAngle);
            wr.Write(MaxAngle);
            wr.Write(AiPidCoefficientP);
            wr.Write(AiPidCoefficientI);
            wr.Write(AiPidCoefficientDriftP);
            wr.Write(AiPidCoefficientDriftI);
            wr.Write(AiPidCoefficientDriftD);
            wr.Write(AiPidCoefficientD);
            wr.Write(AiMinLookAheadDistanceForDrift);
            wr.Write(AiLookAheadTimeForDrift);
            wr.WritePadding();
        }
    }
}
