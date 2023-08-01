using Dapper;
using FirstProject___Test.Models;
using System.Data;
using System.Data.Sql;

namespace FirstProject___Test.Repositories
{
    public class PostRepository
    {
        private readonly IDbConnection _connection;

        public PostRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public Post GetPostById(int id)
        {
            string query = "SELECT * FROM Posts WHERE postId ='" + id + "';";
            return _connection.QueryFirstOrDefault<Post>(query, new { postId = id });
        }
        public Post GetPostByUserId(int userToken)
        {
            string query = "SELECT * FROM Posts WHERE userToken='" + userToken + "';";
            return _connection.QueryFirstOrDefault<Post>(query);
        }
        public List<Post> GetAllPosts()
        {
            string query = "SELECT * FROM Posts;";
            return _connection.Query<Post>(query).ToList();
        }
        public void InsertPost(Post post)
        {
            string query = "INSERT INTO Posts(userToken,title,text,url,createdAt) VALUES (@userToken,@title,@text,@createdAt,@url);";
            _connection.Execute(query, post);
        }
        public void UpdatePost(Post post)
        {
            string query = "UPDATE Posts SET userToken = @userToken, title = @title, text = @text, url = @url, createdAt = @createdAt;";
            _connection.Execute(query, post);
        }
        public void DeletePost(int id)
        {
            string query = "DELETE FROM Posts WHERE postId = @postId;";
            _connection.Execute(query, id);
        }
    }
}
