using BundleFormat;
using BundleUtilities;
using ModelViewer.SceneData;
using PluginAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaseHandlers
{
	public class BasePlugin : Plugin
	{
		public override void Init()
		{
			EntryTypeRegistry.Register(EntryType.PolygonSoupListResourceType, new PolygonSoupList());
			EntryTypeRegistry.Register(EntryType.TriggerResourceType, new TriggerData());
			EntryTypeRegistry.Register(EntryType.StreetDataResourceType, new StreetData());
			EntryTypeRegistry.Register(EntryType.ProgressionResourceType, new ProgressionData());
			EntryTypeRegistry.Register(EntryType.IDList, new IDList());
			EntryTypeRegistry.Register(EntryType.TrafficDataResourceType, new Traffic());
			EntryTypeRegistry.Register(EntryType.FlaptFileResourceType, new FlaptFile());
			//EntryTypeRegistry.Register(EntryType.AptDataHeaderType, new AptData());
			EntryTypeRegistry.Register(EntryType.InstanceListResourceType, new InstanceList());
			EntryTypeRegistry.Register(EntryType.GraphicsSpecResourceType, new GraphicsSpec());
			EntryTypeRegistry.Register(EntryType.RwRenderableResourceType, new Renderable());

			PluginCommandRegistry.Register("dump_all_collisions", "Dump All Collisions", DumpAllCollisions, IsWorldCol);
			PluginCommandRegistry.Register("remove_wreck_surfaces", "Remove Wreck Surfaces", RemoveWreckSurfaces, IsWorldCol);
		}

		public override string GetID()
		{
			return "baseplugin";
		}

		public override string GetName()
		{
			return "Base Resource Handlers";
		}

		#region Extra Tools

		private bool IsWorldCol(BundleArchive archive)
		{
			for (int i = 0; i < archive.Entries.Count; i++)
			{
				BundleEntry entry = archive.Entries[i];

				if (entry.Type == EntryType.PolygonSoupListResourceType)
					return true;
			}

			return false;
		}

		private void DumpAllCollisions(IWin32Window window, BundleArchive archive)
		{
			if (archive == null)
				return;

			FolderBrowserDialog fbd = new FolderBrowserDialog();
			DialogResult result = fbd.ShowDialog(window);
			if (result == DialogResult.OK)
			{
				string path = fbd.SelectedPath;

				for (int i = 0; ; i++)
				{
					string idListName = "trk_clil" + i;
					string polyName = "trk_col_" + i;

					ulong idListID = Crc32.HashCrc32B(idListName);
					ulong polyID = Crc32.HashCrc32B(polyName);

					BundleEntry entry = archive.GetEntryByID(idListID);
					if (entry == null)
						break;
					Stream outFile = File.Open(path + "/" + idListName + ".bin", FileMode.Create, FileAccess.Write);
					BinaryWriter bw = new BinaryWriter(outFile);
					bw.Write(entry.EntryBlocks[0].Data);
					bw.Flush();
					bw.Close();
					outFile.Close();

					BundleEntry polyEntry = archive.GetEntryByID(polyID);
					if (polyEntry == null)
						break;
					Stream outFilePoly = File.Open(path + "/" + polyName + ".bin", FileMode.Create, FileAccess.Write);
					BinaryWriter bwPoly = new BinaryWriter(outFilePoly);
					bwPoly.Write(polyEntry.EntryBlocks[0].Data);
					bwPoly.Flush();
					bwPoly.Close();
					outFilePoly.Close();

					PolygonSoupList poly = new PolygonSoupList();
					poly.Read(polyEntry);
					Scene scene = poly.MakeScene();
					scene.ExportWavefrontObj(path + "/" + polyName + ".obj");
				}

				MessageBox.Show(window, "Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}

		private void RemoveWreckSurfaces(IWin32Window window, BundleArchive archive)
		{
			for (int i = 0; i < archive.Entries.Count; i++)
			{
				BundleEntry entry = archive.Entries[i];

				if (entry.Type == EntryType.PolygonSoupListResourceType)
				{
					PolygonSoupList list = new PolygonSoupList();
					list.Read(entry);
					list.RemoveWreckSurfaces();
					list.Write(entry);
				}
			}
		}

		/*public void ConvertImagesFromPS3ToPC_old(IWin32Window window, BundleArchive archive)
		{
			if (archive == null)
			{
				MessageBox.Show(window, "No Archive Open!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return;
			}
			if (archive.Console)
			{
				for (int i = 0; i < archive.Entries.Count; i++)
				{
					BundleEntry entry = archive.Entries[i];
					if (entry.EntryBlocks[0].Data.Length == 48 && entry.EntryBlocks[1].Data != null && entry.EntryBlocks[1].Data.Length > 0)
					{
						MemoryStream ms = new MemoryStream(entry.EntryBlocks[0].Data);
						BinaryReader2 br = new BinaryReader2(ms);
						br.BigEndian = entry.Console;

						byte compression = br.ReadByte();
						byte[] unknown1 = br.ReadBytes(3);
						byte[] type = Encoding.ASCII.GetBytes("DXT1");
						if (compression == 0x85)
						{
							type = new byte[] { 0x15, 0x00, 0x00, 0x00 };
						}
						else if (compression == 0x86)
						{
							type = Encoding.ASCII.GetBytes("DXT1");
						}
						else if (compression == 0x88)
						{
							type = Encoding.ASCII.GetBytes("DXT5");
						}
						int unknown2 = Util.ReverseBytes(br.ReadInt32());
						int width = Util.ReverseBytes(br.ReadInt16());
						int height = Util.ReverseBytes(br.ReadInt16());
						br.Close();

						MemoryStream msx = new MemoryStream();
						BinaryWriter bw = new BinaryWriter(msx);

						bw.Write((int)0);
						bw.Write((int)0);
						bw.Write((int)0);
						bw.Write((int)1);

						bw.Write(type);
						bw.Write((short)width);
						bw.Write((short)height);
						bw.Write((int)0x15);
						bw.Write((int)0);

						bw.Flush();

						byte[] Data = msx.ToArray();

						bw.Close();

						entry.EntryBlocks[0].Data = Data;

						entry.Dirty = true;
					}
				}
			}
			else
			{
				MessageBox.Show(window, "This feature only works on PS3 Bundle Files", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		public void ConvertToPC(IWin32Window window, BundleArchive archive)
		{
			// TODO: Support everything

			for (int i = 0; i < archive.Entries.Count; i++)
			{
				BundleEntry entry = archive.Entries[i];

				if (entry.Type == EntryType.IDList)
				{
					IDList list = new IDList();
					list.Read(entry);
					list.Write(entry);
				}
				else if (entry.Type == EntryType.PolygonSoupListResourceType)
				{
					PolygonSoupList list = new PolygonSoupList();
					list.Read(entry);
					list.Write(entry);
				}
			}

			//PatchImages();

			archive.Platform = BundlePlatform.PC;
		}*/

		#endregion
	}
}
