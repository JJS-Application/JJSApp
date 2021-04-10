
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Parameters;
using JJS.Application.ViewModels.Company;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace JJS.WebApi.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize]
    public class StreamController : BaseApiController
    {
        private readonly IStreamRepositoryAsync _streamRepository;
        public StreamController(IStreamRepositoryAsync streamRepository)
        {
            _streamRepository = streamRepository;
        }

        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PaginatedInputModel filter)
        {
            var result = await _streamRepository.GetAllByFilterAsync(filter);
            return Ok(result);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _streamRepository.GetByIdAsync(id));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(StreamViewModel command)
        {
            return Ok(await _streamRepository.CreateBusinessStreamAsync(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, StreamViewModel command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await _streamRepository.UpdateBusinessStreamAsync(id, command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _streamRepository.DeleteBusinessStreamAsync(id));
        }
    }
}
