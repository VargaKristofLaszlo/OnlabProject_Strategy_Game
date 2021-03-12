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
    public static class GetUnitTypes
    {
        public record Query(int PageNumber, int PageSize) : IRequest<CollectionResponse<Game.Shared.Models.Unit>>;

        public class Handler : IRequestHandler<Query, CollectionResponse<Game.Shared.Models.Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<CollectionResponse<Game.Shared.Models.Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Validation
                int pageNumber = request.PageNumber.ValidatePageNumber();
                int pageSize = request.PageSize.ValideatePageSize();

                var unitList = await _unitOfWork.Units.GetAllUnitsAsync(pageNumber, pageSize);

                var unitListForPage = unitList.Units
                    .Select(model => _mapper.Map<Game.Shared.Models.Unit>(model));

                return new CollectionResponse<Game.Shared.Models.Unit>
                {
                    Records = unitListForPage,
                    PagingInformations = new PagingInformations
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        PagesCount = Paging.CalculatePageCount(unitList.Count, pageSize)
                    }
                };
            }
        }
    }
}
