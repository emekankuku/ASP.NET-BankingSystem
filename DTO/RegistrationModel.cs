using System.ComponentModel.DataAnnotations;

namespace BankingSystem.DTO
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }

        [Required(ErrorMessage = "Checking Balance is required")]
        public double checking { get; set; }

        [Required(ErrorMessage = "Savings Balance is required")]
        public double savings { get; set; }

    }
}
