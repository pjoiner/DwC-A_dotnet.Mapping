using System.Collections.Generic;

namespace DwcaCodegen.Config
{
    public enum TermAttributeType
    {
        none,
        name,
        index
    }

    public class GeneratorConfiguration
    {
        public bool PascalCase { get; set; } = true;
        public TermAttributeType TermAttribute { get; set; } = TermAttributeType.none;
        public string Namespace { get; set; } = "DwC";
        public string Output { get; set; } = ".";
        public IList<string> Usings { get; set; } = new List<string>();
        public IDictionary<string, PropertyConfiguration> Properties { get; set; } =
            new Dictionary<string, PropertyConfiguration>();
    }
}
