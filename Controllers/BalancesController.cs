using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BankingSystem.Data;
using BankingSystem.Models;
using BankingSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using BankingSystem.DTO;

namespace BankingSystem.Controllers
{
	public class BalancesController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly IBalanceRepository _balanceRepository;
		private readonly ITransactionRepository _transactionRepository;

		public BalancesController(IBalanceRepository balanceRepository, ITransactionRepository transactionRepository, UserManager<User> userManager)
		{
			_balanceRepository = balanceRepository;
			_transactionRepository = transactionRepository;
			_userManager = userManager;
		}

		public IActionResult GetBalance()
		{
			System.Security.Claims.ClaimsPrincipal currentUser = User;
			var id = _userManager.GetUserId(User);
			var balance = _balanceRepository.GetBalance(id);
			if (balance != null)
				return View(balance);
			return RedirectToAction("NoBalance");
		}

		public IActionResult Deposit()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Deposit([Bind("deposit", "account")] DepositModel model)
		{
			System.Security.Claims.ClaimsPrincipal currentUser = User;
			var id = _userManager.GetUserId(User);
			var balance = _balanceRepository.GetBalance(id);
			Transaction transaction;

			if (model.account.Equals(Accounts.Checking))
			{
				balance.CheckingTransaction(model.deposit);
				transaction = new Transaction(model.deposit, "+$" + model.deposit.ToString(), Accounts.Checking, balance);
				transaction.Time = DateTime.Now;
				balance.Transactions.Add(transaction);
				_transactionRepository.AddTransaction(transaction);
			}
			else if (model.account.Equals(Accounts.Savings))
			{
				balance.SavingsTransaction(model.deposit);
				transaction = new Transaction(model.deposit, "+$" + model.deposit.ToString(), Accounts.Savings, balance);
				transaction.Time = DateTime.Now;
				balance.Transactions.Add(transaction);
				_transactionRepository.AddTransaction(transaction);
			}
			_balanceRepository.UpdateBalance(balance);



			return RedirectToAction("GetBalance");
		}

		public IActionResult Transfer()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Transfer([Bind("trans", "fromAccount", "toAccount")] TransferModel model)
		{
			var id = _userManager.GetUserId(User);
			var balance = _balanceRepository.GetBalance(id);

			double trans = model.trans;
			string fromAccount = model.fromAccount;
			string toAccount = model.toAccount;
			Transaction trans1, trans2;

			if (fromAccount.Equals(toAccount))
			{
				ModelState.AddModelError("message", "Accounts must be different");
				return RedirectToAction("Transfer");
			}

			if (fromAccount.Equals(Accounts.Checking))
			{
				if (balance.Checking < trans)
				{
					ModelState.AddModelError("message", "Insufficient funds");
					return RedirectToAction("Transfer");
				}
				else
				{
					balance.CheckingTransaction(trans * -1);
					balance.SavingsTransaction(trans);

					trans1 = new Transaction(trans * -1, "-$" + trans.ToString(), Accounts.Checking, balance);
					trans1.Time = DateTime.Now;
					balance.Transactions.Add(trans1);
					_transactionRepository.AddTransaction(trans1);

					trans2 = new Transaction(trans, "+$" + trans.ToString(), Accounts.Savings, balance);
					trans2.Time = DateTime.Now;
					balance.Transactions.Add(trans2);
					_transactionRepository.AddTransaction(trans2);
				}
			}
			else if (fromAccount.Equals(Accounts.Savings))
			{
				if (balance.Checking < trans)
				{
					ModelState.AddModelError("message", "Insufficient funds");
					return RedirectToAction("Transfer");
				}
				else
				{
					balance.SavingsTransaction(trans * -1);
					balance.CheckingTransaction(trans);

					trans1 = new Transaction(trans, "+$" + trans.ToString(), Accounts.Checking, balance);
					trans1.Time = DateTime.Now;
					balance.Transactions.Add(trans1);
					_transactionRepository.AddTransaction(trans1);

					trans2 = new Transaction(trans * -1, "-$" + trans.ToString(), Accounts.Savings, balance);
					trans2.Time = DateTime.Now;
					balance.Transactions.Add(trans2);
					_transactionRepository.AddTransaction(trans2);
				}
			}

			_balanceRepository.UpdateBalance(balance);

			return RedirectToAction("GetBalance");
		}

		public IActionResult NoBalance()
		{
			return View();
		}
	}



}

