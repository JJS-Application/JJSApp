using JJS.Application.Interfaces.Repositories;
using JJS.Domain.Entities.CompanyTables;
using JJS.Infrastructure.Persistence.Contexts;
using JJS.Infrastructure.Persistence.Repository;

using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> IsUniqueName(string sName, CancellationToken cancellationToken)
        {
            return await _stream.AllAsync(p => p.Name.ToLower() == sName.ToLower());
        }
    }
}
