using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using BundleFormat;
using BundleUtilities;

namespace VaultFormat
{
    public class Physicsvehiclebaseattribs : IAttribute
    {
        public Vector4 RearRightWheelPosition { get; set; }
        public Vector4 FrontRightWheelPosition { get; set; }
        public Vector4 CoMOffset { get; set; }
        public Vector4 BrakeScaleToFactor { get; set; }
        public float YawDampingOnTakeOff { get; set; }
        public float TractionLineLength { get; set; }
        public float TimeForFullBrake { get; set; }
        public float SurfaceRoughnessFactor { get; set; }
        public float SurfaceRearGripFactor { get; set; }
        public float SurfaceFrontGripFactor { get; set; }
        public float SurfaceDragFactor { get; set; }
        public float RollLimitOnTakeOff { get; set; }
        public float RollDampingOnTakeOff { get; set; }
        public float RearWheelMass { get; set; }
        public float RearTireStaticFrictionCoefficient { get; set; }
        public float RearTireLongForceBias { get; set; }
        public float RearTireDynamicFrictionCoefficient { get; set; }
        public float RearTireAdhesiveLimit { get; set; }
        public float RearLongGripCurvePeakSlipRatio { get; set; }
        public float RearLongGripCurvePeakCoefficient { get; set; }
        public float RearLongGripCurveFloorSlipRatio { get; set; }
        public float RearLongGripCurveFallCoefficient { get; set; }
        public float RearLatGripCurvePeakSlipRatio { get; set; }
        public float RearLatGripCurvePeakCoefficient { get; set; }
        public float RearLatGripCurveFloorSlipRatio { get; set; }
        public float RearLatGripCurveFallCoefficient { get; set; }
        public float RearLatGripCurveDriftPeakSlipRatio { get; set; }
        public float PowerToRear { get; set; }
        public float PowerToFront { get; set; }
        public float PitchDampingOnTakeOff { get; set; }
        public float MaxSpeed { get; set; }
        public float MagicBrakeFactorTurning { get; set; }

        public float MagicBrakeFactorStraightLine { get; set; }
        public float LowSpeedTyreFrictionTractionControl { get; set; }
        public float LowSpeedThrottleTractionControl { get; set; }
        public float LowSpeedDrivingSpeed { get; set; }
        public float LockBrakeScale { get; set; }
        public float LinearDrag { get; set; }
        public float HighSpeedAngularDamping { get; set; }
        public float FrontWheelMass { get; set; }
        public float FrontTireStaticFrictionCoefficient { get; set; }
        public float FrontTireLongForceBias { get; set; }
        public float FrontTireDynamicFrictionCoefficient { get; set; }
        public float FrontTireAdhesiveLimit { get; set; }
        public float FrontLongGripCurvePeakSlipRatio { get; set; }
        public float FrontLongGripCurvePeakCoefficient { get; set; }
        public float FrontLongGripCurveFloorSlipRatio { get; set; }
        public float FrontLongGripCurveFallCoefficient { get; set; }
        public float FrontLatGripCurvePeakSlipRatio { get; set; }
        public float FrontLatGripCurvePeakCoefficient { get; set; }
        public float FrontLatGripCurveFloorSlipRatio { get; set; }
        public float FrontLatGripCurveFallCoefficient { get; set; }
        public float FrontLatGripCurveDriftPeakSlipRatio { get; set; }
        public float DrivingMass { get; set; }
        public float DriveTimeDeformLimitX { get; set; }
        public float DriveTimeDeformLimitPosZ { get; set; }
        public float DriveTimeDeformLimitNegZ { get; set; }
        public float DriveTimeDeformLimitNegY { get; set; }
        public float DownForceZOffset { get; set; }
        public float DownForce { get; set; }
        public float CrashExtraYawVelocityFactor { get; set; }
        public float CrashExtraRollVelocityFactor { get; set; }
        public float CrashExtraPitchVelocityFactor { get; set; }
        public float CrashExtraLinearVelocityFactor { get; set; }
        public float AngularDrag { get; set; }
        public AttributeHeader header { get; set; }
        public SizeAndPositionInformation info { get; set; }
        public Physicsvehiclebaseattribs(SizeAndPositionInformation chunk, AttributeHeader dataChunk)
        {
            header = dataChunk;
            info = chunk;
        }

