using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foundation;
using ImageIO;
using Vision;
using Xamarin.Forms;

[assembly: Dependency(typeof(paperviz.iOS.OcrService))]
namespace paperviz.iOS
{
    public class OcrService : IOcrService
    {
        public Task<string> GetTexts(Stream bitmapStream)
        {
            var imageData = NSData.FromStream(bitmapStream);
            //var image = UIImage.LoadFromData(imageData);
            //image.
            var requestHandler =
                new VNImageRequestHandler(imageData, CGImagePropertyOrientation.Down, new NSDictionary());

            var completionSource = new TaskCompletionSource<string>();

            var request = new VNRecognizeTextRequest((vnRequest, error) =>
            {
                var results = vnRequest.GetResults<VNRecognizedTextObservation>();
                var sb = new StringBuilder();
                foreach (var textObservation in results)
                {
                    var candidate = textObservation.TopCandidates(1).FirstOrDefault();
                    if (candidate != null)
                    {
                        sb.AppendLine(candidate.String);
                    }
                }

                completionSource.TrySetResult(sb.ToString());
            });

            requestHandler.Perform(new VNRequest[]{request}, out var nsError);
            if (!string.IsNullOrEmpty(nsError?.Description))
            {
                throw new Exception(nsError.Description);
            }

            return completionSource.Task;
        }
    }
}