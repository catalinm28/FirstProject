using FirstProjectRepository.DBModels;
using FirstProjectRepository.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FirstProject___Test.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentRepository _commentRepository;
        public CommentController(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }   

        [HttpPost]
        public IActionResult AddComment(int postId, string commentText)
        {
            
            Guid userToken = GetCurrentUserToken();

            
            Comment newComment = new Comment
            {
                postId = postId,
                userToken = userToken,
                text = commentText,
                createdAt = DateTime.UtcNow
            };

            
            _commentRepository.InsertComment(newComment);
            return RedirectToAction("ViewPost", "Post", new { id = postId });
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
