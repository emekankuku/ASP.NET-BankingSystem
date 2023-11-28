using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Models
{
	public class Transaction
	{
		public long Id { get; set; }
		public Balance balance;
		public double TransAmount { get; set; }
		public string Memo { get; set; }
		public string Account { get; set; }
		public DateTimeOffset Time { get; set; }

		public Transaction()
		{
		}
		public Transaction(double transAmount, string memo, string account, Balance balance)
		{
			TransAmount = transAmount;
			Memo = memo;
			Account = account;
			this.balance = balance;
		}

	}
}
