using AutoMapper;
using BackEnd.Infrastructure;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands
{
    public static class SpyOtherCity
    {
        public record Command(SpyRequest Request) : IRequest<SpyReport>;

        public class Handler : IRequestHandler<Command, SpyReport>
        {
            private readonly IIdentityContext _identityContext;
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;

            public Handler(IIdentityContext identityContext, IUnitOfWork unitOfWork, IMapper mapper)
            {
                _identityContext = identityContext;
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<SpyReport> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Users.GetUserWithCities(request.Request.UserId);

                if (user == null)
                    throw new NotFoundException();

                var city = await _unitOfWork.Cities.FindCityById(user.Cities[request.Request.TargetCityIndex].Id);

                if (city == null)
                    throw new NotFoundException("City could not be found");

                var spyUser = await _unitOfWork.Users.GetUserWithCities(_identityContext.UserId);
                var spyCity = await _unitOfWork.Cities.FindCityById(spyUser.Cities[request.Request.OwnerCityIndex].Id);

                if (spyCity.Tavern.SpyCount < request.Request.UsedSpyCount)
                    throw new BadRequestException("Not enough spies");

                var successChance = 60 + (request.Request.UsedSpyCount - city.Tavern.SpyCount) * 5;

                if (successChance < new Random().Next(0, 100))
                {
                    city.Tavern.SpyCount -= request.Request.UsedSpyCount;

                    await _unitOfWork.CommitChangesAsync();

                    var failedeEpionage = new SpyReport() { Successful = false };

                    await _unitOfWork.Reports.CreateSpyReport(_mapper.Map<BackEnd.Models.Models.SypReport>(failedeEpionage));

                    await _unitOfWork.CommitChangesAsync();

                    return failedeEpionage;
                }

                var cityDetails = _mapper.Map<CityDetails>(city);

                var unitsOfTheCity = await _unitOfWork.Units.GetUnitsInCityByBarrackId(city.Barrack.Id);

                var unitDetails = new UnitsOfTheCity();

                foreach (var item in unitsOfTheCity)
                {
                    switch (item.Unit.Name)
                    {
                        case "Swordsman":
                            unitDetails.Swordsmans += item.Amount;
                            break;
                        case "Heavy Cavalry":
                            unitDetails.HeavyCavalry += item.Amount;
                            break;
                        case "Mounted Archer":
                            unitDetails.MountedArcher += item.Amount;
                            break;
                        case "Light Cavalry":
                            unitDetails.LightCavalry += item.Amount;
                            break;
                        case "Spearman":
                            unitDetails.Spearmans += item.Amount;
                            break;
                        case "Archer":
                            unitDetails.Archers += item.Amount;
                            break;
                        case "Axe Fighter":
                            unitDetails.AxeFighers += item.Amount;
                            break;
                        case "Noble":
                            unitDetails.Noble += item.Amount;
                            break;
                        default:
                            break;
                    }

                }

                var report = new SpyReport()
                {
                    Attacker = _identityContext.Username,
                    CityName = cityDetails.CityName,
                    BarrackStage = cityDetails.BarrackStage,
                    CityHallStage = cityDetails.CityHallStage,
                    CityWallStage = cityDetails.CityWallStage,
                    FarmStage = cityDetails.FarmStage,
                    SilverMineStage = cityDetails.SilverMineStage,
                    StoneMineStage = cityDetails.StoneMineStage,
                    LumberStage = cityDetails.LumberStage,
                    WarehouseStage = cityDetails.WarehouseStage,
                    CastleStage = cityDetails.CastleStage,
                    TavernStage = cityDetails.TavernStage,

                    Spearmans = unitDetails.Spearmans,
                    Swordsmans = unitDetails.Swordsmans,
                    AxeFighers = unitDetails.AxeFighers,
                    Archers = unitDetails.Archers,
                    LightCavalry = unitDetails.LightCavalry,
                    MountedArcher = unitDetails.MountedArcher,
                    HeavyCavalry = unitDetails.HeavyCavalry,
                    Noble = unitDetails.Noble,

                    Wood = city.Resources.Wood,
                    Stone = city.Resources.Stone,
                    Silver = city.Resources.Silver,

                    CreationTime = DateTime.UtcNow,

                    Successful = true
                };

                await _unitOfWork.Reports.CreateSpyReport(_mapper.Map<BackEnd.Models.Models.SypReport>(report));

                await _unitOfWork.CommitChangesAsync();

                return report;
            }
        }
    }
}
