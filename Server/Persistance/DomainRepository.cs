using Agendo.Server.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.Common;

namespace Agendo.Server.Persistance
{
    public interface IDomainRepository
    {
        List<DomainDTO> GetAll(); 
    }

    public class DomainRepository : IDomainRepository
    {
        private readonly IConfiguration _config;

        private  IDbConnection _connection;

        public DomainRepository(IConfiguration config)
        {
            _config = config;

           // _connection = new DbConnection(_configuration.GetSection("AppSettings:Token").Value!);
        }

        public List<DomainDTO> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
