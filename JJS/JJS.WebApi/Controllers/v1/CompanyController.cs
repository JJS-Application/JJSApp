using JJS.Application.Features.Company.Commands.CreateCompany;
using JJS.Application.Features.Company.Commands.DeleteCompanyById;
using JJS.Application.Features.Company.Commands.UpdateCompany;
using JJS.Application.Features.Company.Queries.GetAllCompany;
using JJS.Application.Features.Company.Queries.GetCompanyById;
using JJS.Application.Features.Products.Queries.GetAllProducts;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJS.WebApi.Controllers.v1
{

    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Authorize]
    public class CompanyController : BaseApiController
    {
        // GET: api/<controller>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetAllCompanyParameter filter)
        {

            return Ok(await Mediator.Send(new GetAllCompanyQuery() { PageSize = filter.PageSize, PageNumber = filter.PageNumber }));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetStreamByIdQuery { Id = id }));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post(CreateCompanyCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, UpdateCompanyCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await Mediator.Send(new DeleteCompanyCommand { Id = id }));
        }
    }
}
