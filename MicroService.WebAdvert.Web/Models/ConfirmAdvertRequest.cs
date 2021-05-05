using MicroService.Advert.Model;

namespace MicroService.WebAdvert.Web
{
    public class ConfirmAdvertRequest
    {
        public string Id { get; set; }
        public AdvertStatus Status { get; set; }
        public string FilePath { get; set; }
    }
}