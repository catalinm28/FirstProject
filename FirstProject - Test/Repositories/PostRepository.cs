using Dapper;
using FirstProject___Test.Joins;
using FirstProject___Test.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Globalization;
using System.Runtime.Intrinsics.X86;

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

            string query = "INSERT INTO Posts(userToken,title,text,url,createdAt) VALUES (@userToken,@title,@text,@url,@createdAt);";
            _connection.Execute(query, post);

        }
        public void UpdatePost(Post post)
        {
            string query = "UPDATE Posts SET userToken = @userToken, title = @title, text = @text, url = @url, createdAt = @createdAt WHERE postId = @postId;";
            _connection.Execute(query, post);
        }
        public void DeletePost(int id)
        {
            string query = "DELETE FROM Posts WHERE postId = @postId;";
            _connection.Execute(query, new { postId = id });
        }
        public List<PostUserJoin> GetPostsWithUsername()
        {
            string query = "SELECT p.postId,p.userToken, p.title, p.text, p.url, u.username, p.createdAt, " +
                       "(SELECT COUNT(*) FROM Comments c WHERE c.postId = p.postId) AS number_of_comments " +
                       "FROM Posts p " +
                       "JOIN Users u ON p.userToken = u.userToken";
            return _connection.Query<PostUserJoin>(query).ToList();
        }
        public List<PostUserJoin> GetAllPostsSortedByDate()
        {
            string query = "SELECT p.postId, p.title, p.text, p.url, p.createdAt, u.username FROM Posts p INNER JOIN Users u ON p.userToken = u.userToken ORDER BY p.createdAt DESC;";
            //(SELECT COUNT(*) FROM Comments c WHERE c.postId = p.postId) AS number_of_comments


            return _connection.Query<PostUserJoin>(query).ToList();
        }





    }
    }
