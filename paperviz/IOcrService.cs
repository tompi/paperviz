using System.IO;
using System.Threading.Tasks;

namespace paperviz
{
    public interface IOcrService
    {
        Task<string> GetTexts(Stream bitmapStream);
    }
}