using Dapper;
using FirstProjectRepository.DBModels;
using FirstProjectRepository.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.Repository
{
    public class VoteRepository
    {
        private readonly IDbConnection _connection;
        public VoteRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public bool hasUserVoted(Guid userToken, int postId)
        {
            string procedure = "CheckUserVote";
            var parameters = new { userToken = userToken, postId = postId };
            int count = _connection.ExecuteScalar<int>(procedure, parameters, commandType: CommandType.StoredProcedure);
            if (count > 0)
            {
                return true;
            }
            else return false;
        }
        public void AddVote(Vote vote)
        {
            string procedure = "InsertVote";
            var parameters = new
            {
                userToken = vote.userToken,
                postId = vote.postId,
                voteType = vote.voteType
            };
            _connection.Execute(procedure,parameters, commandType: CommandType.StoredProcedure);
        }
        public int GetVoteCount(int postId)
        {
            string procedure = "GetVoteCount";
            var parameter = new { postId = postId };
            int count = _connection.ExecuteScalar<int>(procedure, parameter, commandType: CommandType.StoredProcedure);
            return count;
        }
   


        public void RemoveVote(Guid userToken, int postId)
        {
            string sql = "DELETE FROM Votes WHERE userToken = @userToken AND PostId = @PostId";
            _connection.Execute(sql, new { userToken = userToken, PostId = postId });
        }
        public void RemoveVotesByPostId(int postId)
        {
            string sql = "DELETE FROM Votes WHERE postId = @postId";
            _connection.Execute(sql, new { postId = postId });
        }
        public int GetUserVoteType(Guid userToken, int postId)
        {
            string sql = "SELECT VoteType FROM Votes WHERE userToken = @userToken AND postId = @postId";
            int voteType = _connection.ExecuteScalar<int>(sql, new { userToken = userToken, postId = postId });
            return voteType;
        }
    }
}
