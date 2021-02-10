using BackEnd.Models.Models;
using BackEnd.Repositories;
using BackEnd.Repositories.Interfaces;
using BackEnd.Services.Extensions;
using BackEnd.Services.Interfaces;
using Shared.Models;
using Shared.Models.Response;
using System.Linq;
using System.Threading.Tasks;

namespace BackEnd.Services.Implementations
{
    public class ViewService : IViewService 
    {
        private readonly IUnitOfWork _unitOfWork;

        public ViewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private int CalculatePageCount(int modelCount, int pageSize) 
        {
            int pagesCount = modelCount / pageSize;

            if (modelCount % pageSize != 0)
                pagesCount += 1;

            return pagesCount;
        }

        public async Task<OperationResponse<Shared.Models.BuildingUpgradeCost>> GetBuildingUpgradeCost(string buildingName, int buildingStage)
        {
            var result = await _unitOfWork.UpgradeCosts.FindUpgradeCost(buildingName, buildingStage);

            if (result == null)
                return OperationResponse<Shared.Models.BuildingUpgradeCost>.Failed("Could not find the specified upgrade cost");

            return OperationResponse<Shared.Models.BuildingUpgradeCost>.Succeeded("Found the upgrade cost", result.ToBuildingUpgradeCostDto());
        }

        public async Task<CollectionResponse<string>> GetCityNamesOfUser(string username, int pageNumber = 1, int pageSize = 10)
        {
            //Validation
            pageNumber.ValidatePageNumber();
            pageSize.ValideatePageSize();

            var user = await _unitOfWork.Users.FindUserByUsernameOrNullAsync(username);

            if (user == null)
                return CollectionResponse<string>.Failed("User not found");

            var cityNameList = user.Cities.Select(city => city.CityName);
            int cityNameListCount = cityNameList.Count();

            var cityNameListForPage = cityNameList
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);           

            return CollectionResponse<string>.Succeeded("City names successfully retrieved from user", cityNameListForPage,
                new PagingInformations(pageNumber, pageSize, CalculatePageCount(cityNameListCount, pageSize)));
        }

        public async Task<CollectionResponse<Credentials>> GetUserCredentialsAsync(int pageNumber = 1, int pageSize = 10)
        {
            //Validation
            pageNumber.ValidatePageNumber();
            pageSize.ValideatePageSize();            

            var credentialList = await _unitOfWork.Users.GetAllUsersAsync(pageNumber, pageSize);
            int credentialListCount = credentialList.Count();

            var credentialListForPage = credentialList               
                    .Select(model => model.ToCredentialsDto());        

            return CollectionResponse<Credentials>.Succeeded("List of credentials retrieved succesfully", credentialListForPage,
                new PagingInformations(pageNumber, pageSize, CalculatePageCount(credentialListCount, pageSize)));
        }
    }
}
