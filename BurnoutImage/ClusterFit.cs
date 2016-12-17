using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BurnoutImage
{
    internal class ClusterFit : ColourFit
    {
        public const int MaxIterations = 8;

        public int IterationCount;
        public Vec3 Principle;
        public byte[] Order;
        public Vec4[] PointWeights;
        public Vec4 XSumWSum;
        public Vec4 Metric;
        public Vec4 BestError;

        public ClusterFit(ColourSet colours, DXTCompression compression) : base(colours, compression)
        {
            Order = new byte[16 * MaxIterations];
            PointWeights = new Vec4[16];
        }

        protected override void Compress3(byte[] block, int offset)
        {
            throw new NotImplementedException();
        }

        protected override void Compress4(byte[] block, int offset)
        {
            throw new NotImplementedException();
        }
    }
}
