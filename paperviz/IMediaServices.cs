using System.Threading.Tasks;

namespace paperviz
{
    public interface IMediaServices
    {
       Task<string> GetImageWithCamera();
    }
}