using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Burnoutcargraphicsasset : IAttribute
    {
        public int PlayerPalletteIndex { get; set; }
        public int PlayerColourIndex { get; set; }
        public short Alloc { get; set; }   //Allocate space for x* Size   Always 1 in VEH_P*	
        public short Num_RandomTrafficColours { get; set; }   // Count
        public short Size { get; set; }   // Size of each entry
        public short EncodedTypePad { get; set; }  // padding
        public List<int> RandomTrafficColours { get; set; }
        public short Alloc_Offences { get; set; }   // Allocate space for x* Size
        public short Num_Offences { get; set; }  //  Count
        public short Size_Offences { get; set; }    //  Size of ea // ch entry
        public short EncodedTypePad_Offences { get; set; } //padding

        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }

        public Burnoutcargraphicsasset(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
            RandomTrafficColours = new List<int>();
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(BitConverter.GetBytes(PlayerPalletteIndex));
            bytes.Add(BitConverter.GetBytes(PlayerColourIndex));
            bytes.Add(BitConverter.GetBytes(Alloc));
            bytes.Add(BitConverter.GetBytes(Num_RandomTrafficColours));
            bytes.Add(BitConverter.GetBytes(Size));
            bytes.Add(BitConverter.GetBytes(EncodedTypePad));
            for (int i = 0; i < Num_RandomTrafficColours; i++)
                bytes.Add(BitConverter.GetBytes(RandomTrafficColours[i]));
            bytes.Add(BitConverter.GetBytes(Alloc_Offences));
            bytes.Add(BitConverter.GetBytes(Num_Offences));
            bytes.Add(BitConverter.GetBytes(Size_Offences));
            bytes.Add(BitConverter.GetBytes(EncodedTypePad_Offences));
            Console.Write("Length:" + bytes.SelectMany(i => i).Count());
            // 28 bytes but not padding...
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
            PlayerPalletteIndex = br.ReadInt32();
            PlayerColourIndex = br.ReadInt32();
            Alloc = br.ReadInt16();
            Num_RandomTrafficColours = br.ReadInt16();
            Size = br.ReadInt16();
            EncodedTypePad = br.ReadInt16();
            for (int i = 0; i < Num_RandomTrafficColours; i++)
                RandomTrafficColours.Add(br.ReadInt32());
            Alloc_Offences = br.ReadInt16();
            Num_Offences = br.ReadInt16();
            Size_Offences = br.ReadInt16();
            EncodedTypePad_Offences = br.ReadInt16();
        }

        public void Write(BinaryWriter2 bw)
        {
            bw.Write(PlayerPalletteIndex);
            bw.Write(PlayerColourIndex);
            bw.Write(Alloc);   //Allocate space for x* Size   Always 1 in VEH_P*	
            bw.Write(Num_RandomTrafficColours);   // Count
            bw.Write(Size);   // Size of each entry
            bw.Write(EncodedTypePad);  // padding
            for (int i = 0; i < Num_RandomTrafficColours; i++)
                bw.Write(RandomTrafficColours[i]);
            bw.Write(Alloc_Offences);   // Allocate space for x* Size
            bw.Write(Num_Offences);  //  Count
            bw.Write(Size_Offences);    //  Size of ea // ch entry
            bw.Write(EncodedTypePad_Offences); //padding
        }
    }
}
