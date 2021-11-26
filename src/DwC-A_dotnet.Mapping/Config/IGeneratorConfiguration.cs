using System.Collections.Generic;

namespace DwC_A.Config
{
    public enum TermAttributeType
    {
        none,
        name,
        index
    }

    public interface IGeneratorConfiguration
    {
        string Namespace { get; }
        string Output { get; }
        bool PascalCase { get; }
        bool MapMethod { get; }
        IDictionary<string, PropertyConfiguration> Properties { get; }
        TermAttributeType TermAttribute { get; }
        IList<string> Usings { get; }
        PropertyConfiguration GetPropertyConfiguration(string term);
    }
}