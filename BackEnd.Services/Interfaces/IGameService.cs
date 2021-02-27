using BackEnd.Models.Models;
using Game.Shared.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IGameService
    {
       
        Task ProduceUnits(UnitProductionRequest request);
        Task SendResourcesToOtherPlayer(SendResourceToOtherPlayerRequest request);        
    }
}
