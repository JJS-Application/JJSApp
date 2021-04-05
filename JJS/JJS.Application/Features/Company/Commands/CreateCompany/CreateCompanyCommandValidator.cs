using FluentValidation;

using JJS.Application.Interfaces.Repositories;
using JJS.Domain.Entities;

using Microsoft.EntityFrameworkCore.Internal;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Company.Commands.CreateCompany
{
    public class CreateCompanyCommandValidator : AbstractValidator<CreateCompanyCommand>
    {
        private readonly ICompanyRepositoryAsync companyRepository;

        public CreateCompanyCommandValidator(ICompanyRepositoryAsync companyRepository)
        {
            this.companyRepository = companyRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(255).WithMessage("{PropertyName} must not exceed 255 characters.")
                .MustAsync(IsUniqueName).WithMessage("{PropertyName} already exists.");

        }

        private async Task<bool> IsUniqueName(string cName, CancellationToken cancellationToken)
        {
            var comapny = await companyRepository.FindByCondition(x => x.Name==cName);
            return comapny == null ? false : true;
        }    
    }
}
                        