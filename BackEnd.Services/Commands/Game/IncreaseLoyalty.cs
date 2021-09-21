using BackEnd.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Commands.Game
{
    public class IncreaseLoyalty
    {
        private readonly IUnitOfWork _unitOfWork;

        public IncreaseLoyalty(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task IncreaseCityLoyalty()
        {
            var cities = await _unitOfWork.Cities.GetAllCities();

            foreach (var city in cities)
            {
                if (city.Loyalty < 100)
                    city.Loyalty = ForceLimit(city.Loyalty + 5);
            }
            await _unitOfWork.CommitChangesAsync();
        }

        private int ForceLimit(int loyalty)
        {
            if (loyalty > 100)
                return 100;
            else
                return loyalty;
        }
    }
}
