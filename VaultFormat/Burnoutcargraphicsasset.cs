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
        public int RandomTrafficColours { get; set; }
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
            bytes.Add(BitConverter.GetBytes(RandomTrafficColours));
            bytes.Add(BitConverter.GetBytes(Alloc_Offences));
            bytes.Add(BitConverter.GetBytes(Num_Offences));
            bytes.Add(BitConverter.GetBytes(Size_Offences));
            bytes.Add(BitConverter.GetBytes(EncodedTypePad_Offences));
            Console.Write("Lenght:" + bytes.SelectMany(i => i).Count());
            // 28 bytes trotzdem kein Padding...
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
            RandomTrafficColours = br.ReadInt32();
            Alloc_Offences = br.ReadInt16();
            Num_Offences = br.ReadInt16();
            Size_Offences = br.ReadInt16();
            EncodedTypePad_Offences = br.ReadInt16();
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(PlayerPalletteIndex);
            wr.Write(PlayerColourIndex);
            wr.Write(Alloc);   //Allocate space for x* Size   Always 1 in VEH_P*	
            wr.Write(Num_RandomTrafficColours);   // Count
            wr.Write(Size);   // Size of each entry
            wr.Write(EncodedTypePad);  // padding
            wr.Write(RandomTrafficColours);
            wr.Write(Alloc_Offences);   // Allocate space for x* Size
            wr.Write(Num_Offences);  //  Count
            wr.Write(Size_Offences);    //  Size of ea // ch entry
            wr.Write(EncodedTypePad_Offences); //padding
        }
    }
}
