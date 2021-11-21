using DotNetConfig;
using DwC_A.Terms;

namespace DwcaCodegen.Config
{
    public class DefaultConfigurationBuilder
    {
        private const string DefaultSection = "dwca-codegen";
        private const string PropertySection = "properties";

        private DotNetConfig.Config config;

        public DefaultConfigurationBuilder()
        {
            config = DotNetConfig.Config.Build(ConfigUtils.FullConfigFilePath);
        }

        public void Init()
        {
            config.SetBoolean(DefaultSection, "pascalCase", true)
                .SetBoolean(DefaultSection, "mapMethod", false)
                .SetString(DefaultSection, "termAttribute", "none")
                .SetString(DefaultSection, "namespace", "DwC")
                .SetString(DefaultSection, "output", ".")
                .SetString(DefaultSection, "usings", "using", "System");

            WriteProperty(Terms.coordinatePrecision, "double?");
            WriteProperty(Terms.coordinateUncertaintyInMeters, "double?");
            WriteProperty(Terms.dateIdentified, "DateTime?");
            WriteProperty(Terms.day, "int?");
            WriteProperty(Terms.decimalLatitude, "double?");
            WriteProperty(Terms.decimalLongitude, "double?");
            WriteProperty(Terms.endDayOfYear, "int?");
            WriteProperty(Terms.footprintSpatialFit, "double?");
            WriteProperty(Terms.individualCount, "int?");
            WriteProperty(Terms.maximumDepthInMeters, "double?");
            WriteProperty(Terms.maximumDistanceAboveSurfaceInMeters, "double?");
            WriteProperty(Terms.measurementDeterminedDate, "DateTime?");
            WriteProperty(Terms.minimumDistanceAboveSurfaceInMeters, "double?");
            WriteProperty(Terms.minimumElevationInMeters, "double?");
            WriteProperty(Terms.month, "int?");
            WriteProperty(Terms.namePublishedInYear, "int?");
            WriteProperty(Terms.pointRadiusSpatialFit, "double?");
            WriteProperty(Terms.relationshipEstablishedDate, "DateTime?");
            WriteProperty(Terms.sampleSizeValue, "int?");
            WriteProperty(Terms.startDayOfYear, "int?");
            WriteProperty(Terms.year, "int?");
        }

        public void WriteProperty(string term, string typeName, bool include = true, string propertyName = null)
        {
            config = DotNetConfig.Config.Build(ConfigUtils.FullConfigFilePath);
            config.SetString(PropertySection, term, "typeName", typeName)
                .SetBoolean(PropertySection, term, "include", include);
        }
    }
}
