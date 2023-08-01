using FirstProject___Test.Models;
using FirstProject___Test.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            List<Post> posts = _postRepository.GetAllPosts();
            return View(posts);
        }
    }
}
