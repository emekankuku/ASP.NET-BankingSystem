using BankingSystem.Models;

namespace BankingSystem.Repositories
{
    public interface IBalanceRepository
    {
        public void AddBalance(Balance balance);

        public void UpdateBalance(Balance balance);

        public Balance GetBalance(string id);
    }
}
