using System;
using System.IO;
using System.Reflection;

namespace DwcaCodegen.Config
{
    public static class ConfigUtils
    {
        private const string ConfigFileName = ".dwca-codegen";

        public static readonly string DefaultSection = "dwca-codegen";
        public static readonly string PropertySection = "properties";


        public static string Location => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static bool IsLocal => Location.Contains(".nuget");

        public static bool IsGlobal => Location.Contains(".dotnet\\tools");

        public static string DefaultConfig => Path.Combine(Location, ".config", ConfigFileName);

        public static string FullConfigFilePath => Path.Combine(ConfigLocation, ConfigFileName);

        public static string ConfigLocation
        {
            get
            {
                if (IsGlobal)
                {
                    return Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                            "dwca-codegen");
                }
                return Directory.GetCurrentDirectory();
            }
        }
    }
}