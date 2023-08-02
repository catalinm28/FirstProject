using FirstProject___Test.Models;
using FirstProject___Test.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProject___Test.Controllers
{
    public class PostController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly UserRepository _userRepository;
        
        public PostController(PostRepository postRepository,UserRepository userRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<Post> posts = _postRepository.GetAllPosts();
            return View(posts);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Add() { return View(); }
        [HttpPost]

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Add(Post post)
        {


            if (ModelState.IsValid)
            {
                post.userToken = GetCurrentUserToken();
                post.createdAt = DateTime.UtcNow;
                _postRepository.InsertPost(post);
                return RedirectToAction("Index", "Home");
            }

            return View(post);
        }
        private Guid GetCurrentUserToken()
        {
            var claim = HttpContext.User.Claims;
            var userTokenClaim = claim.FirstOrDefault(c=>c.Type==ClaimTypes.NameIdentifier);
            if(userTokenClaim != null&& Guid.TryParse(userTokenClaim.Value,out Guid userToken))
            {
                return userToken;
            }
            return Guid.Empty;
        }
    }
}
