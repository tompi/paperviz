using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using paperviz.ScanPreview;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace paperviz
{
    public class MainPageViewModel : ViewModelBase
    {

        private readonly IOcrService _ocrService;
        private readonly CurrentImageService _currentImageService;
        private readonly INavigationService _navigationService;
        
        public MainPageViewModel(IOcrService ocrService, CurrentImageService currentImageService, INavigationService navigationService)
        {
            _ocrService = ocrService;
            _currentImageService = currentImageService;
            _navigationService = navigationService;
            ScanCommand = new AsyncCommand(Scan);
        }

        private async Task Scan()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status != PermissionStatus.Granted) return;
            
            try
            {
                var photo = await MediaPicker.CapturePhotoAsync();
                using (var stream = await photo.OpenReadAsync())
                {
                    var scanResults = await _ocrService.ProcessImage(stream);
                    _currentImageService.ScanResult = scanResults;
                    await _navigationService.NavigateAsync(nameof(ScanPreviewPage));
                }
                
                //await LoadPhotoAsync(photo);
                //Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        
        public AsyncCommand ScanCommand { get; }
    }
}