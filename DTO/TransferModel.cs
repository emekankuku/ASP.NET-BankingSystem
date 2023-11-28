using System.ComponentModel.DataAnnotations;

namespace BankingSystem.DTO
{
	public class TransferModel
	{
		[Required(ErrorMessage = "Transaction amount is required")]
		public double trans { get; set; }

		[Required(ErrorMessage = "Account is required")]
		public string fromAccount { get; set; }

		[Required(ErrorMessage = "Account is required")]
		public string toAccount { get; set; }
	}
}
