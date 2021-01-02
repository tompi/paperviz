using Prism;
using Prism.Ioc;

namespace paperviz.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IOcrService, OcrService>();

        }
    }
}