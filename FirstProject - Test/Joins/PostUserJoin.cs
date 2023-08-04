namespace FirstProject___Test.Joins
{
    public class PostUserJoin
    {
        public int postId { get; set; }
        public Guid userToken { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public string username { get; set; }
        public DateTime createdAt { get; set; }
        public int number_of_comments { get; set; }
    }
}
