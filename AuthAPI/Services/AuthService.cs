using Agendo.AuthAPI.Model;
using Agendo.Shared;
using Dapper;
using System.Data;

namespace Agendo.AuthAPI.Services
{
    public interface IAuthService
    {
        Task<int> Register(User user);
        Task<User> Login(string username);
    }

    public class AuthService : IAuthService
    {
        private readonly IDbConnection _connection;
        public AuthService(IDbConnection connection)
        {
                _connection = connection;
        }

        public async Task<int> Register(User user)
        {
            _connection.Open();
            var insert = @"insert into csmd_domain (do_name,do_password) values
                            (@Username,@PasswordHash)  ";
            var x = await _connection.ExecuteAsync(insert, user);
            _connection.Close();
            return x;
        }

        public async Task<User> Login(string username)
        {
            var query = @$"select do_name as Username ,do_password AS PasswordHash from csmd_domain 
                            where do_name = '{username}'";

            var x = await _connection.QuerySingleAsync<User>(query);

            return x;

        }
    }
}
