﻿using paperviz.ScanPreview;
using Prism;
using Prism.Ioc;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace paperviz
{
    public partial class App
    {
        public App() : this(null) { }
        
        public static double ScreenWidth =>
            DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
        public static double ScreenHeight =>
            DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
        
        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }
        

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage>();
            containerRegistry.RegisterForNavigation<ScanPreviewPage>();

            containerRegistry.RegisterSingleton<CurrentImageService>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");            
        }

        protected override void OnResume()
        {
        }
    }
}
