using BankingSystem.Models;

namespace BankingSystem.Repositories
{
	public interface ITransactionRepository
	{
		public void AddTransaction(Transaction transaction);
	}
}
