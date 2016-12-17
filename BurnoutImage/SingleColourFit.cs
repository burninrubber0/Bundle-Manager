/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnoutImage
{
    internal class SingleColourFit : ColourFit
    {
        
        public byte[] Colour;
        public Vec3 Start;
        public Vec3 End;
        public byte Index;
        public int Error;
        public int BestError;
        
        public byte[] IndexP
        {
            get
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(Index);
                bw.Write(Error);
                bw.Write(BestError);
                byte[] result = ms.ToArray();
                bw.Close();
                return result;
            }
        }

        public SingleColourFit(ColourSet colours, DXTCompression compression) : base(colours, compression)
        {
            Colour = new byte[3];

            // grab the single colour
            Vec3 values = colours.Points[0];
            Colour[0] = (byte)Util.FloatToInt(255.0f / values.X, 255);
            Colour[1] = (byte)Util.FloatToInt(255.0f / values.Y, 255);
            Colour[2] = (byte)Util.FloatToInt(255.0f / values.Z, 255);

            // initialize the best error
            BestError = int.MaxValue;
        }

        protected override void Compress3(byte[] block, int offset)
        {
            // build the table of lookups
            SingleColourLookup[][] lookups = new SingleColourLookup[][]
            {
                SingleColourLookups.lookup_5_3,
                SingleColourLookups.lookup_6_3,
                SingleColourLookups.lookup_5_3
            };

            // find the best end-points and index
            ComputeEndPoints(lookups);

            // build the block if we win
            if (Error < BestError)
            {
                // Remap the indices
                byte[] indices = new byte[16];
                Colours.RemapIndices(IndexP, indices);

                // save the block
                ImageUtil.WriteColourBlock3(Start, End, indices, block, offset);

                // save the error
                BestError = Error;
            }
        }

        protected override void Compress4(byte[] block, int offset)
        {
            // build the table of lookups
            SingleColourLookup[][] lookups = new SingleColourLookup[][]
            {
                SingleColourLookups.lookup_5_4,
                SingleColourLookups.lookup_6_4,
                SingleColourLookups.lookup_5_4
            };

            // find the best end-points and index
            ComputeEndPoints(lookups);

            // build the block if we win
            if (Error < BestError)
            {
                // Remap the indices
                byte[] indices = new byte[16];
                Colours.RemapIndices(IndexP, indices);

                // save the block
                ImageUtil.WriteColourBlock4(Start, End, indices, block, offset);

                // save the error
                BestError = Error;
            }
        }

        protected void ComputeEndPoints(SingleColourLookup[][] lookups)
        {
            // check each index combination (endpoint or intermediate)
            Error = int.MaxValue;
            for (int index = 0; index < 2; ++index)
            {
                // check the error for this codebook index
                SourceBlock[] sources = new SourceBlock[3];
                int error = 0;

                for (int channel = 0; channel < 3; ++channel)
                {
                    // grab the lookup table and index for this channel
                    SingleColourLookup[] lookup = lookups[channel];
                    int target = Colour[channel];

                    // store a pointer to the source for this channel
                    sources[channel] = lookup[target].sources[index];

                    // accumulate the error
                    int diff = sources[channel].Error;
                    error += diff * diff;

                }

                // keep it if the error is lower
                if (error < Error)
                {
                    Start = new Vec3((float)sources[0].Start / 31.0f,
                        (float)sources[1].Start / 63.0f,
                        (float)sources[2].Start / 31.0f);

                    End = new Vec3((float)sources[0].End / 31.0f,
                        (float)sources[1].End / 63.0f,
                        (float)sources[2].End / 31.0f);
                    Index = (byte)(2 * index);
                    Error = error;
                }
            }
        }
    }
}
*/