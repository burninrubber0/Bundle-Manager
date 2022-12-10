using BundleFormat;
using BundleUtilities;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace BaseHandlers
{

    public struct StartingGrid {
        public Vector3I[] maStartingPositions; //0x0	0x80	Vector3[8]	maStartingPositions	
        public Vector3I[] maStartingDirections; //0x80	0x80	Vector3[8]	maStartingDirections	
    }

    public struct BoxRegion {

        public float mPositionX { get; set; } //0x0	0x4	float32_t	mPositionX
        public float mPositionY { get; set; } //0x4	0x4	float32_t	mPositionY	
        public float mPositionZ { get; set; } //0x8	0x4	float32_t	mPositionZ	
        public float mRotationX { get; set; } //0xC	0x4	float32_t	mRotationX
        public float mRotationY { get; set; } //0x10	0x4	float32_t	mRotationY	
        public float mRotationZ { get; set; } //0x14	0x4	float32_t	mRotationZ	
        public float mDimensionX { get; set; } //0x18	0x4	float32_t	mDimensionX	
        public float mDimensionY { get; set; } //0x1C	0x4	float32_t	mDimensionY
        public float mDimensionZ { get; set; } //0x20	0x4	float32_t mDimensionZ
    }

    public struct TriggerRegion {
        public BoxRegion mBoxRegion; //0x0	0x24	BoxRegion	mBoxRegion	
        public int mId; //0x24	0x4	int32_t	mId	
        public Int16 miRegionIndex; //0x28	0x2	int16_t	miRegionIndex	Region index	
        public short meType; //0x2A	0x1	uint8_t	meType	Region type	See type
        public short muPad; //0x2B	0x1	uint8_t[1]	muPad
    }

    public struct Landmark
    {
        public TriggerRegion triggerRegion; //0x0	0x2C	TriggerRegion	super_TriggerRegion	
        //0x2C	0x4			padding
        public int mpaStartingGrids; // 0x30	0x8	StartingGrid*	mpaStartingGrids	StartingGrid	
        public short miStartingGridCount { get; set; } //0x38	0x1	int8_t	miStartingGridCount		Always 0
        public byte muDesignIndex { get; set; } //0x39	0x1	uint8_t	muDesignIndex	Landmark index	
        public byte muDistrict { get; set; } //0x3A	0x1	uint8_t	muDistrict	District ID	Always 3. See Districts
        public byte mu8Flags { get; set; } //0x3B	0x1	uint8_t	mu8Flags	Flags	See flags
        //0x3C	0x4			padding
    }

    public struct GenericRegion
    {
        public TriggerRegion triggerRegion; //0x0	0x2C	TriggerRegion	super_TriggerRegion	
        public int miGroupID { get; set; } //0x2C	0x4	int32_t	miGroupID	GameDB ID
        public short miCameraCut1 { get; set; } //0x30	0x2	int16_t	miCameraCut1
        public short miCameraCut2 { get; set; } //0x32	0x2	int16_t miCameraCut2
        public byte miCameraType1 { get; set; } //0x34	0x1	int8_t	miCameraType1	Stunt camera type 1	See stunt camera type
        public byte miCameraType2 { get; set; } //0x35	0x1	int8_t	miCameraType2	Stunt camera type 2	See stunt camera type
        public byte meType { get; set; } //0x36	0x1	uint8_t	meType	Generic region type	See type
        public byte miIsOneWay { get; set; } //0x37	0x1	int8_t	miIsOneWay
    }

    public struct Blackspot {
        public TriggerRegion triggerRegion; //0x0	0x2C	TriggerRegion	super_TriggerRegion	
        public ushort muScoreType; //0x2C	0x1	uint8_t	muScoreType		See score type
        //0x2D	0x3			padding
        public int miScoreAmount; //0x30	0x4	int32_t	miScoreAmount	
    }

    public struct Killzone
    {
        public int mppTriggers;//0x0	0x8	GenericRegion** mppTriggers GenericRegion pointer array	
        public int miTriggerCount; //0x8	0x4	int32_t miTriggerCount  Number of generic regions	
        //0xC	0x4			padding	
        public int mpRegionIds; //0x10	0x8	CgsID* mpRegionIds Regions GameDB IDs	
        public int miRegionIdCount; //0x18	0x4	int32_t miRegionIdCount Number of region IDs	
        //0x1C	0x4			padding
    }

    public struct SignatureStunt {
        public UInt64 mId; //0x0	0x8	CgsID	mId	GameDB ID	
        public long miCamera; //0x8	0x8	int64_t	miCamera	
        public int mppStuntElements; //0x10	0x4	GenericRegion**	mppStuntElements	
        public int miStuntElementCount; //0x14	0x4	int32_t miStuntElementCount
        //0x1C	0x4			padding	

    }

    public struct RoamingLocation
    {
        public Vector3I mPosition { get; set; }   //0x0	0x10	Vector3	mPosition		     
        public uint muDistrictIndex { get; set; } //0x10	0x1	uint8_t	muDistrictIndex		See Districts
        //0x11	0xF			padding
    }

    public struct VFXBoxRegion {
        public TriggerRegion region; //0x0	0x2C	TriggerRegion	super_TriggerRegion	
    }

    public struct SpawnLocation
    {
        public Vector3I mPosition { get; set; } //0x0	0x10	Vector3	mPosition
        public Vector3I mDirection { get; set; } //0x10	0x10	Vector3	mDirection	
        public UInt64 mJunkyardId { get; set; } //0x20	0x8	CgsID	mJunkyardId	GameDB ID	
        public uint muType { get; set; } //0x28	0x1	uint8_t	muType	Type	See type
        // 0x29	0x7			padding

    }



    public class TriggerData : IEntryData
    {
        public int miVersionNumber { get; set; } //0x0	0x4	int32_t	miVersionNumber	Resource version	42
        public uint muSize { get; set; } //0x4	0x4	uint32_t	muSize	Resource size
         //0x8	0x8			padding
        public Vector3I mPlayerStartPosition { get; set; } //0x10	0x10	Vector3	mPlayerStartPosition	Dev start position	
        public Vector3I mPlayerStartDirection { get; set; } //0x20	0x10	Vector3	mPlayerStartDirection	Dev start direction	
        public uint mpLandmarks { get; set; } //0x30	0x8	Landmark*	mpLandmarks	Landmarks
        public int miLandmarkCount { get; set; } //0x38	0x4	int32_t	miLandmarkCount	Number of landmarks
        public int miOnlineLandmarkCount { get; set; } //0x3C	0x4	int32_t	miOnlineLandmarkCount	Number of online landmarks	
        public uint mpSignatureStunts { get; set; } // 0x40	0x8	SignatureStunt*	mpSignatureStunts	Signature stunts	
        public int miSignatureStuntCount { get; set; } //0x48	0x4	int32_t	miSignatureStuntCount	Number of signature stunts
        //0x4C	0x4			padding
        public uint mpGenericRegions { get; set; } //0x50	0x8	GenericRegion*	mpGenericRegions	Generic regions
        public int miGenericRegionCount { get; set; } //0x58	0x4	int32_t	miGenericRegionCount	Number of generic regions
        //0x5C	0x4			padding	
        public uint mpKillzones { get; set; } //0x60	0x8	Killzone*	mpKillzones	Killzones
        public int miKillzoneCount { get; set; } // 0x68	0x4	int32_t	miKillzoneCount	Number of killzones
        //0x6C	0x4			padding
        public uint mpBlackspots { get; set; } //0x70	0x8	Blackspot* mpBlackspots    Blackspots
        public int miBlackspotCount { get; set; } //0x78	0x4	int32_t	miBlackspotCount	Number of blackspots
        //0x7C	0x4			padding
        public uint mpVFXBoxRegions { get; set; } //0x80	0x8	VFXBoxRegion*	mpVFXBoxRegions	VFX box regions	
        public int miVFXBoxRegionCount { get; set; } //0x88	0x4	int32_t	miVFXBoxRegionCount	Number of VFX box regions
        //0x8C	0x4			padding
        public uint mpRoamingLocations { get; set; } //0x90	0x8	RoamingLocation*	mpRoamingLocations	Roaming locations
        public int miRoamingLocationCount { get; set; } //0x98	0x4	int32_t	miRoamingLocationCount	Number of roaming locations
        //0x9C	0x4			padding
        public uint mpSpawnLocations { get; set; } //0xA0	0x8	SpawnLocation*	mpSpawnLocations	Spawn locations	
        public int miSpawnLocationCount { get; set; } //0xA8	0x4	int32_t	miSpawnLocationCount	Number of spawn locations
        //0xAC	0x4			padding
        public uint mppRegions { get; set; } //0xB0	0x8	TriggerRegion**	mppRegions	Trigger regions	Generic regions used in Killzones, landmarks for debug only
        public int miRegionCount { get; set; } //0xB8	0x4	int32_t	miRegionCount	Number of regions
        //0xBC	0x4			padding


        public TriggerData()
        {
        }

        private void Clear()
        {
        }

        public bool Read(BundleEntry entry, ILoader loader = null)
        {
            Clear();

            MemoryStream ms = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(ms);
            br.BigEndian = entry.Console;

            return true;
        }

        public IEntryEditor GetEditor(BundleEntry entry)
        {
            TriggerDataEditor triggerDataEditor = new TriggerDataEditor();
            triggerDataEditor.trigger = this;
            triggerDataEditor.EditEvent += () =>
            {
                Write(entry);
            };
            return triggerDataEditor;
        }

        public EntryType GetEntryType(BundleEntry entry)
        {
            return EntryType.TriggerData;
        }

        public bool Write(BundleEntry entry)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            return true;
        }
    }
}
