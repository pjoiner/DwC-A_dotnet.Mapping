using System.Collections.Generic;

namespace DwcaCodegen.Config
{
    public enum TermAttributeType
    {
        none,
        name,
        index
    }

    public interface IArchiveGeneratorConfiguration
    {
        string Namespace { get; }
        string Output { get; }
        bool PascalCase { get; }
        IDictionary<string, PropertyConfiguration> Properties { get; }
        TermAttributeType TermAttribute { get; }
        IList<string> Usings { get; }
        PropertyConfiguration GetPropertyConfiguration(string term);
    }
}