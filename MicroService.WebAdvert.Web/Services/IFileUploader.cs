using System.IO;
using System.Threading.Tasks;

namespace MicroService.WebAdvert.Web
{
    public interface IFileUploader
    {
        Task<bool> UploadFileAsync(string fileName, Stream storageStream);
    }
}