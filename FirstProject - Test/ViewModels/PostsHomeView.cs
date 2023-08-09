using FirstProjectRepository.DBModels;
using FirstProjectRepository.UsefullModels;

namespace FirstProject___Test.ViewModels
{
    public class PostsHomeView
    {
        public int postId { get; set; }
        public Guid userToken { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public string username { get; set; }
        public DateTime createdAt { get; set; }
        public int number_of_comments { get; set; }

        public List<CommentWithReplies> comments { get; set; }   

        public int upvotes { get; set; }
    }
}
