using Dapper;
using System.Data;
using Agendo.Server.Models;

namespace Agendo.Server.Persistance
{
    public interface IDomainRepository
    {
        Task<List<DomainDTO>> GetAllAsync(int superior);
        Task<List<DomainDTO>> GetListAsync(int superior, IEnumerable<int> domains);
    }

    public class DomainRepository(IDbConnection _connection) : IDomainRepository
    {
        
        public async Task<List<DomainDTO>> GetAllAsync(int superior)
        {
            _connection.Open();
            string selectQuery = @"select do_no AS 'Nr',do_name AS 'Name'   from [dbo].[csmd_domain]
                                    join csmd_authorizations_domain_entity auth on auth.audoen_en_no = do_no
                                    where audoen_do_no = @superior;";
            IEnumerable<DomainDTO> data = await _connection.QueryAsync<DomainDTO>(selectQuery, new
            {
                superior = superior
            });
            _connection.Close();
            return (List<DomainDTO>)data;
            
        }

        public async Task<List<DomainDTO>> GetListAsync(int superior, IEnumerable<int> domains)
        {
            _connection.Open();
            string selectQuery = @"select do_no AS 'Nr',do_name AS 'Name'   from [dbo].[csmd_domain]
                                    join csmd_authorizations_domain_entity auth on auth.audoen_en_no = do_no
                                    where audoen_do_no = @superior and do_no in @domains ;";
            IEnumerable<DomainDTO> data = await _connection.QueryAsync<DomainDTO>(selectQuery, new
            {
                superior = superior,
                domains = domains
            });
            _connection.Close();
            return (List<DomainDTO>)data;
        }
    }
}
