using FirstProject___Test.Models;
using FirstProjectRepository.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstProjectRepository.DBModels;
using System.Drawing;
using System.Security.Claims;
using FirstProjectRepository.UsefullModels;
using FirstProjectRepository.Helpers;

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
            _voteRepository.RemoveVotesByPostId(id);

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
                if (_voteRepository.GetUserVoteType(userToken, postId) == (int)VoteType.Upvote)
                {

                    post.upvotes--;
                    _postRepository.UpdateUpvotes(postId, post.upvotes);
                    _voteRepository.RemoveVote(userToken, postId);
                }
                else
                {
                    post.downvotes--;
                    _postRepository.UpdateDownvotes(postId, post.downvotes);
                    _voteRepository.RemoveVote(userToken, postId);
                }
            }
            else
            {
                // User hasn't voted, so add the upvote
                post.upvotes++;
                _postRepository.UpdateUpvotes(postId, post.upvotes);
                var vote = new Vote
                {
                    userToken = userToken,
                    postId = postId,
                    voteType = VoteType.Upvote
                };
                _voteRepository.AddVote(vote);
            }
            int votecount = post.upvotes - post.downvotes;
            return Json(new { votecount = votecount });
        }

        [HttpPost]
        public IActionResult Downvote(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            if (post == null)
            {
                return NotFound();
            }

            var userToken = GetCurrentUserToken();
            if (_voteRepository.hasUserVoted(userToken, postId))
            {
                if (_voteRepository.GetUserVoteType(userToken, postId) == (int)VoteType.Downvote)
                {
                    post.downvotes--;
                    _postRepository.UpdateDownvotes(postId, post.downvotes);
                    _voteRepository.RemoveVote(userToken, postId);
                }
                else
                {
                    post.upvotes--;
                    _postRepository.UpdateUpvotes(postId, post.upvotes);
                    _voteRepository.RemoveVote(userToken, postId);
                }
            }
            else
            {
                // User hasn't voted, so add the downvote
                post.downvotes++;
                _postRepository.UpdateDownvotes(postId, post.downvotes);
                var vote = new Vote
                {
                    userToken = userToken,
                    postId = postId,
                    voteType = VoteType.Downvote
                };
                _voteRepository.AddVote(vote);
            }
            int votecount = post.upvotes - post.downvotes;

            return Json(new { votecount = votecount });
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

/*[HttpPost]
public IActionResult Downvote(int postId)
{
    var post = _postRepository.GetPostById(postId);
    if (post == null)
    {
        return NotFound();
    }

    var userToken = GetCurrentUserToken();
    if (_voteRepository.hasUserVoted(userToken, postId))
    {
        // User has already voted
        if (_voteRepository.GetUserVoteType(userToken, postId) == VoteType.Upvote)
        {
            // Remove the upvote
            post.upvotes--;
            _postRepository.UpdateUpvotes(postId, post.upvotes);
        }
        else
        {
            // Remove the downvote
            post.downvotes--;
            _postRepository.UpdateDownvotes(postId, post.downvotes);
        }

        _voteRepository.RemoveVote(userToken, postId);
    }
    else
    {
        // User hasn't voted, so add the downvote
        post.downvotes++;
        _postRepository.UpdateDownvotes(postId, post.downvotes);

        var vote = new Vote
        {
            userToken = userToken,
            postId = postId,
            voteType = VoteType.Downvote
        };
        _voteRepository.AddVote(vote);
    }

    return Json(new { downvotes = post.downvotes });
}
*/