using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Request;
using MediatR;
using Services.Exceptions;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading;
using System.Threading.Tasks;

namespace Services.Commands.Admin
{
    public static class BanUser
    {
        public record Command(UserBanRequest Request) : IRequest<Unit>;

        public class Handler : IRequestHandler<Command, Unit>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IEmailSender _emailSender;

            public Handler(IUnitOfWork unitOfWork, IEmailSender emailSender)
            {
                _unitOfWork = unitOfWork;
                _emailSender = emailSender;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(request.Request.Username);

                if (user == null)
                    throw new NotFoundException();

                user.IsBanned = !user.IsBanned;

                if(user.IsBanned)
                    await _emailSender.SendEmailAsync(user.Email, request.Request.CauseOfBan,
                        request.Request.Message);
             
                await _unitOfWork.CommitChangesAsync();

                return new Unit();
            }
        }
    }
}
