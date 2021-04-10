using JJS.Application.DTOs.Company;
using JJS.Application.Exceptions;
using JJS.Application.FilterSorting;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Parameters;
using JJS.Application.StaticMessages;
using JJS.Application.ViewModels.Company;
using JJS.Application.Wrappers;
using JJS.Domain.Entities.CompanyTables;
using JJS.Infrastructure.Persistence.Contexts;
using JJS.Infrastructure.Persistence.Repository;

using Microsoft.EntityFrameworkCore;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JJS.Infrastructure.Persistence.Repositories
{
    public class CompanyRepositoryAsync : GenericRepositoryAsync<Organization>, ICompanyRepositoryAsync
    {
        private readonly DbSet<Organization> _company;

        public CompanyRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _company = dbContext.Set<Organization>();
        }

        public async Task<Response<string>> CreateCompanyAsync(OrgViewModel model)
        {
            var organization = new Organization
            {
                Description = model.Description,
                EstablishmentDate = model.EstablishmentDate,
                Name = model.Name,
                WebsiteURL = model.WebsiteURL
            };

            await AddAsyn(organization);

            return new Response<string>($"{organization.Id}", string.Format(Messages.CompanyMessages.Added, organization.Id));
        }

        public async Task<Response<string>> DeleteCompanyAsync(int id)
        {
            var org = await GetAsync(id);
            if (org == null)
                throw new ApiException(Messages.CompanyMessages.NotFound);
            await DeleteAsyn(org);
            return new Response<string>(Messages.CompanyMessages.NotFound);
        }

        /// <summary>
        ///   Get all companies data by filtering, searching and ordering
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<Response<List<OrgDto>>> GetAllByFilterAsync(PaginatedInputModel filter)
        {
            var res = await base.ApplyFilterOrderSortAsync(filter);
            var result = res.Select(x => new OrgDto
            {
                Id = x.Id,
                Description = x.Description,
                Name = x.Name,
                EstablishmentDate = x.EstablishmentDate,
                WebsiteURL = x.WebsiteURL
            }).ToList();
            return new Response<List<OrgDto>>(result, "", res.PageIndex, res.TotalPages, res.TotalItems, res.HasPreviousPage, res.HasNextPage);
        }

        public async Task<Response<OrgDto>> GetByIdAsync(int id)
        {
            var result = await GetAsync(id);
            if (result == null)
                throw new ApiException(Messages.CompanyMessages.NotFound);

            var obj = new OrgDto
            {
                Id = result.Id,
                Description = result.Description,
                Name = result.Name,
                EstablishmentDate = result.EstablishmentDate,
                WebsiteURL = result.WebsiteURL
            };
            return new Response<OrgDto>(obj);
        }

        public async Task<Response<string>> UpdateCompanyAsync(int id, OrgViewModel model)
        {
            var org = await GetAsync(id);
            if (org == null)
                throw new ApiException(Messages.CompanyMessages.NotFound);

            var organization = new Organization
            {
                Description = model.Description,
                EstablishmentDate = model.EstablishmentDate,
                Name = model.Name,
                WebsiteURL = model.WebsiteURL
            };

            await UpdateAsyn(organization, id);

            return new Response<string>($"{organization.Id}", string.Format(Messages.CompanyMessages.Update, id));

        }

    }
}
