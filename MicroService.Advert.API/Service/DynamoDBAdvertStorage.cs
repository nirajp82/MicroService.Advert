using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
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
        private readonly IAmazonDynamoDB _amazonDynamoDBClient;

        public DynamoDBAdvertStorage(IMapper mapper, IAmazonDynamoDB client)
        {
            _mapper = mapper;
            _amazonDynamoDBClient = client;
        }

        public async Task<string> Add(AdvertModel model)
        {
            AdvertDbModel dbModel = _mapper.Map<AdvertDbModel>(model);
            dbModel.Id = Guid.NewGuid().ToString();
            dbModel.CreatedDateTime = DateTime.UtcNow;
            dbModel.AdvertStatus = AdvertStatus.Pending;

            //using AmazonDynamoDBClient client = new();
            using DynamoDBContext context = new(_amazonDynamoDBClient);
            await context.SaveAsync(dbModel);

            return dbModel.Id;
        }

        public async Task<bool> CheckHealthAsync()
        {
            //using AmazonDynamoDBClient client = new();
            DescribeTableResponse tableData = await _amazonDynamoDBClient.DescribeTableAsync("Adverts");
            return string.Compare(tableData.Table.TableStatus, "active", true) == 0;
        }

        public async Task<bool> Confirm(ConfirmAdvertModel model)
        {
            //using AmazonDynamoDBClient client = new();
            using DynamoDBContext context = new(_amazonDynamoDBClient);
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
