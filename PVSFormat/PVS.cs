using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;
using BundleUtilities;
using BurnoutImage;
using PluginAPI;

namespace PVSFormat
{
    public struct ZonePoint
    {
        public float X;
        public float Y;
        public int Padding1; // Padding???
		public int Padding2; // Padding???

		public override string ToString()
        {
            return "X: " + X + ", Y: " + Y + ", Padding1: " + Padding1 + ", Padding2: " + Padding2;
        }
    }

	public enum NeighbourFlags
	{
		E_RENDERFLAG_NONE = 0x0,
		E_NEIGHBOURFLAG_RENDER = 0x1,
		E_NEIGHBOURFLAG_IMMEDIATE = 0x2,
		E_NEIGHBOURFLAG_RENDER_IMMEDIATE = 0x3,
	}

    public struct ZoneNeighbour
    {
        public int NeighborIndex;
        public uint NeighborPtr;
        public NeighbourFlags Flags;
        public int Padding1; // Padding???
		public int Padding2; // Padding???

		public override string ToString()
        {
            return "NeighborIndex: " + NeighborIndex + ", Type: " + Flags + ", Unk1: " + Padding1 + ", Unk2: " + Padding2;
        }
    }

    public struct PVSZone
    {
        public uint Address;
        public uint PointsPtr;
        public uint SafeNeighboursPtr;
        public uint UnsafeNeighboursPtr;
        public uint Unknown;
        public long ZoneID;
		public short ZoneType;
		public short NumPoints;
        public short NumSafeNeighbours;
        public short NumUnsafeNeighbours;
        public int Flags;
        public int Padding1; // Padding???
		public int Padding2; // Padding???
		public int Padding3; // Padding???

		public List<ZonePoint> Points;
        public List<ZoneNeighbour> UnsafeNeighbours;

        public override string ToString()
        {
            return "ZoneID: " + ZoneID;
        }
    }

    public class PVS : IEntryData
    {
		private Image _gameMap;

		public uint PointsPtr;
		public uint ZonePtr;
		public uint ZonePointStart1;
		public uint ZonePointStart2;
		public uint ZonePointCount;
		public uint TotalZones;
        public uint TotalPoints;
		public uint Padding; // Padding???
		public List<PVSZone> Zones;

        public PVS()
        {
            Zones = new List<PVSZone>();
        }

		private void Clear()
		{
			PointsPtr = default;
			ZonePtr = default;
			ZonePointStart1 = default;
			ZonePointStart2 = default;
			ZonePointCount = default;
			TotalZones = default;
			TotalPoints = default;

			Zones.Clear();
		}

