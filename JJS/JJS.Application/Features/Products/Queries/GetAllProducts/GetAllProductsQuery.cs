using AutoMapper;

using JJS.Application.Filters;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;

using MediatR;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Products.Queries.GetAllProducts
{
    public class GetAllProductQuery : IRequest<PagedResponse<IEnumerable<GetAllProductViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductQuery, PagedResponse<IEnumerable<GetAllProductViewModel>>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public GetAllProductsQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllProductViewModel>>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllProductParameter>(request);
            var product = await _productRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var productViewModel = _mapper.Map<IEnumerable<GetAllProductViewModel>>(product);
            return new PagedResponse<IEnumerable<GetAllProductViewModel>>(productViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
