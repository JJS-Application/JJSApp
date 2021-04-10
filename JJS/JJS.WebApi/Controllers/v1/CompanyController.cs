using JJS.Application.Interfaces.Repositories;
using JJS.Application.Parameters;
using JJS.Application.ViewModels.Company;
using JJS.Application.ViewModels.RequestFilter;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Linq;
using System.Threading.Tasks;

namespace JJS.WebApi.Controllers.v1
{

    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class CompanyController : BaseApiController
    {
        private readonly ICompanyRepositoryAsync _companyRepository;
        public CompanyController(ICompanyRepositoryAsync companyRepository)
        {
            _companyRepository = companyRepository;
        }

        // GET: api/<controller>
        [HttpPost("GetAll")]
        public async Task<IActionResult> Get([FromBody] PaginatedInputModel filter)
        {
            var result = await _companyRepository.GetAllByFilterAsync(filter);             
            return Ok(result);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _companyRepository.GetByIdAsync(id));
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<IActionResult> Post(OrgViewModel command)
        {
            return Ok(await _companyRepository.CreateCompanyAsync(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, OrgViewModel command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await _companyRepository.UpdateCompanyAsync(id, command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _companyRepository.DeleteCompanyAsync(id));
        }
    }
}
