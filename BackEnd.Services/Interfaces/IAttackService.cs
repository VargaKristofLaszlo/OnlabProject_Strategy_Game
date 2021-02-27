using Game.Shared.Models.Request;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IAttackService
    {
        Task AttackOtherCity(AttackRequest request);
    }
}
