using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MicroService.Advert.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MicroService.WebAdvert.Web
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly HttpClient _client;
        private readonly IMapper _mapper;
        private readonly string _baseAddress;
        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _client = client;
            _mapper = mapper;

            string baseUriUse = configuration.GetValue<string>("AdvertApi:Uri:Use");
            _baseAddress = configuration.GetValue<string>($"AdvertApi:Uri:{baseUriUse}");
            
        }
        public async Task<AdvertResponse> CreateAsync(CreateAdvertModel model)
        {
            AdvertModel advertApiModel = _mapper.Map<AdvertModel>(model);
            string jsonModel = JsonConvert.SerializeObject(advertApiModel);
            HttpResponseMessage response = await _client.PostAsync(new Uri($"{_baseAddress}/create"),
                             new StringContent(jsonModel, Encoding.UTF8, "application/json"));
            string responseJson = await response.Content.ReadAsStringAsync();
            CreateAdvertResponse createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responseJson);
            AdvertResponse advertResponse = _mapper.Map<AdvertResponse>(createAdvertResponse);
            return advertResponse;
        }

        public async Task<bool> ConfirmAsync(ConfirmAdvertRequest model)
        {
            ConfirmAdvertModel confirmApiModel = _mapper.Map<ConfirmAdvertModel>(model);
            string jsonModel = JsonConvert.SerializeObject(confirmApiModel);
            HttpResponseMessage response = await _client.PutAsync(new Uri($"{_baseAddress}/confirm"),
                                new StringContent(jsonModel, Encoding.UTF8, "application/json"));
            return response.StatusCode == HttpStatusCode.OK;
        }
        
        public async Task<List<Advertisement>> GetAllAsync()
        {
            var apiCallResponse = await _client.GetAsync(new Uri($"{_baseAddress}/all"));
            var responseJson = await apiCallResponse.Content.ReadAsStringAsync();
            var allAdvertModels = JsonConvert.DeserializeObject<List<AdvertModel>>(responseJson);
            return allAdvertModels.Select(x => _mapper.Map<Advertisement>(x)).ToList();
        }

        public async Task<Advertisement> GetAsync(string advertId)
        {
            var apiCallResponse = await _client.GetAsync(new Uri($"{_baseAddress}/{advertId}"));
            var responseJson = await apiCallResponse.Content.ReadAsStringAsync();
            var fullAdvert = JsonConvert.DeserializeObject<AdvertModel>(responseJson);
            return _mapper.Map<Advertisement>(fullAdvert);
        }
    }
}