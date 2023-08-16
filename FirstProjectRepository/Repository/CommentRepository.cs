using Dapper;
using FirstProjectRepository.DBModels;
using FirstProjectRepository.UsefullModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.Repository
{
    public class CommentRepository
    {
        private readonly IDbConnection _connection;
        public CommentRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public Comment GetCommentById(int id)
        {
            string procedureName = "GetCommentById";
            var parameters = new { id = id };
            return _connection.QueryFirstOrDefault<Comment>(procedureName, parameters, commandType: CommandType.StoredProcedure);

        }
        public Comment GetCommentByCommentId(int commentId)
        {
            var parameters = new { commentId };

            string procedureName = "GetCommentByCommentId";
            return _connection.QueryFirstOrDefault<Comment>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public List<Comment> GetAllComments()
        {
            string procedureName = "GetAllComments";
            return _connection.Query<Comment>(procedureName, commandType: CommandType.StoredProcedure).ToList();
        }
        public void InsertComment(Comment comment)
        {
            if (comment.userToken == Guid.Empty)
            {

                comment.userToken = new Guid("1E2DD3E0-0947-4EBB-AB4B-88234500A08B");
            }
            var parameters = new
            {
                commentId = comment.commentId,
                postId = comment.postId,
                userToken = comment.userToken,
                text = comment.text,
                createdAt = comment.createdAt
            };

            string procedureName = "InsertComment";
            _connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public void UpdateComment(Comment comment)
        {
            var parameters = new
            {
                id = comment.id,
                text = comment.text
            };

            string procedureName = "UpdateComment";
            _connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public void DeleteComment(int id)
        {
            var parameters = new { id };

            string procedureName = "DeleteComment";
            _connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public List<CommentWithReplies> GetPostWithCommentsAndReplies(int postId)
        {
            string procedureName = "GetPostWithCommentsAndReplies";
            var parameters = new { postId };
            return _connection.Query<CommentWithReplies>(procedureName, parameters, commandType: CommandType.StoredProcedure).ToList();


        }
        public void DeleteCommentsByPostId(int postId)
        {
            string procedureName = "DeleteCommentsByPostId";
            var parameter = new { postId = postId };
            _connection.Execute(procedureName, parameter, commandType: CommandType.StoredProcedure);
            
        }
    }
}
