namespace FirstProject___Test.Models
{
    public class Comment
    {
        public int id { get; set; }
        public int? commentId { get; private set; }
        public int postId { get; private set; }
        public Guid userToken { get; private set; }
        public string text { get; set; }
        public DateTime createdAt { get; set; }
    }
}
