using MicroService.Advert.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Advert.API.Service
{
    public class MockAdvertStorage : IAdvertStorageService
    {
        public Task<string> Add(AdvertModel model)
        {
            return Task.FromResult(Guid.NewGuid().ToString());
        }

        public Task<bool> CheckHealthAsync()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Confirm(ConfirmAdvertModel model)
        {
            return Task.FromResult(true);
        }
    }
}
