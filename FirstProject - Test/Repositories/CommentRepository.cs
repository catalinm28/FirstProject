using Dapper;
using FirstProject___Test.Models;
using Microsoft.Extensions.Hosting;
using System.Data;

namespace FirstProject___Test.Repositories
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
            string query = "SELECT * FROM Comments WHERE id = @id;";
            return _connection.QueryFirstOrDefault<Comment>(query);
        }
        public List<Comment> GetAllComments()
        {
            string query = "SELECT * FROM Comments;";
            return _connection.Query<Comment>(query).ToList();
        }
        public void InsertComment(Comment comment)
        {
            string query = "INSERT INTO Comments(commentId,postId,userToken,text,createdAt) VALUES (@commentId,@postId,@userToken,@text,@createdAt);";
            _connection.Execute(query, comment);
        }
        public void UpdateComment(Comment comment)
        {
            string query = "UPDATE Comments SET text  = @text WHERE id = @id;";
            _connection.Execute(query, comment);
        }
        public void DeleteComment(int id)
        {
            string query = "DELETE FROM Comments WHERE id = @id;";
            _connection.Execute(query, id);
        }
      
    }
}
