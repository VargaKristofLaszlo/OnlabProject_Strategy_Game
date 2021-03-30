using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Admin
{
    public static class ModerateCityName
    {
        public record Command(CityNameModerationRequest Request) : IRequest<Unit>;

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;     

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;               
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Users.GetUserWithCities(request.Request.UserId);

                if (user == null)
                    throw new NotFoundException();

                var city = user.Cities                    
                    .Where(city => city.CityName.Equals(request.Request.OldCityName))
                    .FirstOrDefault();

                if (city == null)
                    throw new NotFoundException();

                city.CityName = request.Request.NewCityName;

                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }
        }
    }
}
