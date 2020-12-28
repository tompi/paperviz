using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Xamarin.Essentials;
using Xamarin.Forms;

[assembly: Dependency(typeof(paperviz.Droid.MediaServices))]
namespace paperviz.Droid
{
    public class MediaServices : IMediaServices
    {
        private const int ImageRequest = 101;
        private static TaskCompletionSource<string> _cameraImagePathCompletionSource;
        
        public async Task<string> GetImageWithCamera()
        {
            _cameraImagePathCompletionSource = new TaskCompletionSource<string>();
            var intent = new Intent(MediaStore.ActionImageCapture);
            Platform.CurrentActivity.StartActivityForResult(intent, ImageRequest);
            return await _cameraImagePathCompletionSource.Task;
        }        
        public static void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            switch (requestCode)
            {
                case ImageRequest:
                    if (resultCode == Result.Ok)
                    {
                        if (data.Extras != null)
                        {
                            var photo = (Bitmap)data.Extras.Get("data");
                            var tempUri = GetImageUri(Platform.CurrentActivity, photo);
                            var filePathOfCameraImage = GetRealPathFromUri(tempUri);
                            _cameraImagePathCompletionSource.TrySetResult(filePathOfCameraImage);
                        }
                    }
                    else
                    {
                        _cameraImagePathCompletionSource.TrySetResult(null);
                    }
                    break;
            }
        }

        private static Android.Net.Uri GetImageUri(Context inContext, Bitmap inImage)
        {
            using (var stream = new MemoryStream())
            {
                inImage.Compress(Bitmap.CompressFormat.Png, 0, stream);
            }
            var path = MediaStore.Images.Media.InsertImage(inContext.ContentResolver, inImage, "Image", null);
            return Android.Net.Uri.Parse(path);
        }

        private static string GetRealPathFromUri(Android.Net.Uri uri)
        {
            var path = "";
            if (Platform.CurrentActivity.ContentResolver != null)
            {
                var cursor = Platform.CurrentActivity.ContentResolver.Query(uri, null, null, null, null);
                if (cursor != null)
                {
                    cursor.MoveToFirst();
                    int idx = cursor.GetColumnIndex(MediaStore.Images.ImageColumns.Data);
                    path = cursor.GetString(idx);
                    cursor.Close();
                }
            }
            return path;
        }
        
    }
}