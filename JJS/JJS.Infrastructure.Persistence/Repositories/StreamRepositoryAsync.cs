using JJS.Application.DTOs.Company;
using JJS.Application.Exceptions;
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
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Infrastructure.Persistence.Repositories
{
    public class StreamRepositoryAsync : GenericRepositoryAsync<BusinessStream>, IStreamRepositoryAsync
    {
        private readonly DbSet<BusinessStream> _stream;

        public StreamRepositoryAsync(ApplicationDbContext dbContext) : base(dbContext)
        {
            _stream = dbContext.Set<BusinessStream>();
        }


        public async Task<Response<string>> CreateBusinessStreamAsync(StreamViewModel model)
        {
            var businessStream = new BusinessStream
            {
                Name = model.Name
            };

            await AddAsyn(businessStream);

            return new Response<string>($"{businessStream.Id}", string.Format(Messages.BusinessStreamMessages.Added, businessStream.Id));
        }

        public async Task<Response<string>> DeleteBusinessStreamAsync(int id)
        {
            var org = await GetAsync(id);
            if (org == null)
                throw new ApiException(Messages.BusinessStreamMessages.NotFound);
            await DeleteAsyn(org);
            return new Response<string>(Messages.BusinessStreamMessages.NotFound);
        }

        /// <summary>
        ///   Get all companies data by filtering, searching and ordering
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<Response<List<StreamDto>>> GetAllByFilterAsync(PaginatedInputModel filter)
        {
            var res = await base.ApplyFilterOrderSortAsync(filter);
            var result = res.Select(x => new StreamDto
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return new Response<List<StreamDto>>(result, "", res.PageIndex, res.TotalPages, res.TotalItems, res.HasPreviousPage, res.HasNextPage);

           
        }

        public async Task<Response<StreamDto>> GetByIdAsync(int id)
        {
            var result = await GetAsync(id);
            if (result == null)
                throw new ApiException(Messages.BusinessStreamMessages.NotFound);

            var obj = new StreamDto
            {
                Id = result.Id,
                Name = result.Name
            };
            return new Response<StreamDto>(obj);
        }

        public async Task<Response<string>> UpdateBusinessStreamAsync(int id, StreamViewModel model)
        {
            var org = await GetAsync(id);
            if (org == null)
                throw new ApiException(Messages.BusinessStreamMessages.NotFound);

            var businessStream = new BusinessStream
            {
                Name = model.Name
            };

            await UpdateAsyn(businessStream, id);

            return new Response<string>($"{businessStream.Id}", string.Format(Messages.BusinessStreamMessages.Update, id));

        }

    }
}
