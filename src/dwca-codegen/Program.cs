using DwcaCodegen.CommandLine;
using System.CommandLine;

namespace DwcaCodegen
{
    class Program
    {
        static int Main(string[] args)
        {
            return new GenerateCommand()
                .RootCommand
                .InvokeAsync(args)
                .Result;
        }
    }
}
