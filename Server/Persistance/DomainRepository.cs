using Agendo.Server.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;
using Dapper;

namespace Agendo.Server.Persistance
{
    public interface IDomainRepository
    {
        Task<List<DomainDTO>> GetAllAsync(); 
    }

    public class DomainRepository : IDomainRepository
    {

        private  IDbConnection _connection;

        public DomainRepository(IDbConnection connection)
        {
            _connection = connection;

        }

        public async Task<List<DomainDTO>> GetAllAsync()
        {
                _connection.Open();
            string selectQuery = "select * from [dbo].[csmd_domain]";
            IEnumerable<DomainDTO> data = await _connection.QueryAsync<DomainDTO>(selectQuery);
            return (List<DomainDTO>)data;
            
        }
    }
}
