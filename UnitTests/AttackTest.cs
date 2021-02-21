using BackEnd.Models.Models;
using System;
using Xunit;

namespace UnitTests
{
    public class AttackTest
    {
        [Fact]
        public void Test()
        {
            var attackingCity = new City
            {
                Barrack = new Barrack { Id = "barrack_id_one", Stage = 3, BuildingName = "Barrack" },
                BarrackId = "barrack_id_one"
            };

            var attackingUser = new ApplicationUser() { Id = "id_one", };
        }
    }
}
