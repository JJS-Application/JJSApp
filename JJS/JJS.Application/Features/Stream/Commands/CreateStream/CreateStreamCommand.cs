using AutoMapper;

using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;
using JJS.Domain.Entities;
using JJS.Domain.Entities.CompanyTables;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Stream.Commands.CreateStream
{
    public partial class CreateStreamCommand : IRequest<Response<int>>
    {
        public string Name { get; set; }
        public int? CompanyId { get; set; }

    }
    public class CreateStreamCommandHandler : IRequestHandler<CreateStreamCommand, Response<int>>
    {
        private readonly IStreamRepositoryAsync _streamRepository;
        private readonly IMapper _mapper;
        public CreateStreamCommandHandler(IStreamRepositoryAsync productRepository, IMapper mapper)
        {
            _streamRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateStreamCommand request, CancellationToken cancellationToken)
        {
            var stream = _mapper.Map<BusinessStream>(request);
            await _streamRepository.AddAsync(stream);
            return new Response<int>(stream.Id);
        }
    }
}
