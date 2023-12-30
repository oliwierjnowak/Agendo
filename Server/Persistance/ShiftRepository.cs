using Agendo.Server.Models;
using Dapper;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Agendo.Server.Persistance
{
    public interface IShiftRepository
    {

        Task<IEnumerable<EmployeeShift>> GetMultipleEmpsAsync(int superior,IEnumerable<int> emps);
        Task<IEnumerable<EmployeeShift>> GetSingleEmpAsync(int superior,int emp);
        Task<IEnumerable<EmployeeShift>> ManageEmployeesShift(int superior,IEnumerable<int> domains, Shift shift);
		Task<int> DeleteEmployeesShift(int superior, IEnumerable<int> remove, Shift shift);
    }
    public class ShiftRepository(IDbConnection _connection, IRightsRepository _rightsRepository) : IShiftRepository
    {		

        public async Task<IEnumerable<EmployeeShift>> GetMultipleEmpsAsync(int superior,IEnumerable<int> emps)
        {
			string authjoins = $@"
								join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
								join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no";
			string authwhere = $@" and authdomain.audoen_en_no in @emps and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1";

            string shiftOverview = @$"
								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 1) as 'DOW',dosh_monday as 'ShiftNR'  , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_monday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_monday !=  1 {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 2) as 'DOW',dosh_tuesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_tuesday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_tuesday !=  1 {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 3) as 'DOW',dosh_wednesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_wednesday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_wednesday !=  1 {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 4) as 'DOW',dosh_thursday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_thursday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_thursday !=  1 {authwhere}
								union all


								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 5) as 'DOW',dosh_friday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours' 	
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_friday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_friday !=  1	{authwhere}	
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 6) as 'DOW',dosh_saturday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_saturday = EmpShift.ds_no	
								{authjoins}
								where dosh_do_no in @emps and dosh_saturday !=  1 {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 7) as 'DOW',dosh_sunday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_sunday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_sunday !=  1 {authwhere}";
            _connection.Open();
            string selectQuery = shiftOverview;
            IEnumerable<EmployeeShift> data = await _connection.QueryAsync<EmployeeShift>(selectQuery, new
			{
				emps = emps,
				superior = superior
			});
            _connection.Close();
            return data;
        }

        public async Task<IEnumerable<EmployeeShift>> GetSingleEmpAsync(int superior,int emp)
        {
            string authjoins = $@"
								join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
								join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no";
            string authwhere = $@" and ((authdomain.audoen_en_no = @emp and audoen_do_no = @superior ) or @superior = @emp) and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1";


            string shiftOverview = @$"
								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 1) as 'DOW',dosh_monday as 'ShiftNR'  , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_monday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no = @emp and dosh_monday !=  1 {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 2) as 'DOW',dosh_wednesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_wednesday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no = @emp and dosh_tuesday !=  1 {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 3) as 'DOW',dosh_wednesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_wednesday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no = @emp and dosh_wednesday !=  1 {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 4) as 'DOW',dosh_thursday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_thursday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no = @emp and dosh_thursday !=  1 {authwhere}
								union all


								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 5) as 'DOW',dosh_friday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours' 	
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_friday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no = @emp and dosh_friday !=  1	{authwhere}	
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 6) as 'DOW',dosh_saturday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_saturday = EmpShift.ds_no	
								{authjoins}
								where dosh_do_no = @emp and dosh_saturday !=  1 {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 7) as 'DOW',dosh_sunday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_sunday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no = @emp and dosh_sunday !=  1 {authwhere}";
            _connection.Open();
            string selectQuery = shiftOverview;
            IEnumerable<EmployeeShift> data = await _connection.QueryAsync<EmployeeShift>(selectQuery, new
            {
                emp = emp,
                superior = superior
            });
            _connection.Close();
            return data;
        }

        public async Task<IEnumerable<EmployeeShift>> ManageEmployeesShift(int superior,IEnumerable<int> domains, Shift shift)
        {
            var right = await _rightsRepository.RightsOverEmps(domains, superior);
            if(right.Count() != domains.Count())
            {
                // superior has in requested domains some domains that do not belong to him
                return Enumerable.Empty<EmployeeShift>();
            }

            var dow = "";

            switch (shift.DOW)
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
                case 7:
                    dow = "dosh_sunday";
                    break;
            }



			string checkExistence = $@"
