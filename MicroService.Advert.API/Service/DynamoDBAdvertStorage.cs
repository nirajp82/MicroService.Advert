using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using AutoMapper;
using MicroService.Advert.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Advert.API
{
    public class DynamoDBAdvertStorage : IAdvertStorageService
    {
        private readonly IMapper _mapper;

        public DynamoDBAdvertStorage(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<string> Add(AdvertModel model)
        {
            AdvertDbModel dbModel = _mapper.Map<AdvertDbModel>(model);
            dbModel.Id = new Guid().ToString();
            dbModel.CreatedDateTime = DateTime.UtcNow;
            dbModel.AdvertStatus = AdvertStatus.Pending;

            using AmazonDynamoDBClient client = new();
            using DynamoDBContext context = new(client);
            await context.SaveAsync(dbModel);

            return dbModel.Id;
        }


        public async Task<bool> Confirm(ConfirmAdvertModel model)
        {
            using AmazonDynamoDBClient client = new();
            using DynamoDBContext context = new(client);
            AdvertDbModel dbModel = await context.LoadAsync<AdvertDbModel>(model.Id);
            if (dbModel == null)
                throw new KeyNotFoundException($"A Record with id={model.Id} was not found");
            if (model.Status == AdvertStatus.Active)
            {
                dbModel.AdvertStatus = model.Status;
                await context.SaveAsync(dbModel);
                return true;
            }
            else
            {
                await context.DeleteAsync(dbModel);
                return false;
            }
        }
    }
}
