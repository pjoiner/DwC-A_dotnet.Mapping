using System.Runtime.CompilerServices;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace Tests
{
    public static class ModuleInitializer
    {
        [ModuleInitializer]
        public static void Init()
        {
            VerifySourceGenerators.Initialize();
        }
    }

}
