using AutoMapper;

using JJS.Application.Features.Company.Queries.GetAllCompany;
using JJS.Application.Filters;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;

using MediatR;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Stream.Queries.GetAllStream
{
    public class GetAllStreamQuery : IRequest<PagedResponse<IEnumerable<GetAllStreamViewModel>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
    public class GetAllStreamQueryHandler : IRequestHandler<GetAllStreamQuery, PagedResponse<IEnumerable<GetAllStreamViewModel>>>
    {
        private readonly IProductRepositoryAsync _productRepository;
        private readonly IMapper _mapper;
        public GetAllStreamQueryHandler(IProductRepositoryAsync productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<PagedResponse<IEnumerable<GetAllStreamViewModel>>> Handle(GetAllStreamQuery request, CancellationToken cancellationToken)
        {
            var validFilter = _mapper.Map<GetAllStreamParameter>(request);
            var product = await _productRepository.GetPagedReponseAsync(validFilter.PageNumber, validFilter.PageSize);
            var productViewModel = _mapper.Map<IEnumerable<GetAllStreamViewModel>>(product);
            return new PagedResponse<IEnumerable<GetAllStreamViewModel>>(productViewModel, validFilter.PageNumber, validFilter.PageSize);
        }
    }
}
