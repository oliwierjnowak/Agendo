using Agendo.Server.Models;
using Dapper;
using System.Data;

namespace Agendo.Server.Persistance
{
    public interface IEmployeeShiftRepository
    {
        Task<List<EmployeeShift>> GetAllAsync();
        Task<List<EmployeeShift>> GetSingleEmpAsync(int Emp);
    }
    public class EmployeeShiftRepository : IEmployeeShiftRepository
    {
        private IDbConnection _connection;
		

        public EmployeeShiftRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<EmployeeShift>> GetAllAsync()
        {
            string shiftOverview = @$"
    select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 1 )as 'DayOfWeek',dosh_monday as 'ShiftNR'  , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_monday = EmpShift.ds_no
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 2 )as 'DayOfWeek',dosh_tuesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_tuesday = EmpShift.ds_no
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 3 )as 'DayOfWeek',dosh_wednesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_wednesday = EmpShift.ds_no
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 4 )as 'DayOfWeek',dosh_thursday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_thursday = EmpShift.ds_no	
	union all


	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 5 )as 'DayOfWeek',dosh_friday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours' 	
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_friday = EmpShift.ds_no		
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 6 )as 'DayOfWeek',dosh_saturday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_saturday = EmpShift.ds_no	
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 6 )as 'DayOfWeek',dosh_sunday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_sunday = EmpShift.ds_no";
            _connection.Open();
            string selectQuery = shiftOverview;
            IEnumerable<EmployeeShift> data = await _connection.QueryAsync<EmployeeShift>(selectQuery);
            _connection.Close();
            return (List<EmployeeShift>)data;
        }
        public async Task<List<EmployeeShift>> GetSingleEmpAsync(int Emp)
        {
            string shiftOverview = @$"
    select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 1 )as 'DayOfWeek',dosh_monday as 'ShiftNR'  , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_monday = EmpShift.ds_no where dosh_do_no = {Emp}
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 2 )as 'DayOfWeek',dosh_tuesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_tuesday = EmpShift.ds_no where dosh_do_no = {Emp}
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 3 )as 'DayOfWeek',dosh_wednesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_wednesday = EmpShift.ds_no where dosh_do_no = {Emp}
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 4 )as 'DayOfWeek',dosh_thursday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_thursday = EmpShift.ds_no where dosh_do_no = {Emp}
	union all


	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 5 )as 'DayOfWeek',dosh_friday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours' 	
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_friday = EmpShift.ds_no where dosh_do_no = {Emp}		
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 6 )as 'DayOfWeek',dosh_saturday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_saturday = EmpShift.ds_no	where dosh_do_no = {Emp}
	union all

	select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 6 )as 'DayOfWeek',dosh_sunday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
	from [dbo].[csti_do_shift] 
	join [dbo].[csti_daily_schedule] as EmpShift on dosh_sunday = EmpShift.ds_no where dosh_do_no = {Emp}";
            _connection.Open();
            string selectQuery = shiftOverview;
            IEnumerable<EmployeeShift> data = await _connection.QueryAsync<EmployeeShift>(selectQuery);
            _connection.Close();
            return (List<EmployeeShift>)data;
        }
    }
}
