using Dapper;
using FirstProjectRepository.DBModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstProjectRepository.Repository
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
            string procedureName = "GetUserByUserToken";

            var parameters = new { userToken };

            return _connection.QueryFirstOrDefault<User>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public User GetUserByUsername(string username)
        {
            string procedureName = "GetUserByUsername";

            var parameters = new { username };

            return _connection.QueryFirstOrDefault<User>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public User GetUserByEmail(string email)
        {
            string procedureName = "GetUserByEmail";

            var parameters = new { email };

            return _connection.QueryFirstOrDefault<User>(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public List<User> GetAllUsers()
        {
            string procedureName = "GetAllUsers";

            return _connection.Query<User>(procedureName, commandType: CommandType.StoredProcedure).ToList();
        }
        public void InsertUser(User user)
        {

            string procedureName = "InsertUser";
            var parameters = new
            {
                //userToken = user.userToken,
                username = user.username,
                email = user.email,
                password = user.password
            };

            _connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public void UpdateUser(User user)
        {
            string procedureName = "UpdateUser";
            var parameters = new
            {
                userToken = user.userToken,
                username = user.username,
                email = user.email,
                password = user.password
            };

            _connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        public void DeleteUser(int userToken)
        {
            string procedureName = "DeleteUserByUserToken";
            var parameters = new {userToken = userToken};
            _connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
        }
        
    }

}
