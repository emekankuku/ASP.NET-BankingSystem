using BankingSystem.Models;
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.DTO
{
    public class BalanceModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string username { get; set; }

        [Required(ErrorMessage = "Checking Balance is required")]
        public double checking { get; set; }

        [Required(ErrorMessage = "Savings Balance is required")]
        public double savings { get; set; }

        public BalanceModel() { }
    }
}
