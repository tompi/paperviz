using System.IO;
using System.Text;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Exception = System.Exception;
using Frame = Android.Gms.Vision.Frame;

[assembly: Dependency(typeof(paperviz.Droid.OcrService))]
namespace paperviz.Droid
{
    public class OcrService : IOcrService
    {
        private TextRecognizer _recognizer;

        public OcrService()
        {
            _recognizer = new TextRecognizer
                .Builder(Platform.CurrentActivity)
                .Build();
        }

        public string GetTexts(Stream bitmapStream)
        {
            if (!_recognizer.IsOperational)
            {
                throw new Exception("Recognizer not operational.");
            }

            var bitmap = BitmapFactory.DecodeStream(bitmapStream);
            var frame = new Frame.Builder().SetBitmap(bitmap).Build();
            var results = _recognizer.Detect(frame);
            var sb = new StringBuilder();
            for (var i = 0; i < results.Size(); i++)
            {
                sb.AppendLine(((TextBlock) results.ValueAt(i))?.Value);
            }

            return sb.ToString();
        }
    }
}