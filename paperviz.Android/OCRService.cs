using System.IO;
using System.Threading.Tasks;
using Android.Gms.Vision.Texts;
using Android.Graphics;
using paperviz.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Exception = System.Exception;
using Frame = Android.Gms.Vision.Frame;

[assembly: Dependency(typeof(paperviz.Droid.OcrService))]
namespace paperviz.Droid
{
    public class OcrService : IOcrService
    {
        private readonly TextRecognizer _recognizer;

        public OcrService()
        {
            _recognizer = new TextRecognizer
                .Builder(Platform.CurrentActivity)
                .Build();
        }

        public Task<ScanResult> ProcessImage(Stream bitmapStream)
        {
            if (!_recognizer.IsOperational)
            {
                throw new Exception("Recognizer not operational.");
            }

            using var bitmap = BitmapFactory.DecodeStream(bitmapStream);
            using var frame = new Frame.Builder().SetBitmap(bitmap).Build();
            using var results = _recognizer.Detect(frame);
            var result = new ScanResult();
            for (var i = 0; i < results.Size(); i++)
            {
                var androidBlock = (TextBlock) results.ValueAt(i);
                if (androidBlock != null)
                {
                    result.Blocks.Add(GetBlock(androidBlock));
                }
            }

            return Task.FromResult(result);
        }

        private Block GetBlock(TextBlock androidBlock)
        {
            var block = new Block
            {
                Text = androidBlock.Value,
                BoundingBox = new BoundingBox
                {
                    Left = androidBlock.BoundingBox.Left,
                    Top = androidBlock.BoundingBox.Top,
                    Right = androidBlock.BoundingBox.Right,
                    Bottom = androidBlock.BoundingBox.Bottom
                }
            };
            return block;
        }
    }
}