using DwC_A;
using DwcaCodegen.Config;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DwcaCodegen.Generator
{
    public class ArchiveSourceGenerator : IArchiveSourceGenerator
    {
        private readonly ArchiveGeneratorConfiguration config;
        public ArchiveSourceGenerator(ArchiveGeneratorConfiguration config)
        {
            if (config == null)
            {
                this.config = new ArchiveGeneratorConfiguration();
            }
            else
            {
                this.config = config;
            }
        }

        public string[] GenerateSource(string fileName)
        {
            IList<string> sourceFiles = new List<string>();
            using (var archive = new ArchiveReader(fileName))
            {
                var roslynClassGenerator = new ClassGenerator(config);
                var outputPath = config.Output;
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
                string sourceFileName = CreateSourceFileName(archive.CoreFile.FileName, outputPath);
                sourceFiles.Add(sourceFileName);
                var metaData = archive.CoreFile.FileMetaData;
                var coreSource = roslynClassGenerator.GenerateFile(metaData);
                File.WriteAllText(sourceFileName, coreSource, Encoding.UTF8);
                foreach (var extension in archive.Extensions.GetFileReaders())
                {
                    var extensionFileName = CreateSourceFileName(extension.FileName, outputPath); 
                    sourceFiles.Add(extensionFileName);
                    var meta = extension.FileMetaData;
                    var extensionSource = roslynClassGenerator.GenerateFile(meta);
                    File.WriteAllText(extensionFileName, extensionSource, Encoding.UTF8);
                }
            }
            return sourceFiles.ToArray();
        }

        private string CreateSourceFileName(string fileName, string outputPath)
        {
            var sourceFileName = Path.GetFileNameWithoutExtension(fileName); 
            if (config.Capitalize)
            {
                sourceFileName = char.ToUpper(sourceFileName[0]) + sourceFileName.Substring(1);
            }
            return Path.Combine(outputPath, sourceFileName + ".cs");
        }
    }
}
