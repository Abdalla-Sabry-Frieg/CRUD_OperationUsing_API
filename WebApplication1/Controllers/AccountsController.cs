using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public BinaryReader JwtRegisteredClaimName { get; private set; }

        public AccountsController(UserManager<ApplicationUser> userManager ,IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RegisterNewUser(DTO_NewUser user)
        {
            if(ModelState.IsValid) 
            {
                var appUser = new ApplicationUser()
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };
                IdentityResult result = await _userManager.CreateAsync(appUser , user.Password);
                if (result.Succeeded) 
                    { return Ok("Sucssess"); }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }

                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(DTO_LoginUser login)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByNameAsync(login.UserName);
                if (user != null)
                {
                     if(await _userManager.CheckPasswordAsync(user, login.Password))
                     {
                        // return Ok("Token"); // must create token and pass that in the return
                        // Create Claim and Role for user 

                        var claims = new List<Claim>();
                      //  claims.Add(new Claim("name", "Value")); // Custom claim can set ant name and value
                     
                        claims.Add(new Claim(ClaimTypes.Name , user.UserName));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        var roles = await _userManager.GetRolesAsync(user);

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }

                        // add JWT  ans signing credential 
                        var key =new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                        var SC = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            claims:claims,
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            expires: DateTime.Now.AddHours(1),
                            signingCredentials:SC
                            );

                        // Create anonymous object of token 

                        var _token = new
                        {
                            token= new JwtSecurityTokenHandler().WriteToken(token),
                            Exception = token.ValidTo,
                        };

                        return Ok(_token); 
                     }
                     else
                     {
                        return Unauthorized();
                     }
                }
                else
                {
                    ModelState.AddModelError("", "User Name is invalid"); // Key , Value (get the value to user in if statment )
                }
            }
            return BadRequest(ModelState);
        }

    }
}
 