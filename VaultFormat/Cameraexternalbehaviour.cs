using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Cameraexternalbehaviour : IAttribute
    {
        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }
        public float ZDistanceScale { get; set; }
        public float ZAndTiltCutoffSpeedMPH { get; set; }
        public float YawSpring { get; set; }
        public float TiltCameraScale { get; set; }
        public float TiltAroundCar { get; set; }
        public float SlideZOffsetMax { get; set; }
        public float SlideYScale { get; set; }
        public float SlideXScale { get; set; }
        public float PivotZOffset { get; set; }
        public float PivotLength { get; set; }
        public float PivotHeight { get; set; }
        public float PitchSpring { get; set; }
        public float FieldOfView { get; set; }
        public float DriftYawSpring { get; set; }
        public float DownAngle { get; set; }
        public float BoostFieldOfViewZoom { get; set; }
        public float BoostFieldOfView { get; set; }
        public Cameraexternalbehaviour(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(BitConverter.GetBytes(ZDistanceScale));
            bytes.Add(BitConverter.GetBytes(ZAndTiltCutoffSpeedMPH));
            bytes.Add(BitConverter.GetBytes(YawSpring));
            bytes.Add(BitConverter.GetBytes(TiltCameraScale));
            bytes.Add(BitConverter.GetBytes(TiltAroundCar));
            bytes.Add(BitConverter.GetBytes(SlideZOffsetMax));
            bytes.Add(BitConverter.GetBytes(SlideYScale));
            bytes.Add(BitConverter.GetBytes(SlideXScale));
            bytes.Add(BitConverter.GetBytes(PivotZOffset));
            bytes.Add(BitConverter.GetBytes(PivotLength));
            bytes.Add(BitConverter.GetBytes(PivotHeight));
            bytes.Add(BitConverter.GetBytes(PitchSpring));
            bytes.Add(BitConverter.GetBytes(FieldOfView));
            bytes.Add(BitConverter.GetBytes(DriftYawSpring));
            bytes.Add(BitConverter.GetBytes(DownAngle));
            bytes.Add(BitConverter.GetBytes(BoostFieldOfViewZoom));
            bytes.Add(BitConverter.GetBytes(BoostFieldOfView));
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
            ZDistanceScale = br.ReadSingle();
            ZAndTiltCutoffSpeedMPH = br.ReadSingle();
            YawSpring = br.ReadSingle();
            TiltCameraScale = br.ReadSingle();
            TiltAroundCar = br.ReadSingle();
            SlideZOffsetMax = br.ReadSingle();
            SlideYScale = br.ReadSingle();
            SlideXScale = br.ReadSingle();
            PivotZOffset = br.ReadSingle();
            PivotLength = br.ReadSingle();
            PivotHeight = br.ReadSingle();
            PitchSpring = br.ReadSingle();
            FieldOfView = br.ReadSingle();
            DriftYawSpring = br.ReadSingle();
            DownAngle = br.ReadSingle();
            BoostFieldOfViewZoom = br.ReadSingle();
            BoostFieldOfView = br.ReadSingle();
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(ZDistanceScale);
            wr.Write(ZAndTiltCutoffSpeedMPH);
            wr.Write(YawSpring);
            wr.Write(TiltCameraScale);
            wr.Write(TiltAroundCar);
            wr.Write(SlideZOffsetMax);
            wr.Write(SlideYScale);
            wr.Write(SlideXScale);
            wr.Write(PivotZOffset);
            wr.Write(PivotLength);
            wr.Write(PivotHeight);
            wr.Write(PitchSpring);
            wr.Write(FieldOfView);
            wr.Write(DriftYawSpring);
            wr.Write(DownAngle);
            wr.Write(BoostFieldOfViewZoom);
            wr.Write(BoostFieldOfView);
        }
    }
}
