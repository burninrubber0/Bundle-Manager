using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BundleFormat;

namespace BundleManager
{
    public class MaterialEntry
    {
        public Image DiffuseMap { get; set; }
        public Image NormalMap { get; set; }
        public Image SpecularMap { get; set; }

        public MaterialEntry()
        {
            
        }

        public static MaterialEntry Read(BundleEntry entry)
        {
            MaterialEntry result = new MaterialEntry();

            // TODO: Read Material

            return result;
        }
    }
}
