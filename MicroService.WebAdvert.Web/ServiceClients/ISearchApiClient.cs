using System.Collections.Generic;
using System.Threading.Tasks;

namespace MicroService.WebAdvert.Web
{
    public interface ISearchApiClient
    {
        Task<List<AdvertType>> Search(string keyword);
    }
}