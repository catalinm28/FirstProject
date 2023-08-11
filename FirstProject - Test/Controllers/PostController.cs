using FirstProject___Test.Models;
using FirstProjectRepository.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstProjectRepository.DBModels;
using System.Drawing;
using System.Security.Claims;
using FirstProjectRepository.UsefullModels;

namespace FirstProject___Test.Controllers
{
    public class PostController : Controller
    {
        private readonly PostRepository _postRepository;
        private readonly UserRepository _userRepository;
        private readonly CommentRepository _commentRepository;
        private readonly VoteRepository _voteRepository;
        
        public PostController(PostRepository postRepository,UserRepository userRepository, CommentRepository commentRepository, 
            VoteRepository voteRepository)
        {
            _postRepository = postRepository;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _voteRepository = voteRepository;
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
        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            
            var post = _postRepository.GetPostById(id);

            
            if (post == null || post.userToken != GetCurrentUserToken())
            {
                return NotFound(); 
            }

            return View(post);
        }

        
        [HttpPost]
        [Authorize]
        public IActionResult Edit(int id, Post post)
        {
            
            var existingPost = _postRepository.GetPostById(id);

            
            if (existingPost == null || existingPost.userToken != GetCurrentUserToken())
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
               
                existingPost.title = post.title;
                existingPost.text = post.text;
                existingPost.url = post.url;
                
                _postRepository.UpdatePost(existingPost);

                
                return RedirectToAction("Index", "Home");
            }

            
            return View(post);
        }
        public IActionResult Newest()
        {
            List<PostWithUsername> posts = _postRepository.GetAllPostsSortedByDate();
            return View(posts);
        }
        [Authorize]
        public IActionResult Delete(int id)
        {
            var post = _postRepository.GetPostById(id);

            if (post == null || post.userToken != GetCurrentUserToken())
            {
                return NotFound();
            }

            return View(post);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public IActionResult ConfirmDelete(int id)
        {
            var post = _postRepository.GetPostById(id);

            if (post == null || post.userToken != GetCurrentUserToken())
            {
                return NotFound();
            }
            _commentRepository.DeleteCommentsByPostId(id);

            _postRepository.DeletePost(id);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult MyPosts()
        {
            var userToken = GetCurrentUserToken();
            var posts = _postRepository.GetPostByUserId(userToken);
            return View(posts);
        }
        [AllowAnonymous]
        public IActionResult ViewPost(int id)
        {
            ViewBag.CurrentUserToken = GetCurrentUserToken();
           
            var postWithComments = _postRepository.GetPostAndComments(id);
            return View(postWithComments);
        }
        [HttpPost]
        public IActionResult Upvote(int postId)
        {
            
            var post = _postRepository.GetPostById(postId);
            if (post == null)
            {
                return NotFound();
            }

            var userToken = GetCurrentUserToken();
            if (_voteRepository.hasUserVoted(userToken, postId))
            {
                return StatusCode(StatusCodes.Status403Forbidden, "User already voted");
            }


            
            post.upvotes++;
            _postRepository.UpdateUpvotes(postId, post.upvotes);

            var vote = new Vote
            {
                userToken = userToken,
                postId = postId
            };
            _voteRepository.AddVote(vote);
            return Json(new { upvotes = post.upvotes });


        }
        [HttpPost]

        public IActionResult Downvote(int postId)
        {
            var userToken = GetCurrentUserToken();
            var post = _postRepository.GetPostById(postId);
            if(post== null)
            {
                return NoContent();
            }
            if (_voteRepository.hasUserVoted(userToken, postId))
            {
                _voteRepository.RemoveVote(userToken, postId);
            }
            else
            {
                var vote = new Vote
                {
                    userToken = userToken,
                    postId = postId
                };
                _voteRepository.AddVote(vote);
                
                
            }
            var upvotes = _voteRepository.GetVoteCount(postId);
            upvotes--;
            post.upvotes = upvotes;
            return Json(new { upvotes = post.upvotes });
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
