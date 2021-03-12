using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using MediatR;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetProducibleUnitTypes
    {
        public record Query(int CityIndex) : IRequest<IEnumerable<Game.Shared.Models.Unit>>;

        public class Handler : IRequestHandler<Query, IEnumerable<Game.Shared.Models.Unit>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IMapper mapper, IIdentityContext identityContext)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
                _identityContext = identityContext;
            }

            public async Task<IEnumerable<Game.Shared.Models.Unit>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);

                if (user == null)
                    throw new NotFoundException();

                var city = await _unitOfWork.Cities.FindCityById(user.Cities[request.CityIndex].Id);

                var producibleUnits = await _unitOfWork.Units.GetProducibleUnitTypes(city.Barrack.Stage);

                return producibleUnits.ToList().Select(unit => _mapper.Map<Game.Shared.Models.Unit>(unit));
            }
        }
    }
}
