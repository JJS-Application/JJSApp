using JJS.Application.Exceptions;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;

using MediatR;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Stream.Commands.UpdateStream
{
    public class UpdateStreamCommand : IRequest<Response<int>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public class UpdateStreamCommandHandler : IRequestHandler<UpdateStreamCommand, Response<int>>
        {
            private readonly IStreamRepositoryAsync _streamRepository;
            public UpdateStreamCommandHandler(IStreamRepositoryAsync streamRepository)
            {
                _streamRepository = streamRepository;
            }
            public async Task<Response<int>> Handle(UpdateStreamCommand command, CancellationToken cancellationToken)
            {
                var stream = await _streamRepository.GetByIdAsync(command.Id);

                if (stream == null)
                {
                    throw new ApiException($"Stream Not Found.");
                }
                else
                {
                    stream.Name = command.Name;
                    await _streamRepository.UpdateAsync(stream);
                    return new Response<int>(stream.Id);
                }
            }
        }
    }
}
