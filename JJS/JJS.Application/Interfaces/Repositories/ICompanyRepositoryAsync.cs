using JJS.Application.DTOs.Company;
using JJS.Application.Filters;
using JJS.Application.FilterSorting;
using JJS.Application.Parameters;
using JJS.Application.ViewModels.Company;
using JJS.Application.Wrappers;
using JJS.Domain.Entities;
using JJS.Domain.Entities.CompanyTables;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace JJS.Application.Interfaces.Repositories
{
    public interface ICompanyRepositoryAsync : IGenericRepositoryAsync<Organization>
    {
        Task<Response<List<OrgDto>>> GetAllByFilterAsync(PaginatedInputModel filter);
        Task<Response<OrgDto>> GetByIdAsync(int id);

        Task<Response<string>> CreateCompanyAsync(OrgViewModel model);
        Task<Response<string>> UpdateCompanyAsync(int id, OrgViewModel model);

        Task<Response<string>> DeleteCompanyAsync(int id);
    }
}
