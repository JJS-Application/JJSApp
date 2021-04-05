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
        Task<bool> IsUniqueName(string cName, CancellationToken cancellationToken);
    }
}
