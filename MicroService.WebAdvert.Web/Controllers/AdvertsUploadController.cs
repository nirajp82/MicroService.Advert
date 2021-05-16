using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using MicroService.Advert.Model;
using MicroService.WebAdvert.Web;
//using MicroService.WebAdvert.Web.ServiceClients;
//using MicroService.WebAdvert.Web.Services;
//using AdvertApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroService.WebAdvert.Web
{
    public class AdvertsUploadController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _advertApiClient;
        private readonly IMapper _mapper;

        public AdvertsUploadController(IFileUploader fileUploader, IMapper mapper, IAdvertApiClient advertApiClient)
        {
            _fileUploader = fileUploader;
            _advertApiClient = advertApiClient;
            _mapper = mapper;
        }

        [Authorize]
        public IActionResult Create(CreateAdvertViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                CreateAdvertModel createAdvertModel = _mapper.Map<CreateAdvertModel>(model);
                AdvertResponse apiCallResponse = await _advertApiClient.CreateAsync(createAdvertModel);
                string id = apiCallResponse.Id;
                if (imageFile != null)
                {
                    string fileName = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using var readStream = imageFile.OpenReadStream();
                        var result = await _fileUploader.UploadFileAsync(filePath, readStream);
                        if (!result)
                            throw new Exception("Could not upload image to the file repository. Please see logs for more detail");

                        ConfirmAdvertRequest confirmModel = new ConfirmAdvertRequest
                        {
                            Id = id,
                            FilePath = filePath,
                            Status = AdvertStatus.Active
                        };
                        bool canConfirm = await _advertApiClient.ConfirmAsync(confirmModel);
                        if (!canConfirm)
                            throw new Exception($"Cannot confirm upload for advert {id}");
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        ConfirmAdvertRequest confirmModel = new ConfirmAdvertRequest
                        {
                            Id = id,
                            FilePath = filePath,
                            Status = AdvertStatus.Pending
                        };
                        await _advertApiClient.ConfirmAsync(confirmModel);
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return View(model);
        }
    }
}