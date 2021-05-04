using AutoMapper;
using MicroService.Advert.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Advert.API.Service
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile()
        {
            CreateMap<AdvertModel, AdvertDbModel>();
        }
    }
}
