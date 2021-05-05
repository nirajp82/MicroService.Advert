using AutoMapper;
using MicroService.Advert.Model;

namespace MicroService.WebAdvert.Web
{
    public class AdvertApiProfile : Profile
    {
        public AdvertApiProfile()
        {
            CreateMap<AdvertModel, Advertisement>().ReverseMap();
          
            CreateMap<Advertisement, IndexViewModel>()
                .ForMember(dest => dest.Title, src => src.MapFrom(field => field.Title))
                .ForMember(dest => dest.Image, src => src.MapFrom(field => field.FilePath));
           
            CreateMap<AdvertType, SearchViewModel>()
                .ForMember(dest => dest.Title, src => src.MapFrom(field => field.Title))
                .ForMember(dest => dest.Id, src => src.MapFrom(field => field.Id));
            
            CreateMap<AdvertModel, CreateAdvertModel>().ReverseMap();
            CreateMap<CreateAdvertResponse, AdvertResponse>().ReverseMap();
            CreateMap<ConfirmAdvertRequest, ConfirmAdvertModel>().ReverseMap();
            CreateMap<CreateAdvertViewModel, CreateAdvertModel>().ReverseMap();
        }
    }
}