using Repositories.Interfaces;
using System.Threading.Tasks;

namespace BackEnd.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUsersRepository Users { get; }
        IUpgradeCostRepository UpgradeCosts { get; }
        IUnitRepository Units { get; }
        ICityRepository Cities { get; }       
        Task<int> CommitChangesAsync();
    }
}
