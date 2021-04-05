using AutoMapper;

using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;
using JJS.Domain.Entities;
using JJS.Domain.Entities.CompanyTables;

using MediatR;

using System;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Company.Commands.CreateCompany
{
    public partial class CreateCompanyCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }        
        public string Description { get; set; }
        public DateTime? EstablishmentDate { get; set; }
        public string WebsiteURL { get; set; }

    }
    public class CreateCompanyCommandHandler : IRequestHandler<CreateCompanyCommand, Response<int>>
    {
        private readonly ICompanyRepositoryAsync _companyRepository;
        private readonly IMapper _mapper;
        public CreateCompanyCommandHandler(ICompanyRepositoryAsync companyRepository, IMapper mapper)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = _mapper.Map<Organization>(request);
            await _companyRepository.AddAsync(company);
            return new Response<int>(company.Id);
        }
    }
}
