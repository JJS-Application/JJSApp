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

namespace JJS.Application.Features.Stream.Commands.CreateStream
{
    public class CreateStreamCommandValidator : AbstractValidator<CreateStreamCommand>
    {
        private readonly IStreamRepositoryAsync _streamRepository;

        public CreateStreamCommandValidator(IStreamRepositoryAsync streamRepository)
        {
            this._streamRepository = streamRepository;

            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MaximumLength(255).WithMessage("{PropertyName} must not exceed 255 characters.")
                .MustAsync(IsUniqueName).WithMessage("{PropertyName} already exists.");

        }

        private async Task<bool> IsUniqueName(string cName, CancellationToken cancellationToken)
        {
            return await _streamRepository.IsUniqueName(cName, cancellationToken);
        }    
    }
}
                        