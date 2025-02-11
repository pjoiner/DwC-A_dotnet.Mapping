using DotNetConfig;
using DwC_A.Config;
using System;
using System.Linq;
using System.Text;

namespace DwcaCodegen.Config;

public class ArchiveGeneratorConfigFactory(DotNetConfig.Config config)
{
    private const string TermAttributeNamespaceName = "DwC_A.Attributes";
    private const string ExtensionsNamespaceName = "DwC_A.Extensions";
    private const string SystemNamespaceName = "System";
    private const string DwcaNamespace = "DwC_A";
    private readonly DotNetConfig.Config config = config;

    public IGeneratorConfiguration BuildConfiguration()
    {
        var @namespace = config.GetString(ConfigUtils.DefaultSection, "namespace") ?? DwcaNamespace;
        var pascalCase = config.GetBoolean(ConfigUtils.DefaultSection, "pascalCase") ?? true;
        var termAttribute = Enum.Parse<TermAttributeType>(config.GetString(ConfigUtils.DefaultSection, "termAttribute") ?? "none");
        var output = config.GetString(ConfigUtils.DefaultSection, "output") ?? ".";
        var mapMethod = config.GetBoolean(ConfigUtils.DefaultSection, "mapMethod") ?? false;
        return BuildConfiguration(@namespace, pascalCase, termAttribute, output, mapMethod);
    }

    public IGeneratorConfiguration BuildConfiguration(
        string @namespace,
        bool pascalCase,
        TermAttributeType termAttribute,
        string output,
        bool mapMethod)
    {
        var archiveGeneratorConfiguration = new ArchiveGeneratorConfiguration()
            .AddNamespace(@namespace)
            .AddPascalCase(pascalCase)
            .AddTermAttribute(termAttribute)
            .AddOutput(output)
            .AddMapMethod(mapMethod);

        BuildUsings(archiveGeneratorConfiguration);

        BuildProperties(archiveGeneratorConfiguration);

        return archiveGeneratorConfiguration;
    }

    private void BuildProperties(ArchiveGeneratorConfiguration archiveGeneratorConfiguration)
    {
        var props = config.GetRegex(ConfigUtils.PropertySection);
        var keys = props
            .Select(n => n.Subsection)
            .Distinct();
        foreach (var key in keys)
        {
            archiveGeneratorConfiguration.Properties.Remove(key);
            archiveGeneratorConfiguration.Properties.Add(key, new PropertyConfiguration()
            {
                Include = config.GetBoolean(ConfigUtils.PropertySection, key, "include") ?? true,
                TypeName = config.GetString(ConfigUtils.PropertySection, key, "typeName"),
                PropertyName = config.GetString(ConfigUtils.PropertySection, key, "propertyName")
            });
        }
    }

    private void BuildUsings(ArchiveGeneratorConfiguration archiveGeneratorConfiguration)
    {
        var usingEntries = config.GetAll(ConfigUtils.DefaultSection, "usings", "using")
            .Select(n => n.RawValue);
        foreach (var entry in usingEntries)
        {
            archiveGeneratorConfiguration.AddUsing(entry);
        }
        archiveGeneratorConfiguration.AddUsing(SystemNamespaceName);
        if (archiveGeneratorConfiguration.TermAttribute != TermAttributeType.none)
        {
            archiveGeneratorConfiguration.AddUsing(TermAttributeNamespaceName);
        }
        if (archiveGeneratorConfiguration.MapMethod)
        {
            archiveGeneratorConfiguration.AddUsing(ExtensionsNamespaceName);
        }
    }
}
