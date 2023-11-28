using BankingSystem.Data;
using BankingSystem.Models;

namespace BankingSystem.Repositories
{
	public class TransactionRepository: ITransactionRepository
	{
		private readonly ApplicationDbContext _context;
		public TransactionRepository(ApplicationDbContext context)
		{
			_context = context;
		}

			public void AddTransaction(Transaction transaction)
			{
				_context.Add(transaction);
				_context.SaveChanges();
			}
		}
}
