using FirstProject___Test.Models;
using FirstProject___Test.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject___Test.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserRepository _userRepository;
        public RegisterController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SignUp(RegistrationModel model) 
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.GetUserByUsername(model.Username) != null)
                {
                    ModelState.AddModelError("Username", "Username Already Taken");
                }
                if(_userRepository.GetUserByEmail(model.Email) != null)
                {
                    ModelState.AddModelError("Email", "Email Already Taken");
                }
                else
                {
                    var newUser = new User
                    {
                        username = model.Username,
                        email = model.Email,
                        password = _userRepository.ComputeHash(model.Password)
                    };
                    _userRepository.InsertUser(newUser);
                    return RedirectToAction("Index","Login");
                   
                }
            }
            return View(model);
        }

        
    }
}
