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
            EntryTypeRegistry.Register(EntryType.TriggerData, new TriggerData());
            EntryTypeRegistry.Register(EntryType.StreetData, new StreetData());
            EntryTypeRegistry.Register(EntryType.ProgressionData, new ProgressionData());
            EntryTypeRegistry.Register(EntryType.EntryList, new IDList());
            EntryTypeRegistry.Register(EntryType.TrafficData, new Traffic());
            EntryTypeRegistry.Register(EntryType.FlaptFile, new FlaptFile());
            //EntryTypeRegistry.Register(EntryType.AptDataHeaderType, new AptData());
            EntryTypeRegistry.Register(EntryType.InstanceList, new InstanceList());
            EntryTypeRegistry.Register(EntryType.GraphicsSpec, new GraphicsSpec());
            EntryTypeRegistry.Register(EntryType.Renderable, new Renderable());
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
