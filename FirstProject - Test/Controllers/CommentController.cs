using FirstProject___Test.ViewModels;
using FirstProjectRepository.DBModels;
using FirstProjectRepository.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
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
            return Json(newComment);
        }
        [HttpGet]
        public IActionResult AddReply(int postId,int parentCommentId)
        {
            var ViewModel = new AddReplyViewModel
            {
                postId = postId,
                parentCommentId = parentCommentId
            };
            return View(ViewModel);
        }
        [HttpPost]
        public IActionResult AddReply(int postId, int parentCommentId, string replyText)
        {
            
            Guid userToken = GetCurrentUserToken();

            
            var reply = new Comment
            {
                postId = postId,
                commentId = parentCommentId,
                userToken = userToken,
                text = replyText,
                createdAt = DateTime.UtcNow
            };

            
            _commentRepository.InsertComment(reply);

            
            return RedirectToAction("ViewPost","Post", new { id = postId });
        }
        public IActionResult EditComment(int commentId)
        {
            var comment = _commentRepository.GetCommentById(commentId);

            if (comment == null|| comment.userToken!=GetCurrentUserToken())
            {
                return NotFound();
            }

           

            return View(comment);
        }

        [HttpPost]
        public IActionResult EditComment(int commentId, string editedText)
        {
            var comment = _commentRepository.GetCommentById(commentId);

            if (comment == null || comment.userToken != GetCurrentUserToken())
            {
                return NotFound();
            }

            
            

            // Update the comment text
            comment.text = editedText;
            _commentRepository.UpdateComment(comment);

            return RedirectToAction("ViewPost", "Post", new { id = comment.postId });
        }
        [Authorize]
        public IActionResult Delete(int commentId)
        {
            var comment = _commentRepository.GetCommentById(commentId);

            if (comment == null || comment.userToken != GetCurrentUserToken())
            {
                return NotFound();
            }

            return View(comment);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public IActionResult ConfirmDelete(int commentId)
        {
            var comment = _commentRepository.GetCommentById(commentId);

            if (comment == null || comment.userToken != GetCurrentUserToken())
            {
                return NotFound();
            }

            _commentRepository.DeleteComment(commentId);

            return RedirectToAction("Index", "Home");
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
