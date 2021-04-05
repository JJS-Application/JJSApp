using JJS.Application.Exceptions;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;

using MediatR;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Company.Commands.UpdateCompany
{
    public class UpdateCompanyCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Rate { get; set; }
        public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, Response<int>>
        {
            private readonly ICompanyRepositoryAsync _companyRepository;
            public UpdateCompanyCommandHandler(ICompanyRepositoryAsync companyRepository)
            {
                _companyRepository = companyRepository;
            }
            public async Task<Response<int>> Handle(UpdateCompanyCommand command, CancellationToken cancellationToken)
            {
                var company = await _companyRepository.GetByIdAsync(command.Id);

                if (company == null)
                {
                    throw new ApiException($"Company Not Found.");
                }
                else
                {
                    //todo
                    await _companyRepository.UpdateAsync(company);
                    return new Response<int>(company.Id);
                }
            }
        }
    }
}
