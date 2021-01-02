using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ImageIO;
using paperviz.Text;
using Vision;
using Xamarin.Forms;

[assembly: Dependency(typeof(paperviz.iOS.OcrService))]
namespace paperviz.iOS
{
    public class OcrService : IOcrService
    {
        public Task<ScanResult> ProcessImage(Stream bitmapStream)
        {
            var imageData = NSData.FromStream(bitmapStream);
            // TODO: Find a way to make orientation foolproof
            // (Probably convert stream to UIImage which has orientation encoded...)
            var requestHandler =
                new VNImageRequestHandler(imageData, CGImagePropertyOrientation.Up, new NSDictionary());
            
            var completionSource = new TaskCompletionSource<ScanResult>();

            var request = new VNRecognizeTextRequest((vnRequest, error) =>
            {
                var results = vnRequest.GetResults<VNRecognizedTextObservation>();

                var scanResult = new ScanResult();
                foreach (var textObservation in results)
                {
                    var candidate = textObservation.TopCandidates(1).FirstOrDefault();
                    
                    if (candidate != null)
                    {
                        scanResult.Add(GetBlock(candidate, textObservation));
                    }
                }
                completionSource.TrySetResult(scanResult);
            });

            requestHandler.Perform(new VNRequest[]{request}, out var nsError);
            // ReSharper disable once ConstantConditionalAccessQualifier
            if (!string.IsNullOrEmpty(nsError?.Description))
            {
                throw new Exception(nsError.Description);
            }

            return completionSource.Task;
        }

        private Block GetBlock(VNRecognizedText vnRecognizedText,
            VNRecognizedTextObservation vnRecognizedTextObservation)
        {
            var block = new Block
            {
                Text = $"{vnRecognizedText.String} ({vnRecognizedTextObservation.Confidence})",
                BoundingBox = new BoundingBox
                {
                    Left = (int) vnRecognizedTextObservation.TopLeft.X,
                    Top = (int) vnRecognizedTextObservation.TopLeft.Y,
                    Right = (int) vnRecognizedTextObservation.BottomRight.X,
                    Bottom = (int) vnRecognizedTextObservation.BottomRight.Y,
                }
            };
            return block;
        }
    }
}