using JJS.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JJS.Application.Interfaces.Repositories
{
    public interface IProductRepositoryAsync : IGenericRepositoryAsync<Product>
    {
        Task<bool> IsUniqueBarcodeAsync(string barcode);
    }
}
