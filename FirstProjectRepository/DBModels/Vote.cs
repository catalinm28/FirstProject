using FirstProjectRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.DBModels
{
    public class Vote
    {
        public int VoteId { get; set; }
        public Guid userToken { get; set; }
        public int postId { get; set; }
        
        public VoteType voteType { get; set; }
    }
}
