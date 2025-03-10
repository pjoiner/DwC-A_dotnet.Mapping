﻿using DotNetConfig;
using DwC_A.Terms;
using System.IO;

namespace DwcaCodegen.Config;

public class DefaultConfigurationBuilder
{
    private DotNetConfig.Config config;

    public void Init()
    {
        if(File.Exists(ConfigUtils.FullConfigFilePath))
        {
            File.Delete(ConfigUtils.FullConfigFilePath);
        }
        config = DotNetConfig.Config.Build(ConfigUtils.FullConfigFilePath);

        config = config.SetBoolean(ConfigUtils.DefaultSection, "pascalCase", true)
            .SetBoolean(ConfigUtils.DefaultSection, "mapMethod", false)
            .SetString(ConfigUtils.DefaultSection, "termAttribute", "none")
            .SetString(ConfigUtils.DefaultSection, "namespace", "DwC")
            .SetString(ConfigUtils.DefaultSection, "output", ".")
            .SetString(ConfigUtils.DefaultSection, "usings", "using", "System");

        WriteProperty("*", "string");
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

    private void WriteProperty(string term, string typeName, bool include = true)
    {
        config = config.SetString(ConfigUtils.PropertySection, term, "typeName", typeName)
            .SetBoolean(ConfigUtils.PropertySection, term, "include", include);
    }
}
