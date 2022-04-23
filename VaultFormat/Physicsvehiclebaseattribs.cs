using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BundleUtilities;

namespace VaultFormat
{
    public class Physicsvehiclebaseattribs : IAttribute
    {
        public Vector3I RearRightWheelPosition { get; set; }
        public Vector3I FrontRightWheelPosition { get; set; }
        public Vector3I CoMOffset { get; set; }
        public Vector3I BrakeScaleToFactor { get; set; }
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
            List<byte[]> bytes = new List<byte[]>();
            bytes.Add(RearRightWheelPosition.toBytes());
            bytes.Add(FrontRightWheelPosition.toBytes());
            bytes.Add(CoMOffset.toBytes());
            bytes.Add(BrakeScaleToFactor.toBytes());
            bytes.Add(BitConverter.GetBytes(YawDampingOnTakeOff));
            bytes.Add(BitConverter.GetBytes(TractionLineLength));
            bytes.Add(BitConverter.GetBytes(TimeForFullBrake));
            bytes.Add(BitConverter.GetBytes(SurfaceRoughnessFactor));
            bytes.Add(BitConverter.GetBytes(SurfaceRearGripFactor));
            bytes.Add(BitConverter.GetBytes(SurfaceFrontGripFactor));
            bytes.Add(BitConverter.GetBytes(SurfaceDragFactor));
            bytes.Add(BitConverter.GetBytes(RollLimitOnTakeOff));
            bytes.Add(BitConverter.GetBytes(RollDampingOnTakeOff));
            bytes.Add(BitConverter.GetBytes(RearWheelMass));
            bytes.Add(BitConverter.GetBytes(RearTireStaticFrictionCoefficient));
            bytes.Add(BitConverter.GetBytes(RearTireLongForceBias));
            bytes.Add(BitConverter.GetBytes(RearTireDynamicFrictionCoefficient));
            bytes.Add(BitConverter.GetBytes(RearTireAdhesiveLimit));
            bytes.Add(BitConverter.GetBytes(RearLongGripCurvePeakSlipRatio));
            bytes.Add(BitConverter.GetBytes(RearLongGripCurvePeakCoefficient));
            bytes.Add(BitConverter.GetBytes(RearLongGripCurveFloorSlipRatio));
            bytes.Add(BitConverter.GetBytes(RearLongGripCurveFallCoefficient));
            bytes.Add(BitConverter.GetBytes(RearLatGripCurvePeakSlipRatio));
            bytes.Add(BitConverter.GetBytes(RearLatGripCurvePeakCoefficient));
            bytes.Add(BitConverter.GetBytes(RearLatGripCurveFloorSlipRatio));
            bytes.Add(BitConverter.GetBytes(RearLatGripCurveFallCoefficient));
            bytes.Add(BitConverter.GetBytes(RearLatGripCurveDriftPeakSlipRatio));
            bytes.Add(BitConverter.GetBytes(PowerToRear));
            bytes.Add(BitConverter.GetBytes(PowerToFront));
            bytes.Add(BitConverter.GetBytes(PitchDampingOnTakeOff));
            bytes.Add(BitConverter.GetBytes(MaxSpeed));
            bytes.Add(BitConverter.GetBytes(MagicBrakeFactorTurning));
            bytes.Add(BitConverter.GetBytes(MagicBrakeFactorStraightLine));
            bytes.Add(BitConverter.GetBytes(LowSpeedTyreFrictionTractionControl));
            bytes.Add(BitConverter.GetBytes(LowSpeedThrottleTractionControl));
            bytes.Add(BitConverter.GetBytes(LowSpeedDrivingSpeed));
            bytes.Add(BitConverter.GetBytes(LockBrakeScale));
            bytes.Add(BitConverter.GetBytes(LinearDrag));
            bytes.Add(BitConverter.GetBytes(HighSpeedAngularDamping));
            bytes.Add(BitConverter.GetBytes(FrontWheelMass));
            bytes.Add(BitConverter.GetBytes(FrontTireStaticFrictionCoefficient));
            bytes.Add(BitConverter.GetBytes(FrontTireLongForceBias));
            bytes.Add(BitConverter.GetBytes(FrontTireDynamicFrictionCoefficient));
            bytes.Add(BitConverter.GetBytes(FrontTireAdhesiveLimit));
            bytes.Add(BitConverter.GetBytes(FrontLongGripCurvePeakSlipRatio));
            bytes.Add(BitConverter.GetBytes(FrontLongGripCurvePeakCoefficient));
            bytes.Add(BitConverter.GetBytes(FrontLongGripCurveFloorSlipRatio));
            bytes.Add(BitConverter.GetBytes(FrontLongGripCurveFallCoefficient));
            bytes.Add(BitConverter.GetBytes(FrontLatGripCurvePeakSlipRatio));
            bytes.Add(BitConverter.GetBytes(FrontLatGripCurvePeakCoefficient));
            bytes.Add(BitConverter.GetBytes(FrontLatGripCurveFloorSlipRatio));
            bytes.Add(BitConverter.GetBytes(FrontLatGripCurveFallCoefficient));
            bytes.Add(BitConverter.GetBytes(FrontLatGripCurveDriftPeakSlipRatio));
            bytes.Add(BitConverter.GetBytes(DrivingMass));
            bytes.Add(BitConverter.GetBytes(DriveTimeDeformLimitX));
            bytes.Add(BitConverter.GetBytes(DriveTimeDeformLimitPosZ));
            bytes.Add(BitConverter.GetBytes(DriveTimeDeformLimitNegZ));
            bytes.Add(BitConverter.GetBytes(DriveTimeDeformLimitNegY));
            bytes.Add(BitConverter.GetBytes(DownForceZOffset));
            bytes.Add(BitConverter.GetBytes(DownForce));
            bytes.Add(BitConverter.GetBytes(CrashExtraYawVelocityFactor));
            bytes.Add(BitConverter.GetBytes(CrashExtraRollVelocityFactor));
            bytes.Add(BitConverter.GetBytes(CrashExtraPitchVelocityFactor));
            bytes.Add(BitConverter.GetBytes(CrashExtraLinearVelocityFactor));
            bytes.Add(BitConverter.GetBytes(AngularDrag));
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
            RearRightWheelPosition = br.ReadVector3I();
            FrontRightWheelPosition = br.ReadVector3I();
            CoMOffset = br.ReadVector3I();
            BrakeScaleToFactor = br.ReadVector3I();
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