        public int getDataSize()
        {
            List<byte[]> bytes = new List<byte[]>
            {
                RearRightWheelPosition.toBytes(),
                FrontRightWheelPosition.toBytes(),
                CoMOffset.toBytes(),
                BrakeScaleToFactor.toBytes(),
                BitConverter.GetBytes(YawDampingOnTakeOff),
                BitConverter.GetBytes(TractionLineLength),
                BitConverter.GetBytes(TimeForFullBrake),
                BitConverter.GetBytes(SurfaceRoughnessFactor),
                BitConverter.GetBytes(SurfaceRearGripFactor),
                BitConverter.GetBytes(SurfaceFrontGripFactor),
                BitConverter.GetBytes(SurfaceDragFactor),
                BitConverter.GetBytes(RollLimitOnTakeOff),
                BitConverter.GetBytes(RollDampingOnTakeOff),
                BitConverter.GetBytes(RearWheelMass),
                BitConverter.GetBytes(RearTireStaticFrictionCoefficient),
                BitConverter.GetBytes(RearTireLongForceBias),
                BitConverter.GetBytes(RearTireDynamicFrictionCoefficient),
                BitConverter.GetBytes(RearTireAdhesiveLimit),
                BitConverter.GetBytes(RearLongGripCurvePeakSlipRatio),
                BitConverter.GetBytes(RearLongGripCurvePeakCoefficient),
                BitConverter.GetBytes(RearLongGripCurveFloorSlipRatio),
                BitConverter.GetBytes(RearLongGripCurveFallCoefficient),
                BitConverter.GetBytes(RearLatGripCurvePeakSlipRatio),
                BitConverter.GetBytes(RearLatGripCurvePeakCoefficient),
                BitConverter.GetBytes(RearLatGripCurveFloorSlipRatio),
                BitConverter.GetBytes(RearLatGripCurveFallCoefficient),
                BitConverter.GetBytes(RearLatGripCurveDriftPeakSlipRatio),
                BitConverter.GetBytes(PowerToRear),
                BitConverter.GetBytes(PowerToFront),
                BitConverter.GetBytes(PitchDampingOnTakeOff),
                BitConverter.GetBytes(MaxSpeed),
                BitConverter.GetBytes(MagicBrakeFactorTurning),
                BitConverter.GetBytes(MagicBrakeFactorStraightLine),
                BitConverter.GetBytes(LowSpeedTyreFrictionTractionControl),
                BitConverter.GetBytes(LowSpeedThrottleTractionControl),
                BitConverter.GetBytes(LowSpeedDrivingSpeed),
                BitConverter.GetBytes(LockBrakeScale),
                BitConverter.GetBytes(LinearDrag),
                BitConverter.GetBytes(HighSpeedAngularDamping),
                BitConverter.GetBytes(FrontWheelMass),
                BitConverter.GetBytes(FrontTireStaticFrictionCoefficient),
                BitConverter.GetBytes(FrontTireLongForceBias),
                BitConverter.GetBytes(FrontTireDynamicFrictionCoefficient),
                BitConverter.GetBytes(FrontTireAdhesiveLimit),
                BitConverter.GetBytes(FrontLongGripCurvePeakSlipRatio),
                BitConverter.GetBytes(FrontLongGripCurvePeakCoefficient),
                BitConverter.GetBytes(FrontLongGripCurveFloorSlipRatio),
                BitConverter.GetBytes(FrontLongGripCurveFallCoefficient),
                BitConverter.GetBytes(FrontLatGripCurvePeakSlipRatio),
                BitConverter.GetBytes(FrontLatGripCurvePeakCoefficient),
                BitConverter.GetBytes(FrontLatGripCurveFloorSlipRatio),
                BitConverter.GetBytes(FrontLatGripCurveFallCoefficient),
                BitConverter.GetBytes(FrontLatGripCurveDriftPeakSlipRatio),
                BitConverter.GetBytes(DrivingMass),
                BitConverter.GetBytes(DriveTimeDeformLimitX),
                BitConverter.GetBytes(DriveTimeDeformLimitPosZ),
                BitConverter.GetBytes(DriveTimeDeformLimitNegZ),
                BitConverter.GetBytes(DriveTimeDeformLimitNegY),
                BitConverter.GetBytes(DownForceZOffset),
                BitConverter.GetBytes(DownForce),
                BitConverter.GetBytes(CrashExtraYawVelocityFactor),
                BitConverter.GetBytes(CrashExtraRollVelocityFactor),
                BitConverter.GetBytes(CrashExtraPitchVelocityFactor),
                BitConverter.GetBytes(CrashExtraLinearVelocityFactor),
                BitConverter.GetBytes(AngularDrag)
            };
            // No padding needed
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
            RearRightWheelPosition = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            FrontRightWheelPosition = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            CoMOffset = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            BrakeScaleToFactor = new Vector4(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            YawDampingOnTakeOff = br.ReadSingle();
            TractionLineLength = br.ReadSingle();
            TimeForFullBrake = br.ReadSingle();
            SurfaceRoughnessFactor = br.ReadSingle();
            SurfaceRearGripFactor = br.ReadSingle();
            SurfaceFrontGripFactor = br.ReadSingle();
            SurfaceDragFactor = br.ReadSingle();
            RollLimitOnTakeOff = br.ReadSingle();
            RollDampingOnTakeOff = br.ReadSingle();
            RearWheelMass = br.ReadSingle();
            RearTireStaticFrictionCoefficient = br.ReadSingle();
            RearTireLongForceBias = br.ReadSingle();
            RearTireDynamicFrictionCoefficient = br.ReadSingle();
            RearTireAdhesiveLimit = br.ReadSingle();
            RearLongGripCurvePeakSlipRatio = br.ReadSingle();
            RearLongGripCurvePeakCoefficient = br.ReadSingle();
            RearLongGripCurveFloorSlipRatio = br.ReadSingle();
            RearLongGripCurveFallCoefficient = br.ReadSingle();
            RearLatGripCurvePeakSlipRatio = br.ReadSingle();
            RearLatGripCurvePeakCoefficient = br.ReadSingle();
            RearLatGripCurveFloorSlipRatio = br.ReadSingle();
            RearLatGripCurveFallCoefficient = br.ReadSingle();
            RearLatGripCurveDriftPeakSlipRatio = br.ReadSingle();
            PowerToRear = br.ReadSingle();
            PowerToFront = br.ReadSingle();
            PitchDampingOnTakeOff = br.ReadSingle();
            MaxSpeed = br.ReadSingle();
            MagicBrakeFactorTurning = br.ReadSingle();
            MagicBrakeFactorStraightLine = br.ReadSingle();
            LowSpeedTyreFrictionTractionControl = br.ReadSingle();
            LowSpeedThrottleTractionControl = br.ReadSingle();
            LowSpeedDrivingSpeed = br.ReadSingle();
            LockBrakeScale = br.ReadSingle();
            LinearDrag = br.ReadSingle();
            HighSpeedAngularDamping = br.ReadSingle();
            FrontWheelMass = br.ReadSingle();
            FrontTireStaticFrictionCoefficient = br.ReadSingle();
            FrontTireLongForceBias = br.ReadSingle();
            FrontTireDynamicFrictionCoefficient = br.ReadSingle();
            FrontTireAdhesiveLimit = br.ReadSingle();
            FrontLongGripCurvePeakSlipRatio = br.ReadSingle();
            FrontLongGripCurvePeakCoefficient = br.ReadSingle();
            FrontLongGripCurveFloorSlipRatio = br.ReadSingle();
            FrontLongGripCurveFallCoefficient = br.ReadSingle();
            FrontLatGripCurvePeakSlipRatio = br.ReadSingle();
            FrontLatGripCurvePeakCoefficient = br.ReadSingle();
            FrontLatGripCurveFloorSlipRatio = br.ReadSingle();
            FrontLatGripCurveFallCoefficient = br.ReadSingle();
            FrontLatGripCurveDriftPeakSlipRatio = br.ReadSingle();
            DrivingMass = br.ReadSingle();
            DriveTimeDeformLimitX = br.ReadSingle();
            DriveTimeDeformLimitPosZ = br.ReadSingle();
            DriveTimeDeformLimitNegZ = br.ReadSingle();
            DriveTimeDeformLimitNegY = br.ReadSingle();
            DownForceZOffset = br.ReadSingle();
            DownForce = br.ReadSingle();
            CrashExtraYawVelocityFactor = br.ReadSingle();
            CrashExtraRollVelocityFactor = br.ReadSingle();
            CrashExtraPitchVelocityFactor = br.ReadSingle();
            CrashExtraLinearVelocityFactor = br.ReadSingle();
            AngularDrag = br.ReadSingle();
        }

        public void Write(BinaryWriter wr)
        {
            wr.Write(RearRightWheelPosition.toBytes());
            wr.Write(FrontRightWheelPosition.toBytes());
            wr.Write(CoMOffset.toBytes());
            wr.Write(BrakeScaleToFactor.toBytes());
            wr.Write(YawDampingOnTakeOff);
            wr.Write(TractionLineLength);
            wr.Write(TimeForFullBrake);
            wr.Write(SurfaceRoughnessFactor);
            wr.Write(SurfaceRearGripFactor);
            wr.Write(SurfaceFrontGripFactor);
            wr.Write(SurfaceDragFactor);
            wr.Write(RollLimitOnTakeOff);
            wr.Write(RollDampingOnTakeOff);
            wr.Write(RearWheelMass);
            wr.Write(RearTireStaticFrictionCoefficient);
            wr.Write(RearTireLongForceBias);
            wr.Write(RearTireDynamicFrictionCoefficient);
            wr.Write(RearTireAdhesiveLimit);
            wr.Write(RearLongGripCurvePeakSlipRatio);
            wr.Write(RearLongGripCurvePeakCoefficient);
            wr.Write(RearLongGripCurveFloorSlipRatio);
            wr.Write(RearLongGripCurveFallCoefficient);
            wr.Write(RearLatGripCurvePeakSlipRatio);
            wr.Write(RearLatGripCurvePeakCoefficient);
            wr.Write(RearLatGripCurveFloorSlipRatio);
            wr.Write(RearLatGripCurveFallCoefficient);
            wr.Write(RearLatGripCurveDriftPeakSlipRatio);
            wr.Write(PowerToRear);
            wr.Write(PowerToFront);
            wr.Write(PitchDampingOnTakeOff);
            wr.Write(MaxSpeed);
            wr.Write(MagicBrakeFactorTurning);
            wr.Write(MagicBrakeFactorStraightLine);
            wr.Write(LowSpeedTyreFrictionTractionControl);
            wr.Write(LowSpeedThrottleTractionControl);
            wr.Write(LowSpeedDrivingSpeed);
            wr.Write(LockBrakeScale);
            wr.Write(LinearDrag);
            wr.Write(HighSpeedAngularDamping);
            wr.Write(FrontWheelMass);
            wr.Write(FrontTireStaticFrictionCoefficient);
            wr.Write(FrontTireLongForceBias);
            wr.Write(FrontTireDynamicFrictionCoefficient);
            wr.Write(FrontTireAdhesiveLimit);
            wr.Write(FrontLongGripCurvePeakSlipRatio);
            wr.Write(FrontLongGripCurvePeakCoefficient);
            wr.Write(FrontLongGripCurveFloorSlipRatio);
            wr.Write(FrontLongGripCurveFallCoefficient);
            wr.Write(FrontLatGripCurvePeakSlipRatio);
            wr.Write(FrontLatGripCurvePeakCoefficient);
            wr.Write(FrontLatGripCurveFloorSlipRatio);
            wr.Write(FrontLatGripCurveFallCoefficient);
            wr.Write(FrontLatGripCurveDriftPeakSlipRatio);
            wr.Write(DrivingMass);
            wr.Write(DriveTimeDeformLimitX);
            wr.Write(DriveTimeDeformLimitPosZ);
            wr.Write(DriveTimeDeformLimitNegZ);
            wr.Write(DriveTimeDeformLimitNegY);
            wr.Write(DownForceZOffset);
            wr.Write(DownForce);
            wr.Write(CrashExtraYawVelocityFactor);
            wr.Write(CrashExtraRollVelocityFactor);
            wr.Write(CrashExtraPitchVelocityFactor);
            wr.Write(CrashExtraLinearVelocityFactor);
            wr.Write(AngularDrag);
        }
    }
}
