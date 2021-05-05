using MicroService.Advert.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Advert.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly IAdvertStorageService _advertStorageService;

        public AdvertController(IAdvertStorageService advertStorageService)
        {
            _advertStorageService = advertStorageService;
        }

        [HttpGet]
        [Route("Healthcheck")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> Healthcheck()
        {
            try
            {
                string status = await _advertStorageService.CheckHealthAsync() ? "Healthy": "Unhealthy";
                return Ok(status);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(CreateAdvertResponse), 200)]
        public async Task<IActionResult> Create(AdvertModel model)
        {
            string recordId;
            try
            {
                recordId = await _advertStorageService.Add(model);
            }            
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return StatusCode(201, new CreateAdvertResponse { Id = recordId });
        }

        [HttpPut]
        [Route("Confirm")]
        [ProducesResponseType(400)]
        [ProducesResponseType( 200)]
        public async Task<IActionResult> Confirm(ConfirmAdvertModel model)
        {
            try
            {
                 await _advertStorageService.Confirm(model);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
            return Ok();
        }
    }
}