        public bool Read(BundleEntry entry, ILoader loader)
        {
			Clear();

            Stream s = entry.MakeStream();
            BinaryReader2 br = new BinaryReader2(s);
            br.BigEndian = entry.Console;

			PointsPtr = br.ReadUInt32();
			ZonePtr = br.ReadUInt32();

			ZonePointStart1 = br.ReadUInt32();
			ZonePointStart2 = br.ReadUInt32();

			ZonePointCount = br.ReadUInt32();
			TotalZones = br.ReadUInt32();
            TotalPoints = br.ReadUInt32();
			Padding = br.ReadUInt32();

            for (uint i = 0; i < ZonePointCount; i++)
            {
                PVSZone pvsEntry = new PVSZone();

                pvsEntry.Address = (uint)br.BaseStream.Position;
                pvsEntry.PointsPtr = br.ReadUInt32();
                pvsEntry.SafeNeighboursPtr = br.ReadUInt32();
                pvsEntry.UnsafeNeighboursPtr = br.ReadUInt32();
                pvsEntry.Unknown = br.ReadUInt32();
                pvsEntry.ZoneID = br.ReadInt64();
				pvsEntry.ZoneType = br.ReadInt16();
				pvsEntry.NumPoints = br.ReadInt16();
				pvsEntry.NumSafeNeighbours = br.ReadInt16();
                pvsEntry.NumUnsafeNeighbours = br.ReadInt16();
                pvsEntry.Flags = br.ReadInt32();
                pvsEntry.Padding1 = br.ReadInt32();
				pvsEntry.Padding2 = br.ReadInt32();
                pvsEntry.Padding3 = br.ReadInt32();

                long pos = br.BaseStream.Position;

                br.BaseStream.Position = (long)pvsEntry.PointsPtr;

                pvsEntry.Points = new List<ZonePoint>();
                for (int j = 0; j < pvsEntry.NumPoints; j++)
                {
                    ZonePoint data = new ZonePoint();

                    data.X = br.ReadSingle();
                    data.Y = br.ReadSingle();
                    data.Padding1 = br.ReadInt32();
                    data.Padding2 = br.ReadInt32();

                    pvsEntry.Points.Add(data);
                }

                br.BaseStream.Position = (long) pvsEntry.UnsafeNeighboursPtr;

                pvsEntry.UnsafeNeighbours = new List<ZoneNeighbour>();
                for (int j = 0; j < pvsEntry.NumUnsafeNeighbours; j++)
                {
                    ZoneNeighbour data = new ZoneNeighbour();

                    data.NeighborIndex = -1;

                    data.NeighborPtr = br.ReadUInt32();
                    data.Flags = (NeighbourFlags)br.ReadInt32();
                    data.Padding1 = br.ReadInt32();
                    data.Padding2 = br.ReadInt32();

                    pvsEntry.UnsafeNeighbours.Add(data);
                }

                br.BaseStream.Position = pos;

                Zones.Add(pvsEntry);
            }

            for (int i = 0; i < Zones.Count; i++)
            {
                for (int k = 0; k < Zones[i].UnsafeNeighbours.Count; k++)
                {
                    ZoneNeighbour data = Zones[i].UnsafeNeighbours[k];
                    uint ptr = data.NeighborPtr;
                    for (int j = 0; j < Zones.Count; j++)
                    {
                        if (Zones[j].Address == ptr)
                        {
                            data.NeighborIndex = j;
                            Zones[i].UnsafeNeighbours[k] = data;
                            break;
                        }
                    }
                }
            }

            br.Close();
            s.Close();

			_gameMap = GetGameMap(entry.Archive);

            return true;
        }

		public bool Write(BundleEntry entry)
        {
            return true;
        }

		public EntryType GetEntryType(BundleEntry entry)
		{
			return EntryType.ZoneList;
		}

		public IEntryEditor GetEditor(BundleEntry entry)
		{
			PVSEditor pvsForm = new PVSEditor();
			pvsForm.GameMap = _gameMap;
			pvsForm.Open(this);

			return pvsForm;
		}

		private Image GetGameMap(BundleArchive archive)
		{
			ulong id = 0x9F55039D;
			BundleEntry descEntry1 = archive.GetEntryByID(id);
			if (descEntry1 == null)
			{
				string file = BundleCache.GetFileByEntryID(id);
				if (!string.IsNullOrEmpty(file))
				{
					BundleArchive archive2 = BundleArchive.Read(file);
					if (archive2 != null)
						descEntry1 = archive2.GetEntryByID(id);
				}
			}

			if (descEntry1 == null)
			{
				string path = Path.GetDirectoryName(archive.Path) + Path.DirectorySeparatorChar + "GUITEXTURES.BIN";
				BundleArchive archive2 = BundleArchive.Read(path);
				if (archive2 != null)
					descEntry1 = archive2.GetEntryByID(id);
			}

			Image image = null;

			if (descEntry1 != null && descEntry1.Type == EntryType.Texture)
			{
				if (archive.Console)
					image = GameImage.GetImagePS3(descEntry1.EntryBlocks[0].Data, descEntry1.EntryBlocks[1].Data);
				else
					image = GameImage.GetImage(descEntry1.EntryBlocks[0].Data, descEntry1.EntryBlocks[1].Data);

				if (image != null)
					TextureCache.AddToCache(id, image);
			}
			return image;
		}
	}
}
