using JJS.Application.Exceptions;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;

using MediatR;

using System.Threading;
using System.Threading.Tasks;
using JJS.Domain.Entities.CompanyTables;

namespace JJS.Application.Features.Company.Queries.GetCompanyById
{
    public class GetStreamByIdQuery : IRequest<Response<Organization>>
    {
        public int Id { get; set; }
        public class GetCompanyByIdQueryHandler : IRequestHandler<GetStreamByIdQuery, Response<Organization>>
        {
            private readonly ICompanyRepositoryAsync _companyRepository;
            public GetCompanyByIdQueryHandler(ICompanyRepositoryAsync CompanyRepository)
            {
                _companyRepository = CompanyRepository;
            }
            public async Task<Response<Organization>> Handle(GetStreamByIdQuery query, CancellationToken cancellationToken)
            {
                var company = await _companyRepository.GetByIdAsync(query.Id);
                if (company == null) throw new ApiException($"Company Not Found.");
                return new Response<Organization>(company);
            }
        }
    }
}
