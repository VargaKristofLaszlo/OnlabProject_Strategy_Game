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
        IReportRepository Reports { get; }
        IHangFireRepository HangFire { get; }
        Task<int> CommitChangesAsync();
        
    }
}
