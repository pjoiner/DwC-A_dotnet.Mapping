using DwC_A;
using DwC_A.Config;
using DwC_A.Generator;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DwcaCodegen.Generator;

public class ArchiveSourceGenerator : IArchiveSourceGenerator
{
    public string[] GenerateSource(string fileName, IGeneratorConfiguration config)
    {
        List<string> sourceFiles = [];
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
            var coreSource = ClassGenerator.GenerateFile(metaData, config);
            File.WriteAllText(sourceFileName, coreSource, Encoding.UTF8);
            foreach (var extension in archive.Extensions.GetFileReaders())
            {
                var extensionFileName = CreateSourceFileName(extension.FileName, outputPath, config);
                sourceFiles.Add(extensionFileName);
                var meta = extension.FileMetaData;
                var extensionSource = ClassGenerator.GenerateFile(meta, config);
                File.WriteAllText(extensionFileName, extensionSource, Encoding.UTF8);
            }
        }
        return [.. sourceFiles];
    }

    private static string CreateSourceFileName(string fileName, string outputPath, IGeneratorConfiguration config)
    {
        var sourceFileName = Path.GetFileNameWithoutExtension(fileName);
        if (config.PascalCase)
        {
            sourceFileName = char.ToUpper(sourceFileName[0]) + sourceFileName[1..];
        }
        return Path.Combine(outputPath, sourceFileName + ".cs");
    }
}
