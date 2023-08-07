using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.UsefullModels
{
    public class PostWithComments
    {
        public int postId { get; set; }
        public string title { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public string username { get; set; }
        public DateTime createdAt { get; set; }
        public int numberOfComments { get; set; }

        public List<CommentWithReplies> comments { get; set; }
    }
}
