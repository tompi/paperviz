using Prism;
using Prism.Ioc;

namespace paperviz.iOS
{
    public class IosInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IOcrService, OcrService>();

        }
    }
}