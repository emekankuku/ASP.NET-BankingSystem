using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Models
{
    public class Balance
    {
        [Key, ForeignKey("UserId")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public double Checking {  get; set; }
        public double Savings {  get; set; }
        public ICollection<Transaction> Transactions { get; } = new List<Transaction>();

        public Balance()
        {
        }

        public Balance(double checking, double savings, User user)
        {
            Checking = checking;
            Savings = savings;
            User = user;
        }

        public Balance(double checking, double savings)
        {
            Checking = checking;
            Savings = savings;
        }

        public void CheckingTransaction(double deposit)
        {
            Checking += deposit;
        }

        public void SavingsTransaction(double deposit)
        {
            Savings += deposit;
        }
    }
}
