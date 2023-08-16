using FirstProject___Test.Models;
using FirstProjectRepository.DBModels;
using FirstProjectRepository.Helpers;
using FirstProjectRepository.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FirstProject___Test.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly Crypt _crypt;
        public RegisterController(UserRepository userRepository,Crypt crypt)
        {
            _userRepository = userRepository;
            _crypt = crypt;
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
                        password = _crypt.ComputeHash(model.Password)
                    };
                    _userRepository.InsertUser(newUser);
                    return RedirectToAction("Login","Login");
                   
                }
            }
            return View(model);
        }

        
    }
}
