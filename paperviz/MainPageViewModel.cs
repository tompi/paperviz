using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace paperviz
{
    public class MainPageViewModel : ViewModelBase
    {
        private IMediaServices _mediaServices;

        public MainPageViewModel()
        {
            ScanCommand = new AsyncCommand(Scan);
            _mediaServices = DependencyService.Get<IMediaServices>();
        }

        private async Task Scan()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status != PermissionStatus.Granted) return;
            
            var image = await _mediaServices.GetImageWithCamera();
        }

        public AsyncCommand ScanCommand { get; }
    }
}