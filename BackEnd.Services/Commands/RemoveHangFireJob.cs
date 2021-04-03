using BackEnd.Repositories.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands
{
    public static class RemoveHangFireJob
    {
        public record Command(string JobId) : IRequest<Unit>;

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var job = await _unitOfWork.HangFire.GetJobByJobId(request.JobId);

                if (job != null) 
                {
                    _unitOfWork.HangFire.RemoveJob(job);
                    await _unitOfWork.CommitChangesAsync();
                }

                return new Unit();
            }
        }
    }
}
