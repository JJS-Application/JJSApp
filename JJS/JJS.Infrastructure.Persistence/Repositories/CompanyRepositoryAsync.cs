using JJS.Application.Interfaces.Repositories;
using JJS.Domain.Entities.CompanyTables;
using JJS.Infrastructure.Persistence.Contexts;
using JJS.Infrastructure.Persistence.Repository;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async override Task<Organization> GetByIdAsync(int id)
        {
            return await _company.Include(x => x.BusinessStream).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
