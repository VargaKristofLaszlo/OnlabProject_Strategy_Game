using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Queries
{
    public static class GetUnitsOfCity
    {
        public record Query(int CityIndex) : IRequest<UnitsOfTheCity>;

        public class Handler : IRequestHandler<Query, UnitsOfTheCity>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IIdentityContext _identityContext;

            public Handler(IUnitOfWork unitOfWork, IIdentityContext identityContext)
            {
                _unitOfWork = unitOfWork;
                _identityContext = identityContext;
            }

            public async Task<UnitsOfTheCity> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);

                var city = user.Cities.ElementAt(request.CityIndex);

                var units = await _unitOfWork.Units.GetUnitsInCityByBarrackId(city.BarrackId);

                var result = new UnitsOfTheCity();

                foreach (var item in units)
                {
                    switch (item.Unit.Name)
                    {
                        case "Swordsman":
                            result.Swordsmans += item.Amount;
                            break;
                        case "Heavy Cavalry":
                            result.HeavyCavalry += item.Amount;
                            break;
                        case "Mounted Archer":
                            result.MountedArcher += item.Amount;
                            break;
                        case "Light Cavalry":
                            result.LightCavalry += item.Amount;
                            break;
                        case "Spearman":
                            result.Spearmans += item.Amount;
                            break;
                        case "Archer":
                            result.Archers += item.Amount;
                            break;
                        case "Axe Fighter":
                            result.AxeFighers += item.Amount;
                            break;
                        case "Noble":
                            result.Noble += item.Amount;
                            break;
                        default:
                            break;
                    }
                }
                return result;
            }
        }
    }
}
