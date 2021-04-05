using JJS.Application.Exceptions;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;

using MediatR;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Stream.Commands.DeleteStreamById
{
    public class DeleteStreamCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public class DeleteStreamByIdCommandHandler : IRequestHandler<DeleteStreamCommand, Response<int>>
        {
            private readonly IStreamRepositoryAsync _streamRepository;
            public DeleteStreamByIdCommandHandler(IStreamRepositoryAsync streamRepository)
            {
                _streamRepository = streamRepository;
            }
            public async Task<Response<int>> Handle(DeleteStreamCommand command, CancellationToken cancellationToken)
            {
                var Stream = await _streamRepository.GetByIdAsync(command.Id);
                if (Stream == null) throw new ApiException($"Stream Not Found.");
                await _streamRepository.DeleteAsync(Stream);
                return new Response<int>(Stream.Id);
            }
        }
    }
}
