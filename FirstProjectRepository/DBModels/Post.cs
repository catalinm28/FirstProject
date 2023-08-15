using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.DBModels
{
    public class Post
    {
        public int postId { get; set; }
        public Guid userToken { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string? url { get; set; }
        public DateTime createdAt { get; set; }

        public int upvotes { get; set; }
        public int downvotes { get; set; }

        public int votecount { get; set; }
    }
}
