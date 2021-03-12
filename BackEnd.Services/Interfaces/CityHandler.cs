using BackEnd.Models.Models;
using BackEnd.Repositories.Interfaces;
using Services.Exceptions;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public abstract class CityHandler
    {
        protected readonly IUnitOfWork _unitOfWork;
        public CityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        protected bool CheckIfCityHasEnoughResources(City city, Resources cost)
        {
            return
                city.Resources.Population >= cost.Population &&
                city.Resources.Silver >= cost.Silver &&
                city.Resources.Stone >= cost.Stone &&
                city.Resources.Wood >= cost.Wood;
        }

        protected void PayTheCost(City city, Resources cost)
        {
            city.Resources.Population -= cost.Population;
            city.Resources.Wood -= cost.Wood;
            city.Resources.Silver -= cost.Silver;
            city.Resources.Stone -= cost.Stone;
        }

        protected async Task<City> GetCityByCityIndex(int cityIndex, string userId)
        {
            var user = await _unitOfWork.Users.GetUserWithCities(userId);

            if (user == null)
                throw new NotFoundException();

            var city = await _unitOfWork.Cities.FindCityById(user.Cities[cityIndex].Id);

            if (city == null)
                throw new NotFoundException();

            return city;
        }
    }
}
