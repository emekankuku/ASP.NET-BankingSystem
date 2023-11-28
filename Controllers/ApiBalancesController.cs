using BankingSystem.Data;
using BankingSystem.DTO;
using BankingSystem.Models;
using BankingSystem.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Controllers
{
    [Route("api/Balances")]
    [ApiController]
    public class ApiBalancesController : ControllerBase
    {

        private readonly UserManager<User> _userManager;
        private readonly IBalanceRepository _balanceRepository;

        public ApiBalancesController(IBalanceRepository balanceRepository, UserManager<User> userManager)
        {
            _balanceRepository = balanceRepository;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateBalance([FromBody] BalanceModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.username);

            if (userExists == null)
                return Unauthorized();

            if (model.checking < 500.00 || model.savings < 500.00)
                return StatusCode(StatusCodes.Status500InternalServerError, "Checking and savings account balances must start at $500.00");

            Balance newBalance = new Balance(model.checking, model.savings, userExists);
            _balanceRepository.AddBalance(newBalance);

            return Ok(new Response { Status = "Success", Message = "Balance successfully created." });
        }

        [HttpPost]
        [Route("getBalance")]
        public async Task<IActionResult> GetBalance([FromBody] UserNameModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.username);
            BalanceModel bModel = new BalanceModel();

            if (userExists == null)
                return Unauthorized();

            var balanceExists = _balanceRepository.GetBalance(userExists.Id);
            if (balanceExists == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "No balance yet.");

            bModel.checking = balanceExists.Checking;
            bModel.savings = balanceExists.Savings;
            bModel.username = model.username;

            return Ok(new Response { Status = "Success", Message = "Checking: " + balanceExists.Checking + "; Savings: " + balanceExists.Savings });
        }

        [HttpPost]
        [Route("transaction")]
        public async Task<IActionResult> Transaction([FromBody] BalanceModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.username);

            if (userExists == null)
                return Unauthorized();

            var existingBalance = _balanceRepository.GetBalance(userExists.Id);

            if (existingBalance == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Balance unavailable.");

            existingBalance.CheckingTransaction(model.checking);
            existingBalance.SavingsTransaction(model.savings);

            if (existingBalance.Checking < 0 || existingBalance.Savings < 0)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to execute withdrawal due to insuffient funds.");

            _balanceRepository.UpdateBalance(existingBalance);

            return Ok(new Response { Status = "Success", Message = "Successfully implemented transaction!" });
        }

    }
}
