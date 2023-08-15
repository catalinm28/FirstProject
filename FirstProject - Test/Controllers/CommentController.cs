using FirstProject___Test.Helpful;
using FirstProject___Test.ViewModels;
using FirstProjectRepository.DBModels;
using FirstProjectRepository.Repository;
using FirstProjectRepository.UsefullModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace FirstProject___Test.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentRepository _commentRepository;
        private readonly UserRepository _userRepository;
        public CommentController(CommentRepository commentRepository, UserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }   

        [HttpPost]
        public IActionResult AddComment(int postId, string commentText)
        {
            
            Guid userToken = UserTokenHelper.GetCurrentUserToken(HttpContext);

            
            Comment newComment = new Comment
            {
                postId = postId,
                userToken = userToken,
                text = commentText,
                createdAt = DateTime.UtcNow
            };

            string username;
            _commentRepository.InsertComment(newComment);
            User user = _userRepository.GetUserByUserToken(userToken);
            if (user == null) username = "anonim";
            else username = user.username;
            
            return Json(new {username = username,text = newComment.text,createdAt = TimeHelper.TimeAgo(newComment.createdAt)});

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
            
            Guid userToken = UserTokenHelper.GetCurrentUserToken(HttpContext);

            
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

            if (comment == null|| comment.userToken!=UserTokenHelper.GetCurrentUserToken(HttpContext))
            {
                return NotFound();
            }

           

            return View(comment);
        }

        [HttpPost]
        public IActionResult EditComment(int commentId, string editedText)
        {
            var comment = _commentRepository.GetCommentById(commentId);

            if (comment == null || comment.userToken != UserTokenHelper.GetCurrentUserToken(HttpContext))
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

            if (comment == null || comment.userToken != UserTokenHelper.GetCurrentUserToken(HttpContext))
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

            if (comment == null || comment.userToken != UserTokenHelper.GetCurrentUserToken(HttpContext))
            {
                return NotFound();
            }

            _commentRepository.DeleteComment(commentId);

            return RedirectToAction("Index", "Home");
        }
    
    }
}
