using PluginAPI;
using PluginSystem.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PluginSystem
{
    public static class PluginLoader
    {
        private static readonly Dictionary<string, Plugin> _plugins = new Dictionary<string, Plugin>();

        private delegate void OnLog(string message);
        private static event OnLog Log;

        private static void LogInfo(string message)
        {
            if (Log == null)
            {
                Console.WriteLine(message);
                //Debug.WriteLine(message);
            }

            Log?.Invoke(message);
        }

        private static void ScanPluginsFolder()
        {
            LogInfo("Scanning Plugins Folder");

            string directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");
            Directory.CreateDirectory(directory);
            string[] pluginDLLs = Directory.GetFiles(directory, "*.dll", SearchOption.TopDirectoryOnly);

            foreach (string pluginDLL in pluginDLLs)
            {
                try
                {
                    Assembly.LoadFile(pluginDLL);
                }
                catch (Exception ex)
                {
                    LogInfo(ex.Message + "\n" + ex.StackTrace);
                }
            }
        }

        private static void FindPlugins()
        {
            LogInfo("Finding Plugins");

            Type pluginType = typeof(Plugin);

            List<Assembly> assemblies = AssemblyUtil.BuildAssemblyList();

            foreach (Assembly assembly in assemblies)
            {
                //LogInfo("Scanning Assembly: " + assembly.GetName().Name);

                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsSubclassOf(pluginType))
                        continue;

                    try
                    {
                        Plugin plugin = Activator.CreateInstance(type, true) as Plugin;
                        if (plugin == null)
                        {
                            throw new InvalidCastException("Type is not plugin");
                        }

                        if (plugin.GetID() == null)
                        {
                            LogInfo(type.FullName + " is missing Plugin ID!");
                            continue;
                        }

                        if (_plugins.ContainsKey(plugin.GetID()))
                            continue;
                        _plugins.Add(plugin.GetID(), plugin);

                    } catch (Exception ex)
                    {
                        string err = ex.Message + "\n" + ex.StackTrace;
                        LogInfo(err);
                        continue;
                    }
                }
            }
        }

        private static void InitPlugin(Plugin plugin)
        {
            LogInfo("Initializing Plugin (" + plugin.GetID() + ":" + plugin.GetName() + ")");
            plugin.Init();
        }

        private static void InitPlugins()
        {
            LogInfo("Initializing Plugins");
            foreach (var plugin in _plugins)
            {
                InitPlugin(plugin.Value);
            }
        }

        public static void LoadPlugins()
        {
            LogInfo("Loading Plugins");
            ScanPluginsFolder();
            FindPlugins();
            InitPlugins();
        }
    }
}
