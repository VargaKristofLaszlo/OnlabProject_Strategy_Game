using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using Game.Shared.Models.Response;
using MediatR;
using Services.Extensions;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetOtherUsersCities
    {
        public record Query(int PageNumber, int PageSize) : IRequest<CollectionResponse<CityPagingData>>;

        public class Handler : IRequestHandler<Query, CollectionResponse<CityPagingData>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityOptions)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityOptions;
            }

            public async Task<CollectionResponse<CityPagingData>> Handle(Query request, CancellationToken cancellationToken)
            {
                //Validation
                int pageNumber = request.PageNumber.ValidatePageNumber();
                int pageSize = request.PageSize.ValideatePageSize();


                var users = await _unitOfWork.Users.GetAllUsersWithCities(request.PageNumber, request.PageSize, _identityContext.UserId);
                var listForPage = new List<CityPagingData>();

                foreach (var user in users.Users)
                {
                    listForPage.AddRange(MapToDtoFromModel(user));
                }

                return new CollectionResponse<CityPagingData>()
                {
                    Records = listForPage,
                    PagingInformations = new PagingInformations
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        PagesCount = Paging.CalculatePageCount(users.Count, pageSize)
                    }
                };
            }

            private List<CityPagingData> MapToDtoFromModel(ApplicationUser model) 
            {
                List<CityPagingData> res = new List<CityPagingData>();

                for (int i = 0; i < model.Cities.Count; i++)
                {
                    res.Add(new CityPagingData
                    {
                        Index = i,
                        Name = model.Cities.ElementAt(i).CityName,
                        OwnerId = model.Id,
                        OwnerName = model.UserName
                    });
                }
                return res;
            } 
        }
    }
}
