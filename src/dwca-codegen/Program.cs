using DwcaCodegen.CommandLine;
using System.CommandLine;

namespace DwcaCodegen
{
    class Program
    {
        static int Main(string[] args)
        {
            var application = new Application();
            return new Commands(application, application)
                .Root
                .InvokeAsync(args)
                .Result;
        }
    }
}
