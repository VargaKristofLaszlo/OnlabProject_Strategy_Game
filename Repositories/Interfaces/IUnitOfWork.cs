using System.Threading.Tasks;

namespace BackEnd.Repositories.Interfaces
{
    public interface IUnitOfWork
    {
        IUsersRepository Users { get; }

        IUpgradeCostRepository UpgradeCosts { get; }

        Task CommitChangesAsync();
    }
}
