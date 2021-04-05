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

namespace JJS.Application.Features.Company.Queries.GetAllCompany
{
    public class GetAllCompanyQuery : IRequest<PagedResponse<IEnumerable<GetAllCompanyViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllCompanyQueryHandler : IRequestHandler<GetAllCompanyQuery, PagedResponse<IEnumerable<GetAllCompanyViewModel>>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public GetAllCompanyQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllCompanyViewModel>>> Handle(GetAllCompanyQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllCompanyParameter>(request);
            var product = await _productRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var productViewModel = _mapper.Map<IEnumerable<GetAllCompanyViewModel>>(product);
            return new PagedResponse<IEnumerable<GetAllCompanyViewModel>>(productViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
