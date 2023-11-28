using BankingSystem.Data;
using BankingSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Repositories
{
    public class BalanceRepository : IBalanceRepository
    {

        private readonly ApplicationDbContext _context;
        public BalanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void AddBalance(Balance balance)
        {
            _context.Add(balance);
            _context.SaveChanges();
        }

        public Balance GetBalance(string id)
        {
            return _context.Balance.FirstOrDefault(b => b.UserId == id);
        }

        public void UpdateBalance(Balance balance)
        {
            _context.Entry(balance).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