select dosh_do_no from csti_do_shift 
join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no
where dosh_week_number = @ISOWeek and dosh_year = @ISOYear
and authdomain.audoen_en_no in @emps and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1
";
            _connection.Open();
            var existence = await _connection.QueryAsync<int>(checkExistence, new
            {
                emps = domains,
                superior = superior,
				ISOYear = shift.ISOYear,
				ISOWeek = shift.ISOWeek
            });
            _connection.Close();

            var updatedResult = Enumerable.Empty<EmployeeShift>();
            if (existence.Count() > 0)
            {
                string update = $@"
			update s
			set  dosh_week_number = @ISOWeek, dosh_year = @ISOYear, [{dow}] = @ShiftNR
			OUTPUT inserted.dosh_do_no as 'EmpNR', inserted.dosh_week_number as 'ISOWeek',inserted.dosh_year as 'ISOYear',inserted.dosh_monday as 'ShiftNR',{shift.DOW} as 'DOW'
			FROM [dbo].[csti_do_shift] s
				 join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
				 join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no
			where
			 dosh_week_number = @ISOWeek and dosh_year = @ISOYear
			 and authdomain.audoen_en_no in @existence and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1
			";
                _connection.Open();
                 updatedResult = await _connection.QueryAsync<EmployeeShift>(update, new
                {
                    existence = existence,
                    superior = superior,
                    ISOYear = shift.ISOYear,
                    ShiftNR = shift.ShiftNR,
                    ISOWeek = shift.ISOWeek
                });
                _connection.Close();
            }

            IEnumerable<int> haveToBeCreated = domains.Except(existence);

          

			if (haveToBeCreated.Count() > 0)
			{
                
                string insert = @$"
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday] ) 
OUTPUT inserted.dosh_do_no as 'EmpNR', inserted.dosh_week_number as 'ISOWeek',inserted.dosh_year as 'ISOYear',inserted.{dow} as 'ShiftNR', {shift.DOW} as 'DOW' 
values 
";

                string values = "";
                foreach (var x in haveToBeCreated)
                {
                    values += $@" ({x}, @ISOWeek, @ISOYear, {(shift.DOW == 1 ? "@ShiftNR" : "1")},
									{(shift.DOW == 2 ? "@ShiftNR" : "1")},
									{(shift.DOW == 3 ? "@ShiftNR" : "1")},
									{(shift.DOW == 4 ? "@ShiftNR" : "1")},
									{(shift.DOW == 5 ? "@ShiftNR" : "1")},
									{(shift.DOW == 6 ? "@ShiftNR" : "1")},
									{(shift.DOW == 7 ? "@ShiftNR" : "1")}),";

                }

                insert += values[..^1];
                _connection.Open();
                var insertResult = await _connection.QueryAsync<EmployeeShift>(insert, new
                {
                    ISOYear = shift.ISOYear,
                    ShiftNR = shift.ShiftNR,
                    ISOWeek = shift.ISOWeek

                }
                 );
                _connection.Close();
                return insertResult.Concat(updatedResult);
			}
			else
			{
				return updatedResult;

            }

        }

        public async Task<int> DeleteEmployeesShift(int superior, IEnumerable<int> remove, Shift shift)
        {
            var dow = "";

            switch (shift.DOW)
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
                case 7:
                    dow = "dosh_sunday";
                    break;
            }



            string checkExistence = $@"
select dosh_do_no from csti_do_shift 
join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no
where dosh_week_number = @ISOWeek and dosh_year = @ISOYear and {dow} = @ShiftNR
and authdomain.audoen_en_no in @emps and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1
";			_connection.Open();
            var existence = await _connection.QueryAsync<int>(checkExistence, new
            {
                emps = remove,
                superior = superior,
                ISOYear = shift.ISOYear,
                ShiftNR = shift.ShiftNR,
                ISOWeek = shift.ISOWeek
            });
            _connection.Close();
            string update = $@"
			update s
			set  dosh_week_number = @ISOWeek, dosh_year = @ISOYear, [{dow}] = 1
			OUTPUT inserted.dosh_do_no as 'EmpNR', inserted.dosh_week_number as 'ISOWeek',inserted.dosh_year as 'ISOYear',inserted.dosh_monday as 'ShiftNR'
			FROM [dbo].[csti_do_shift] s
				 join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
				 join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no
			where
			 dosh_week_number = @ISOWeek and dosh_year = @ISOYear
			 and authdomain.audoen_en_no in @existence and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1
			";
            _connection.Open();
            var updatedResult = await _connection.ExecuteAsync(update, new
            {
                existence = existence,
                superior = superior,
                ISOYear = shift.ISOYear,
                ShiftNR = shift.ShiftNR,
                ISOWeek = shift.ISOWeek
            });
            _connection.Close();
            return updatedResult;

        
        }
    }
}
