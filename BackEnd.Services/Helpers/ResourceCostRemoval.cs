using BackEnd.Models.Models;
using SendGrid.Helpers.Errors.Model;

namespace Services.Helpers
{
    public static class ResourceCostRemoval
    {
        public static void PayTheCost(City city, Resources cost)
        {
            if (
                city.Resources.Population < cost.Population ||
                city.Resources.Wood < cost.Wood ||
                city.Resources.Stone < cost.Stone ||
                city.Resources.Silver < cost.Silver)
                throw new BadRequestException("The city does not have enough resources");

            city.Resources.Population -= cost.Population;
            city.Resources.Wood -= cost.Wood;
            city.Resources.Silver -= cost.Silver;
            city.Resources.Stone -= cost.Stone;
        }
    }
}
