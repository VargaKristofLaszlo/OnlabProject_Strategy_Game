using AutoMapper;
using BackEnd.Models.Models;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Profiles
{
    public class UserCredentialProfile : Profile
    {
        public UserCredentialProfile()
        {
            CreateMap<ApplicationUser, Credentials>()
                .ForMember(credential => credential.Username, map => map.MapFrom(user => user.UserName))
                .ForMember(credential => credential.Email, map => map.MapFrom(user => user.Email))
                .ForMember(credential => credential.Id, map => map.MapFrom(user => user.Id));
        }
    }
}
