using System.IO;

namespace paperviz
{
    public interface IOcrService
    {
        string GetTexts(Stream bitmapStream);
    }
}