using BackEnd.Infrastructure;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetCityNamesOfUser
    {
        public record Query(string UserId = null) : IRequest<IEnumerable<string>>;

        public class Handler : IRequestHandler<Query, IEnumerable<string>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityOptions)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityOptions;
            }

            public async Task<IEnumerable<string>> Handle(Query request, CancellationToken cancellationToken)
            {
                ApplicationUser user;
                if (string.IsNullOrEmpty(request.UserId))
                    user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);

                else
                    user = await _unitOfWork.Users.GetUserWithCities(request.UserId);

                return user.Cities.Select(city => city.CityName);
            }
        }
    }
}