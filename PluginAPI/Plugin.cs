using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginAPI
{
    public abstract class Plugin
    {
        public virtual void Init()
        {
            // Stub
        }

        public abstract string GetID();

        public abstract string GetName();

        public virtual string GetDescription()
        {
            return null;
        }
    }
}
