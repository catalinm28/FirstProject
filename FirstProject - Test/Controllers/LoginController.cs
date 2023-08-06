using FirstProjectRepository.Helpers;
using FirstProjectRepository.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstProject___Test.Controllers
{
    public class LoginController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly string _secretKey;
        private readonly Crypt _crypt;
        public LoginController(UserRepository userRepository,Crypt crypt)
        {
            _userRepository = userRepository;
            _secretKey = "my - 32 - character - ultra - secure - and - ultra - long - secret";
            _crypt = crypt;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            var user = _userRepository.GetUserByUsername(username);
            if (user == null || !VerifyPassword(password, user.password))
            {
                ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
            var token = GenerateJwtToken(user.userToken);

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTime.UtcNow.AddMinutes(30)
            });
            return RedirectToAction("Index", "Home");
        }
        private string GenerateJwtToken(Guid userToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);
            var tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, userToken.ToString())
                }),
                Audience = "https://localhost",
                Issuer = "https://localhost",
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDecriptor);
            return tokenHandler.WriteToken(token);
        }
        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            string inputHash = _crypt.ComputeHash(inputPassword);
            return storedHash == inputHash;
        }
        

    }
}
