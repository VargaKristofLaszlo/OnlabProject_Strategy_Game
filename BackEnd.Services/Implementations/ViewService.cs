using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Extensions;
using BackEnd.Services.Interfaces;
using Shared.Models;
using Shared.Models.Response;
using System.Linq;
using System.Threading.Tasks;
using Services.Exceptions;
using AutoMapper;
using Game.Shared.Models;

namespace BackEnd.Services.Implementations
{
    public class ViewService : IViewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ViewService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private int CalculatePageCount(int modelCount, int pageSize)
        {
            int pagesCount = modelCount / pageSize;

            if (modelCount % pageSize != 0)
                return pagesCount + 1;

            return  pagesCount;
        }

        public async Task<BuildingUpgradeCost> GetBuildingUpgradeCost(string buildingName, int buildingStage)
        {
            var result = await _unitOfWork.UpgradeCosts.FindUpgradeCost(buildingName, buildingStage);

            if (result == null)
                throw new NotFoundException();

            return _mapper.Map<BuildingUpgradeCost>(result);
        }

        public async Task<CollectionResponse<string>> GetCityNamesOfUser(string username, int pageNumber = 1, int pageSize = 10)
        {
            //Validation
            pageNumber.ValidatePageNumber();
            pageSize.ValideatePageSize();

            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(username);

            if (user == null)
                throw new NotFoundException();

            var cityNameList = user.Cities.Select(city => city.CityName);
            int cityNameListCount = cityNameList.Count();

            var cityNameListForPage = cityNameList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return new CollectionResponse<string>
            {
                Records = cityNameListForPage,
                PagingInformations = new PagingInformations 
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    PagesCount = CalculatePageCount(cityNameListCount, pageSize)
                }
            };
        }


        public async Task<CollectionResponse<Credentials>> GetUserCredentialsAsync(int pageNumber = 1, int pageSize = 10)
        {
            //Validation
            pageNumber.ValidatePageNumber();
            pageSize.ValideatePageSize();

            var credentialList = await _unitOfWork.Users.GetAllUsersAsync(pageNumber, pageSize);

            var credentialListForPage = credentialList.Users
                    .Select(model => _mapper.Map<Credentials>(model));

            return new CollectionResponse<Credentials>
            {
                Records = credentialListForPage,
                PagingInformations = new PagingInformations 
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    PagesCount = CalculatePageCount(credentialList.Count, pageSize)
                }
            };
        }

        public async Task<CollectionResponse<Unit>> GetUnitTypes(int pageNumber = 1, int pageSize = 10)
        {
            //Validation
            pageNumber.ValidatePageNumber();
            pageSize.ValideatePageSize();

            var unitList = await _unitOfWork.Units.GetAllUnitsAsync(pageNumber, pageSize);

            var unitListForPage = unitList.Units
                .Select(model => _mapper.Map<Unit>(model));

            return new CollectionResponse<Unit>
            {
                Records = unitListForPage,
                PagingInformations = new PagingInformations 
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    PagesCount = CalculatePageCount(unitList.Count, pageSize)
                }
            };
        }
    }
}
