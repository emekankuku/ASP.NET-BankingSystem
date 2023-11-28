using System.ComponentModel.DataAnnotations;

namespace BankingSystem.DTO
{
    public class DepositModel
    {

        [Required(ErrorMessage = "Deposit amount is required")]
        public double deposit { get; set; }

        [Required(ErrorMessage = "Account is required")]
        public string account { get; set; }


    }
}
