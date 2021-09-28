using AutoMapper;
using BackEnd.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Profiles
{
    public class ReportProfile : Profile
    {
        public ReportProfile()
        {
            CreateMap<Report, Game.Shared.Models.Report>().ReverseMap();
            CreateMap<SypReport, Game.Shared.Models.SpyReport>().ReverseMap();



        }
    }
}
