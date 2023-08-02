namespace FirstProject___Test.Models
{
    public class Post
    {
        public int postId { get; set; }
        public Guid userToken { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public DateTime createdAt { get; set; }
    }
}
