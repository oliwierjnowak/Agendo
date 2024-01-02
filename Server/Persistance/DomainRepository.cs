using Dapper;
using System.Data;
using Agendo.Shared.DTOs;
using System.Data.SqlClient;

namespace Agendo.Server.Persistance
{
    public interface IDomainRepository
    {
        Task<List<DomainDTO>> GetAllAsync(int superior);
        Task<List<DomainDTO>> GetListAsync(int superior, IEnumerable<int> domains);
        Task<IEnumerable<DomainDTO>> GetShiftEmployees(int user, int year, int isoWeekNumber, DayOfWeek dayOfWeek, int shiftNR);
    }

    public class DomainRepository(SqlConnection _connection) : IDomainRepository
    {
        
        public async Task<List<DomainDTO>> GetAllAsync(int superior)
        {
            await _connection.OpenAsync();
            string selectQuery = @"select do_no AS 'Nr',do_name AS 'Name'   from [dbo].[csmd_domain]
                                    join csmd_authorizations_domain_entity auth on auth.audoen_en_no = do_no
                                    where audoen_do_no = @superior;";
            IEnumerable<DomainDTO> data = await _connection.QueryAsync<DomainDTO>(selectQuery, new
            {
                superior = superior
            });
            await _connection.CloseAsync();
            return (List<DomainDTO>)data;
            
        }
        public async Task<List<DomainDTO>> GetListAsync(int superior, IEnumerable<int> domains)
        {
            await _connection.OpenAsync();
            string selectQuery = @"select do_no AS 'Nr',do_name AS 'Name'   from [dbo].[csmd_domain]
                                    join csmd_authorizations_domain_entity auth on auth.audoen_en_no = do_no
                                    where audoen_do_no = @superior and do_no in @domains ;";
            IEnumerable<DomainDTO> data = await _connection.QueryAsync<DomainDTO>(selectQuery, new
            {
                superior = superior,
                domains = domains
            });
            await _connection.CloseAsync();
            return (List<DomainDTO>)data;
        }

        public async Task<IEnumerable<DomainDTO>> GetShiftEmployees(int user, int year, int isoWeekNumber, DayOfWeek dayOfWeek, int shiftNR)
        {

            var dow = "";

            switch ((int)dayOfWeek)
            {
                case 1:
                    dow = "dosh_monday";
                    break;
                case 2:
                    dow = "dosh_tuesday";
                    break;
                case 3:
                    dow = "dosh_wednesday";
                    break;
                case 4:
                    dow = "dosh_thursday";
                    break;
                case 5:
                    dow = "dosh_friday";
                    break;
                case 6:
                    dow = "dosh_saturday";
                    break;
                case 0:
                    dow = "dosh_sunday";
                    break;
            }
            var selectquery = $@"select do_no AS 'Nr',do_name AS 'Name' from csti_do_shift 
                                    join csmd_domain on dosh_do_no = do_no
                                    join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
                                    join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no
                                    where dosh_year = @dyear AND {dow} = @shift AND dosh_week_number = @dwn  
                                    and  audoen_do_no = @user and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1";
            await _connection.OpenAsync();
            IEnumerable<DomainDTO> data = await _connection.QueryAsync<DomainDTO>(selectquery, new
            {
                dyear = year,
                shift = shiftNR,
                dwn = isoWeekNumber,
                user = user
            });
            await _connection.CloseAsync();
            return data;


        }
    }
}
