using JJS.Application.DTOs.Company;
using JJS.Application.Parameters;
using JJS.Application.ViewModels.Company;
using JJS.Application.Wrappers;
using JJS.Domain.Entities.CompanyTables;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Interfaces.Repositories
{
    public interface IStreamRepositoryAsync : IGenericRepositoryAsync<BusinessStream>
    {
        Task<Response<List<StreamDto>>> GetAllByFilterAsync(PaginatedInputModel filter);
        Task<Response<StreamDto>> GetByIdAsync(int id);
        Task<Response<string>> CreateBusinessStreamAsync(StreamViewModel model);
        Task<Response<string>> UpdateBusinessStreamAsync(int id, StreamViewModel model);
        Task<Response<string>> DeleteBusinessStreamAsync(int id);
    }
}
