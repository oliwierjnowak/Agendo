using Agendo.Server.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace Agendo.Server.Persistance
{
    public interface IRightsRepository
    {
        Task<EmployeeRights> RightsOverEmp(int emp, int user);
        Task<IEnumerable<EmployeeRights>> RightsOverEmps(IEnumerable<int> emp, int user);
    }

    public class RightsRepository(IDbConnection _connection) : IRightsRepository
    {
        public async Task<EmployeeRights> RightsOverEmp(int emp, int user)
        {
            var query = @"select audoen_no as AuthType ,
                            audoen_do_no as Superior,
                            audoen_en_no as Emp,
                            auth.au_enabled as 'Enabled',
                            auth.au_from as 'FromDate',
                            auth.au_to as 'ToDate'

                            from csmd_authorizations_domain_entity
                            join csmd_authorizations auth on audoen_no = au_ri_no 
                            where auth.au_enabled = 1 and GETDATE() BETWEEN auth.au_from AND auth.au_to and
                            ((audoen_en_no = @emp and audoen_en_no = @user )or (audoen_do_no = @user and audoen_en_no = @emp) );";
            _connection.Open();
            var result = await _connection.QueryFirstOrDefaultAsync<EmployeeRights>(query,new
            {
                emp = emp,
                user = user 
            });
            _connection.Close();
            return result;
        }

        public async Task<IEnumerable<EmployeeRights>> RightsOverEmps(IEnumerable<int> emps, int user)
        {
            var query = @"select audoen_no as AuthType ,
                            audoen_do_no as Superior,
                            audoen_en_no as Emp,
                            auth.au_enabled as 'Enabled',
                            auth.au_from as 'FromDate',
                            auth.au_to as 'ToDate'

                            from csmd_authorizations_domain_entity
                            join csmd_authorizations auth on audoen_no = au_ri_no 
                            where auth.au_enabled = 1 and GETDATE() BETWEEN auth.au_from AND auth.au_to
                            and (audoen_do_no = @user and audoen_en_no IN @emps)";
            _connection.Open();
            var result = await _connection.QueryAsync<EmployeeRights>(query, new
            {
                emps = emps,
                user = user
            });
            _connection.Close();
            return result;
        }
    }
}
