using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.UsefullModels
{
    public class CommentWithReplies
    {
        public int id { get; set; }
        public int? commentId { get; set; }
        
        public int postId { get; set; }
       
        public Guid userToken { get; set; }
        public string username { get; set; }
        public string text { get; set; }
        public DateTime createdAt { get; set; }
        public int number_of_replies { get; set; }

        public List<CommentWithReplies> replies { get; set; }
    }
}
