using AutoMapper;
using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Game.Shared.Models.Response;
using Models.Models;
using Services.Extensions;
using Services.Interfaces;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{

    public class ReportSender : IReportSender
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportSender(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateAttackReport(string attacker, string attackerCityName, string defender, string defenderCityName,
            Dictionary<Unit, int> attackerTroops, Dictionary<Unit, int> defendingTroops,
            Dictionary<string, int> attackingUnits, IEnumerable<UnitsInCity> defendingUnits,
            int stolenWoodAmount, int stolenStoneAmount, int stolenSilverAmount, int loyalty)
        {
            Report report = new Report()
            {
                CreationTime = DateTime.UtcNow,
                Attacker = attacker,
                AttackerCityName = attackerCityName,
                Defender = defender,
                DefendingCityName = defenderCityName,
                Loyalty = loyalty,
                ReportType = Game.Shared.Models.ReportType.Attack,

                StolenWoodAmount = stolenWoodAmount,
                StolenStoneAmount = stolenStoneAmount,
                StolenSilverAmount = stolenSilverAmount,

                SwordsmanAttackerCountBefore = attackingUnits.First(x => x.Key.Equals("Swordsman")).Value,
                HeavyCavalryAttackerCountBefore = attackingUnits.First(x => x.Key.Equals("Heavy Cavalry")).Value,
                MountedArcherAttackerCountBefore = attackingUnits.First(x => x.Key.Equals("Mounted Archer")).Value,
                LightCavalryAttackerCountBefore = attackingUnits.First(x => x.Key.Equals("Light Cavalry")).Value,
                SpearmanAttackerCountBefore = attackingUnits.First(x => x.Key.Equals("Spearman")).Value,
                ArcherAttackerCountBefore = attackingUnits.First(x => x.Key.Equals("Archer")).Value,
                AxeFighterAttackerCountBefore = attackingUnits.First(x => x.Key.Equals("Axe Fighter")).Value,
                NobleAttackerCountBefore = attackingUnits.First(x => x.Key.Equals("Noble")).Value,


                SwordsmanAttackerCountAfter = attackerTroops.First(x => x.Key.Name.Equals("Swordsman")).Value,
                HeavyCavalryAttackerCountAfter = attackerTroops.First(x => x.Key.Name.Equals("Heavy Cavalry")).Value,
                MountedArcherAttackerCountAfter = attackerTroops.First(x => x.Key.Name.Equals("Mounted Archer")).Value,
                LightCavalryAttackerCountAfter = attackerTroops.First(x => x.Key.Name.Equals("Light Cavalry")).Value,
                SpearmanAttackerCountAfter = attackerTroops.First(x => x.Key.Name.Equals("Spearman")).Value,
                ArcherAttackerCountAfter = attackerTroops.First(x => x.Key.Name.Equals("Archer")).Value,
                AxeFighterAttackerCountAfter = attackerTroops.First(x => x.Key.Name.Equals("Axe Fighter")).Value,
                NobleAttackerCountAfter = attackerTroops.First(x => x.Key.Name.Equals("Noble")).Value,


                SwordsmanDefenderCountBefore = defendingUnits.First(x => x.Unit.Name.Equals("Swordsman")).Amount,
                HeavyCavalryDefenderCountBefore = defendingUnits.First(x => x.Unit.Name.Equals("Heavy Cavalry")).Amount,
                MountedArcherDefenderCountBefore = defendingUnits.First(x => x.Unit.Name.Equals("Mounted Archer")).Amount,
                LightCavalryDefenderCountBefore = defendingUnits.First(x => x.Unit.Name.Equals("Light Cavalry")).Amount,
                SpearmanDefenderCountBefore = defendingUnits.First(x => x.Unit.Name.Equals("Spearman")).Amount,
                ArcherDefenderCountBefore = defendingUnits.First(x => x.Unit.Name.Equals("Archer")).Amount,
                AxeFighterDefenderCountBefore = defendingUnits.First(x => x.Unit.Name.Equals("Axe Fighter")).Amount,
                NobleDefenderCountBefore = defendingUnits.First(x => x.Unit.Name.Equals("Noble")).Amount,


                SwordsmanDefenderCountAfter = defendingTroops.First(x => x.Key.Name.Equals("Swordsman")).Value,
                HeavyCavalryDefenderCountAfter = defendingTroops.First(x => x.Key.Name.Equals("Heavy Cavalry")).Value,
                MountedArcherDefenderCountAfter = defendingTroops.First(x => x.Key.Name.Equals("Mounted Archer")).Value,
                LightCavalryDefenderCountAfter = defendingTroops.First(x => x.Key.Name.Equals("Light Cavalry")).Value,
                SpearmanDefenderCountAfter = defendingTroops.First(x => x.Key.Name.Equals("Spearman")).Value,
                ArcherDefenderCountAfter = defendingTroops.First(x => x.Key.Name.Equals("Archer")).Value,
                AxeFighterDefenderCountAfter = defendingTroops.First(x => x.Key.Name.Equals("Axe Fighter")).Value,
                NobleDefenderCountAfter = defendingTroops.First(x => x.Key.Name.Equals("Noble")).Value
            };

            await _unitOfWork.Reports.CreateReport(report);
        }

        public async Task<CollectionResponse<Game.Shared.Models.Report>> GetReports(int pageNumber, int pageSize, string defenderName)
        {
            //Validation
            if (pageSize < 1)
                pageSize = 1;
            else if (pageSize > 20)
                pageSize = 20;
            if (pageNumber < 1)
                pageNumber = 1;


            pageSize = pageSize.ValideatePageSize();

            var (Reports, Count) = await _unitOfWork.Reports.GetAttackReports(pageNumber, pageSize, defenderName);

            var reportListForPage = Reports
                       .Select(model => _mapper.Map<Game.Shared.Models.Report>(model));

            return new CollectionResponse<Game.Shared.Models.Report>
            {
                Records = reportListForPage,
                PagingInformations = new PagingInformations
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    PagesCount = Paging.CalculatePageCount(Count, pageSize)
                }
            };
        }

        public async Task<CollectionResponse<Game.Shared.Models.SpyReport>> GetSpyReports(int pageNumber, int pageSize, string attackerName)
        {
            //Validation
            if (pageSize < 1)
                pageSize = 1;
            else if (pageSize > 20)
                pageSize = 20;
            if (pageNumber < 1)
                pageNumber = 1;


            pageSize = pageSize.ValideatePageSize();

            var (Reports, Count) = await _unitOfWork.Reports.GetSpyReports(pageNumber, pageSize, attackerName);

            var reportListForPage = Reports
                       .Select(model => _mapper.Map<Game.Shared.Models.SpyReport>(model));

            return new CollectionResponse<Game.Shared.Models.SpyReport>
            {
                Records = reportListForPage,
                PagingInformations = new PagingInformations
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    PagesCount = Paging.CalculatePageCount(Count, pageSize)
                }
            };
        }
    }
}
