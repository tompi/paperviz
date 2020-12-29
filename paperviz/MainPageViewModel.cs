using System;
using System.IO;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace paperviz
{
    public class MainPageViewModel : ViewModelBase
    {

        private readonly IOcrService _ocrService;
        
        public MainPageViewModel()
        {
            ScanCommand = new AsyncCommand(Scan);
            _ocrService = DependencyService.Get<IOcrService>();
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
                    Text = _ocrService.GetTexts(stream);
                }
                
                //await LoadPhotoAsync(photo);
                //Console.WriteLine($"CapturePhotoAsync COMPLETED: {PhotoPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"CapturePhotoAsync THREW: {ex.Message}");
            }
        }
        async Task LoadPhotoAsync(FileResult photo)
        {
            // canceled
            if (photo == null)
            {
                PhotoPath = null;
                return;
            }
            // save the file into local storage
            var newFile = Path.Combine(FileSystem.CacheDirectory, photo.FileName);
            using (var stream = await photo.OpenReadAsync())
            using (var newStream = File.OpenWrite(newFile))
                await stream.CopyToAsync(newStream);

            PhotoPath = newFile;
        }

        public string PhotoPath
        {
            get => Get("");
            set => Set(value);
        }
        public string Text
        {
            get => Get("");
            set => Set(value);
        }

        public AsyncCommand ScanCommand { get; }
    }
}