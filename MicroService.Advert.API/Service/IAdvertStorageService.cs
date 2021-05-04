using MicroService.Advert.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Advert.API
{
    public interface IAdvertStorageService
    {
        Task<string> Add(AdvertModel model);

        Task<bool> Confirm(ConfirmAdvertModel model);
    }
}
