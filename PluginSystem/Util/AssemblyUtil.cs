using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem.Util
{
    public static class AssemblyUtil
    {
        private static bool ContainsAssembly(this List<Assembly> self, string fullname)
        {
            foreach (Assembly assembly in self)
            {
                if (assembly.FullName == fullname)
                    return true;
            }

            return false;
        }

        private static void AddAssemblyWithReferences(this List<Assembly> self, Assembly assembly)
        {
            if (!self.Contains(assembly))
                self.Add(assembly);

            AssemblyName[] references = assembly.GetReferencedAssemblies();
            foreach (AssemblyName reference in references)
            {
                if (self.ContainsAssembly(reference.FullName))
                    continue;

                try
                {
                    self.AddAssemblyWithReferences(Assembly.Load(reference));
                }
                catch { }
            }
        }

        internal static List<Assembly> BuildAssemblyList()
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                AddAssemblyWithReferences(assemblies, assembly);
            }

            return assemblies;
        }
    }
}
