using Dapper;
using FirstProject___Test.Models;
using System.Data;
using System.Data.Sql;
using System.Text;

namespace FirstProject___Test.Repositories
{
    public class UserRepository
    {
        private readonly IDbConnection _connection;
        public UserRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public User GetUserByUserToken(Guid userToken)
        {
            string query = "SELECT * FROM Users WHERE userToken =@userToken;";
            return _connection.QueryFirstOrDefault<User>(query, new {userToken = userToken});
        }
        public User GetUserByUsername(string username)
        {
            string query = "SELECT * FROM Users WHERE username = @username;";
            return _connection.QueryFirstOrDefault<User>(query, new { username = username });
        }
        public User GetUserByEmail(string email)
        {
            string query = "SELECT * FROM Users WHERE email = @email;";
            return _connection.QueryFirstOrDefault<User>(query, new {email = email});
        }
        public List<User> GetAllUsers()
        {
            string query = "SELECT * FROM Users;";
            return _connection.Query<User>(query).ToList();
        }
        public void InsertUser(User user)
        {
            
            string query = "INSERT INTO Users(username,email, password) VALUES(@username,@email,@password)";
            _connection.Execute(query,user);
        }
        public void UpdateUser(User user)
        {
            string query = "UPDATE Users SET username = @username, email = @email, password = @password" +
                "WHERE userToken = @userToken;";
            _connection.Execute(query,user);
        }
        public void DeleteUser(int userToken)
        {
            string query = "DELETE FROM Users WHERE userToken = @userToken";
            _connection.Execute(query,new {userToken = userToken});
        }
        public string ComputeHash(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
