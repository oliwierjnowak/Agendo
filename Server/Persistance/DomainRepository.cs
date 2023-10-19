using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;
using System.Xml.Linq;
using Dapper;
using Agendo.Server.Models;

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
            string selectQuery = "select do_no AS 'Nr',do_name AS 'Name'   from [dbo].[csmd_domain]";
            IEnumerable<DomainDTO> data = await _connection.QueryAsync<DomainDTO>(selectQuery);
            _connection.Close();
            return (List<DomainDTO>)data;
            
        }
    }
}
