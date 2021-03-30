using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Game.Server
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>
    {
        private readonly IUnitOfWork _unitOfWork;

        public MyUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            IUnitOfWork unitOfWork)
            : base(userManager, roleManager, optionsAccessor)
        {
            _unitOfWork = unitOfWork;
        }
             
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var userRole = await _unitOfWork.Users.FindUserRoleAsync(user);
            var isAdmin = userRole.Equals("Admin");

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
            identity.AddClaim(new Claim(ClaimTypes.Role, userRole));

            if(isAdmin)
                identity.AddClaim(new Claim("IsAdmin", isAdmin.ToString()));

            return identity;
        }
    }

    public class MyUserClaimsTransformer : IClaimsTransformation
    {
        private readonly IUnitOfWork _unitOfWork;

        public MyUserClaimsTransformer(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var existingClaimsIdentity = (ClaimsIdentity)principal.Identity;

            var userId = existingClaimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _unitOfWork.Users.FindUserByIdOrNullAsync(userId);
            var userRole = await _unitOfWork.Users.FindUserRoleAsync(user);
            var isAdmin = userRole.Equals("Admin");
            if (user != null)
            {
                existingClaimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                existingClaimsIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                existingClaimsIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                existingClaimsIdentity.AddClaim(new Claim(ClaimTypes.Role, userRole));

                if(isAdmin)
                    existingClaimsIdentity.AddClaim(new Claim("IsAdmin", isAdmin.ToString()));
            }


            return new ClaimsPrincipal(principal);

        }
    }
}
