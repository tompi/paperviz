using System.IO;
using System.Threading.Tasks;
using paperviz.Text;

namespace paperviz
{
    public interface IOcrService
    {
        Task<ScanResult> ProcessImage(Stream bitmapStream);
    }
}