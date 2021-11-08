using DwC_A;
using DwcaCodegen.Config;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DwcaCodegen.Generator
{
    public class ArchiveSourceGenerator : IArchiveSourceGenerator
    {
        private readonly ClassGenerator roslynClassGenerator;

        public ArchiveSourceGenerator(ClassGenerator classGenerator)
        {
            roslynClassGenerator = classGenerator;
        }

        public string[] GenerateSource(string fileName, IArchiveGeneratorConfiguration config)
        {
            IList<string> sourceFiles = new List<string>();
            using (var archive = new ArchiveReader(fileName))
            {
                var outputPath = config.Output;
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                string sourceFileName = CreateSourceFileName(archive.CoreFile.FileName, outputPath, config);
                sourceFiles.Add(sourceFileName);
                var metaData = archive.CoreFile.FileMetaData;
                var coreSource = roslynClassGenerator.GenerateFile(metaData, config);
                File.WriteAllText(sourceFileName, coreSource, Encoding.UTF8);
                foreach (var extension in archive.Extensions.GetFileReaders())
                {
                    var extensionFileName = CreateSourceFileName(extension.FileName, outputPath, config); 
                    sourceFiles.Add(extensionFileName);
                    var meta = extension.FileMetaData;
                    var extensionSource = roslynClassGenerator.GenerateFile(meta, config);
                    File.WriteAllText(extensionFileName, extensionSource, Encoding.UTF8);
                }
            }
            return sourceFiles.ToArray();
        }

        private string CreateSourceFileName(string fileName, string outputPath, IArchiveGeneratorConfiguration config)
        {
            var sourceFileName = Path.GetFileNameWithoutExtension(fileName); 
            if (config.PascalCase)
            {
                sourceFileName = char.ToUpper(sourceFileName[0]) + sourceFileName[1..];
            }
            return Path.Combine(outputPath, sourceFileName + ".cs");
        }
    }
}
