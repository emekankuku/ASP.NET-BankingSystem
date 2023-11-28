using BankingSystem.DTO;
using BankingSystem.Models;
using BankingSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace BankingSystem.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class ApiAuthenticationController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly IConfiguration _configuration;
        private readonly IBalanceRepository _balanceRepository;

        public ApiAuthenticationController(UserManager<User> userManager, IConfiguration configuration, IBalanceRepository balanceRepository)
        {
            this.userManager = userManager;
            _configuration = configuration;
            _balanceRepository = balanceRepository;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            if (model.checking < 500.00 || model.savings < 500.00)
                return StatusCode(StatusCodes.Status500InternalServerError, "Checking and savings account balances must start at $500.00");

            User user = new User()
            {
                Email = model.email,
                UserName = model.username,
                SecurityStamp = Guid.NewGuid().ToString(),
            };
            var newUser = await userManager.CreateAsync(user, model.password);
            if (!newUser.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            Balance newBalance = new Balance(model.checking, model.savings, user);
            _balanceRepository.AddBalance(newBalance);
            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.password))
            {
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }
    }
}
