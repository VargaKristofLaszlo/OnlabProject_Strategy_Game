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
    public static class GetUnitByName
    {
        public record Query(string UnitName) : IRequest<Game.Shared.Models.Resources>;

        public class Handler : IRequestHandler<Query, Game.Shared.Models.Resources>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper;            

            public Handler(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
              
            }

            public async Task<Game.Shared.Models.Resources> Handle(Query request, CancellationToken cancellationToken)
            {
                var unit = await _unitOfWork.Units.FindUnitByName(request.UnitName);

                return _mapper.Map<Game.Shared.Models.Resources>(unit.UnitCost);

            }
        }
    }
}
