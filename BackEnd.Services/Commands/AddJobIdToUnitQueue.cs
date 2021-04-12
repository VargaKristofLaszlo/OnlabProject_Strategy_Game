using BackEnd.Repositories.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands
{
    public static class AddJobIdToUnitQueue
    {
        public record Command(string JobId, string UserId,
            string UnitName, int CityIndex, int Amount) : IRequest<Unit>;

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var jobs = await _unitOfWork.HangFire.GetUserUnitProductionQueue(request.UserId, request.CityIndex);

                var job = jobs.FirstOrDefault(x =>
                    x.UnitName.Equals(request.UnitName) &&
                    x.CityIndex == request.CityIndex &&
                    x.Amount == request.Amount);

                if (job == null)
                    return new Unit();

                job.JobId = request.JobId;

                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }
        }
    }
}
