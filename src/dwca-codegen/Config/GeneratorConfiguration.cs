using System.Collections.Generic;

namespace DwcaCodegen.Config
{
    public class GeneratorConfiguration
    {
        public bool PascalCase { get; set; } = true;
        public bool TermAttribute { get; set; } = false;
        public string Namespace { get; set; } = "DwC";
        public string Output { get; set; } = ".";
        public IList<string> Usings { get; set; } = new List<string>();
        public IDictionary<string, PropertyConfiguration> Properties { get; set; } =
            new Dictionary<string, PropertyConfiguration>();
    }
}
