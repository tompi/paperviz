using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using paperviz.Text;
using Xamarin.Essentials;

namespace paperviz.Export.Text
{
    [UsedImplicitly]
    public class TextExportService : ExportServiceBase
    {
        public override Task Export(ScanResult scanResult)
        {
            var fileName = Path.Combine(_fileFolder, GetFileName(scanResult, "txt"));

            Block previousBlock = null;
            using (var streamWriter = new StreamWriter(fileName))
            {
                var orderedBlocks = scanResult.Blocks
                    .OrderBy(b => b.BoundingBox.Top)
                    .ThenBy(b => b.BoundingBox.Left);
                foreach (var block in orderedBlocks)
                {
                    if (previousBlock != null)
                    {
                        // First find out if this is a new line
                        var middleOfPreviousLine =
                            (previousBlock.BoundingBox.Bottom - previousBlock.BoundingBox.Top) / 2d;
                        if (block.BoundingBox.Top > middleOfPreviousLine)
                        {
                            streamWriter.Write(streamWriter.NewLine);
                        }
                    }
                    streamWriter.Write(block.Text);

                    previousBlock = block;
                }
            }
            return Share.RequestAsync(new ShareFileRequest
            {
                Title = "Export as text",
                File = new ShareFile(fileName)
            });                
        }
    }
}