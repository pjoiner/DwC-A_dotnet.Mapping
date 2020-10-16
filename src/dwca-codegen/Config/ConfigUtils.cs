using System;
using System.IO;
using System.Reflection;

namespace DwcaCodegen.Config
{
    public static class ConfigUtils
    {
        public static string Location => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static bool IsLocal => Location.Contains(".nuget");

        public static bool IsGlobal => Location.Contains(".dotnet\\tools");

        public static string DefaultConfig => Path.Combine(Location, ".config", "default.json");

        public static string ConfigLocation
        {
            get
            {
                if (IsGlobal) 
                {
                    return Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            "dwca-codegen" );
                }
                var configRoot = Directory.GetCurrentDirectory();
                var configDirectory = Path.Combine(configRoot, ".config");
                while (!Directory.Exists(configDirectory))
                {
                    configRoot = Directory.GetParent(configRoot).FullName;
                    configDirectory = Path.Combine(configRoot, ".config");
                }
                return configDirectory;
            }
        }
    }
}
