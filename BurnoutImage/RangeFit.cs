using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCnEncoder.Shared;

namespace BurnoutImage
{
    internal class RangeFit : ColourFit
    {
        public Vec3 Metric;
        public Vec3 Start;
        public Vec3 End;
        public float BestError;

        public RangeFit(ColourSet colours, CompressionFormat compression) : base(colours, compression)
        {
            bool colourMetricPerceptual = false;

            // initialize the metric
            bool perceptual = colourMetricPerceptual;
            if (perceptual)
            {
                Metric = new Vec3(0.2126f, 0.7152f, 0.0722f);
            } else
            {
                Metric = new Vec3(0);
            }

            // initialize the best error
            BestError = float.MaxValue;

            // cache some values
            int count = Colours.Count;
            Vec3[] values = Colours.Points;
            float[] weights = Colours.Weights;

            // get the covariance matrix
            /*Sym3x3 covariance = ComputeWeightedCovariance(count, values, weights);

            // compute the principle components
            Vec3 principle = ComputePrincipleComponent(covariance);

            // get the min and max range as the codebook endpoints
            Vec3 start = new Vec3(0.0f);
            Vec3 end = new Vec3(0.0f);
            if (count > 0)
            {
                float min, max;

                // compute the range
                start = end = values[0];
                
            }*/
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
