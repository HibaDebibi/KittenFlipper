using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KittenFlipper.Contracts;
using KittenFlipper.Infrastructure.BasicAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KittenFlipper.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ImageFlipperController : ControllerBase
    {
        private readonly IKittenFlipperService _kittenFlipperService;

        public ImageFlipperController(IKittenFlipperService kittenFlipperService)
        {
            this._kittenFlipperService = kittenFlipperService;
        }

        /// <summary>
        /// Get the image and flip it : basic authentication
        /// </summary>
        /// <param name="rotationType"></param>
        /// <returns></returns>
        /// <example>1 for flip</example>
        [HttpGet]
        [BasicAuth]
        public async Task<IActionResult> GetAsync(int rotationType = 1)
        {
            byte[] imageBytes;
            if (rotationType <1 || rotationType > 16)
            {
                return Content("Please enter rotation type between 1 and 16");
            }
            else
            {
                imageBytes = await _kittenFlipperService.RotateCatAsync(rotationType);
                return File(imageBytes, "image/jpeg");
            }
           
            return File(imageBytes, "image/jpeg");
        }

        /// <summary>
        /// Get the image and flip : it requires JWT auth
        /// </summary>
        /// <returns></returns>
        [HttpGet("jwt")]
        [Authorize]
        public async Task<IActionResult> GetJwtAsync(int rotationType = 1)
        {
            byte[] imageBytes;
            if (rotationType < 1 || rotationType > 16)
            {
                return Content("Please enter rotation type between 1 and 16");
            }
            else
            {
                imageBytes = await _kittenFlipperService.RotateCatAsync(rotationType);
                return File(imageBytes, "image/jpeg");
            }

            return File(imageBytes, "image/jpeg");
        }
    }
}
