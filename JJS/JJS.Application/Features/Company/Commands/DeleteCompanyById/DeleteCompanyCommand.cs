using JJS.Application.Exceptions;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;

using MediatR;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Company.Commands.DeleteCompanyById
{
    public class DeleteCompanyCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteCompanyByIdCommandHandler : IRequestHandler<DeleteCompanyCommand, Response<int>>
        {
            private readonly ICompanyRepositoryAsync _companyRepository;
            public DeleteCompanyByIdCommandHandler(ICompanyRepositoryAsync companyRepository)
            {
                _companyRepository = companyRepository;
            }
            public async Task<Response<int>> Handle(DeleteCompanyCommand command, CancellationToken cancellationToken)
            {
                var company = await _companyRepository.GetByIdAsync(command.Id);
                if (company == null) throw new ApiException($"Company Not Found.");
                await _companyRepository.DeleteAsync(company);
                return new Response<int>(company.Id);
            }
        }
    }
}
