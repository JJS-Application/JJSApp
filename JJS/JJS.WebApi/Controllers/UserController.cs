using JJS.Application.Extensions;
using JJS.Application.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JJS.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IAccountService _userService;
        public UserController(IAccountService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}/get-user")]
        public async Task<IActionResult> GetUserByIdAsync(string id)
        {
            return Ok(await _userService.GetById(id));
        }

        [HttpGet("{id}/get-all-user")]
        public async Task<IActionResult> GetUsersAsync()
        {
            return Ok(await _userService.GetAll());
        }

        [HttpPost("upload-file/{id}")]
        public async Task<IActionResult> UploadFileAsync(IFormFile file, [FromRoute] string id)
        {
            byte[] bytes = await file.GetBytesAsync();
            return Ok(await _userService.UploadFileAsync(bytes,id));
        }

        [HttpGet("download-file/{id}")]
        public async Task<IActionResult> DownloadAsync([FromRoute] string id)
        {
            var response = await _userService.DownloadAsync(id);
            return Ok(response);
        }
    }
}
