using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Physicsvehiclesuspensionattribs : IAttribute
    {
        public AttributeHeader header;
        public SizeAndPositionInformation info;

        public float UpwardMovement;
        public float TimeToDampAfterLanding;
        public float Strength;
        public float SpringLength;
        public float RearHeight;
        public float MaxYawDampingOnLanding;
        public float MaxVertVelocityDampingOnLanding;
        public float MaxRollDampingOnLanding;
        public float MaxPitchDampingOnLanding;
        public float InAirDamping;
        public float FrontHeight;
        public float DownwardMovement;
        public float Dampening;

        public Physicsvehiclesuspensionattribs(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            this.info = chunk;
            this.header = dataChunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(BitConverter.GetBytes(UpwardMovement));
            bytes.Add(BitConverter.GetBytes(TimeToDampAfterLanding));
            bytes.Add(BitConverter.GetBytes(Strength));
            bytes.Add(BitConverter.GetBytes(SpringLength));
            bytes.Add(BitConverter.GetBytes(RearHeight));
            bytes.Add(BitConverter.GetBytes(MaxYawDampingOnLanding));
            bytes.Add(BitConverter.GetBytes(MaxVertVelocityDampingOnLanding));
            bytes.Add(BitConverter.GetBytes(MaxRollDampingOnLanding));
            bytes.Add(BitConverter.GetBytes(MaxPitchDampingOnLanding));
            bytes.Add(BitConverter.GetBytes(InAirDamping));
            bytes.Add(BitConverter.GetBytes(FrontHeight));
            bytes.Add(BitConverter.GetBytes(DownwardMovement));
            bytes.Add(BitConverter.GetBytes(Dampening));
            Console.WriteLine(bytes.SelectMany(i => i).Count());
            return addPadding(bytes);
        }

        private int addPadding(List<byte[]> bytes)
        {
            if (bytes.SelectMany(i => i).Count() % 16 == 0)
            {
                return bytes.SelectMany(i => i).Count();
            }
            else
            {
                return ((bytes.SelectMany(i => i).Count() / 16) * 16) + 16;
            }
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
            UpwardMovement = br.ReadSingle();
            TimeToDampAfterLanding = br.ReadSingle();
            Strength = br.ReadSingle();
            SpringLength = br.ReadSingle();
            RearHeight = br.ReadSingle();
            MaxYawDampingOnLanding = br.ReadSingle();
            MaxVertVelocityDampingOnLanding = br.ReadSingle();
            MaxRollDampingOnLanding = br.ReadSingle();
            MaxPitchDampingOnLanding = br.ReadSingle();
            InAirDamping = br.ReadSingle();
            FrontHeight = br.ReadSingle();
            DownwardMovement = br.ReadSingle();
            Dampening = br.ReadSingle();
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(UpwardMovement);
            wr.Write(TimeToDampAfterLanding);
            wr.Write(Strength);
            wr.Write(SpringLength);
            wr.Write(RearHeight);
            wr.Write(MaxYawDampingOnLanding);
            wr.Write(MaxVertVelocityDampingOnLanding);
            wr.Write(MaxRollDampingOnLanding);
            wr.Write(MaxPitchDampingOnLanding);
            wr.Write(InAirDamping);
            wr.Write(FrontHeight);
            wr.Write(DownwardMovement);
            wr.Write(Dampening);
        }
    }
}
