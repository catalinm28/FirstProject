using FirstProject___Test.Joins;
using FirstProject___Test.Models;
using FirstProject___Test.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace FirstProject___Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PostRepository _postRepository;
        private readonly UserRepository _userRepository;
        private readonly HttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger,PostRepository postRepository,UserRepository userRepository)
        {
            _logger = logger;
            _postRepository = postRepository;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            List<PostUserJoin> posts = _postRepository.GetPostsWithUsername();
            if (User.Identity.IsAuthenticated)
            {
                
                var currentUserToken = GetCurrentUserToken();

                
                ViewBag.CurrentUserToken = currentUserToken;
            }
            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private Guid GetCurrentUserToken()
        {
            var claim = HttpContext.User.Claims;
            var userTokenClaim = claim.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userTokenClaim != null && Guid.TryParse(userTokenClaim.Value, out Guid userToken))
            {
                return userToken;
            }
            return Guid.Empty;
        }
    }
}