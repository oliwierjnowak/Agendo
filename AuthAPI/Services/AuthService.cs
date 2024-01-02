﻿using Agendo.AuthAPI.Model;
using Agendo.Shared;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Agendo.AuthAPI.Services
{
    public interface IAuthService
    {
        Task<int> Register(User user);
        Task<User> Login(string username);
    }

    public class AuthService(SqlConnection _connection) : IAuthService
    {

        public async Task<int> Register(User user)
        {
            await _connection.OpenAsync();
            var insert = @"insert into csmd_domain (do_name,do_password) values
                            (@Username,@PasswordHash)  ";
            var x = await _connection.ExecuteAsync(insert, user);
            await _connection.CloseAsync();
            return x;
        }

        public async Task<User> Login(string username)
        {
            var query = @$"select do_name as Username, do_password AS PasswordHash, 
                            audoen_no as Role,
                            audoen_do_no as SuperiorID,
                            audoen_en_no as EmpID
                            from csmd_domain 
                            join csmd_authorizations_domain_entity auth on auth.audoen_en_no = do_no 
                            where do_name = '{username}'";
            await _connection.OpenAsync();
            var x = await _connection.QuerySingleAsync<User>(query);
           await  _connection.CloseAsync();
            return x;

        }
    }
}
