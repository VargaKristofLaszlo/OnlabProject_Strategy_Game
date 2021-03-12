using AutoMapper;
using BackEnd.Repositories.Interfaces;
using MediatR;
using Services.Extensions;
using Shared.Models;
using Shared.Models.Response;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetUserCredentials
    {
        public record Query(int PageNumber = 1, int PageSize = 10) : IRequest<CollectionResponse<Credentials>>;

        public class Handler : IRequestHandler<Query, CollectionResponse<Credentials>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<CollectionResponse<Credentials>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Validation
                int pageNumber = request.PageNumber.ValidatePageNumber();
                int pageSize = request.PageSize.ValideatePageSize();

                var (Users, Count) = await _unitOfWork.Users.GetAllUsersAsync(pageNumber, pageSize);

                var credentialListForPage = Users
                        .Select(model => _mapper.Map<Credentials>(model));

                return new CollectionResponse<Credentials>
                {
                    Records = credentialListForPage,
                    PagingInformations = new PagingInformations
                    {
                        PageNumber = pageNumber,
                        PageSize =  pageSize,
                        PagesCount = Paging.CalculatePageCount(Count, pageSize)
                    }
                };
            }
        }
    }
}
