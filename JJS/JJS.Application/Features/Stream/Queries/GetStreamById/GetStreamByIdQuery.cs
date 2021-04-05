using JJS.Application.Exceptions;
using JJS.Application.Interfaces.Repositories;
using JJS.Application.Wrappers;
using JJS.Domain.Entities.CompanyTables;

using MediatR;

using System.Threading;
using System.Threading.Tasks;

namespace JJS.Application.Features.Stream.Queries.GetStreamById
{
    public class GetStreamByIdQuery : IRequest<Response<BusinessStream>>
    {
        public int Id { get; set; }
        public class GetStreamByIdQueryHandler : IRequestHandler<GetStreamByIdQuery, Response<BusinessStream>>
        {
            private readonly IStreamRepositoryAsync _streamRepository;
            public GetStreamByIdQueryHandler(IStreamRepositoryAsync streamRepository)
            {
                _streamRepository = streamRepository;
            }
            public async Task<Response<BusinessStream>> Handle(GetStreamByIdQuery query, CancellationToken cancellationToken)
            {
                var stream = await _streamRepository.GetByIdAsync(query.Id);
                if (stream == null) throw new ApiException($"Stream Not Found.");
                return new Response<BusinessStream>(stream);
            }
        }
    }
}
