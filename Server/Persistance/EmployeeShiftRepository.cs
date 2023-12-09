﻿using Agendo.Server.Models;
using Dapper;
using System.Data;

namespace Agendo.Server.Persistance
{
    public interface IEmployeeShiftRepository
    {
        Task<int> CreateShift(EmployeeShift employeeShift);
        Task<List<EmployeeShift>> GetMultipleEmpsAsync(int superior,IEnumerable<int> emps);
        Task<List<EmployeeShift>> GetSingleEmpAsync(int superior,int emp);
    }
    public class EmployeeShiftRepository(IDbConnection _connection) : IEmployeeShiftRepository
    {		
        public async Task<int> CreateShift(EmployeeShift employeeShift)
        {
			var dow = "";

			switch (employeeShift.DOW)
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
			// isnt checking if the dosh_monday for example already has value
            string query = $@"SELECT COUNT(*) FROM dbo.csti_do_shift 
							WHERE dosh_do_no = @EmpNr and dosh_week_number = @ISOWeek and dosh_year = @ISOYear";
            var exists = _connection.QueryFirst<int>(query, employeeShift);

			if (exists ==  0)
			{
				var day = employeeShift.DOW;
                // insert
                string insert = @$"
insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday] ) 
values (@EmpNr, @ISOWeek, @ISOYear, {(day == 1 ? "@ShiftNR" : "1")},
									{(day == 2 ? "@ShiftNR" : "1")},
									{(day == 3 ? "@ShiftNR" : "1")},
									{(day == 4 ? "@ShiftNR" : "1")},
									{(day == 5 ? "@ShiftNR" : "1")},
									{(day == 6 ? "@ShiftNR" : "1")},
									{(day == 7 ? "@ShiftNR" : "1")});";
               
                var x = await _connection.ExecuteAsync(insert,employeeShift);
               
                return x;
            }
			else
			{
                //update
                string insert = @$"update [dbo].[csti_do_shift] 
									set  dosh_week_number = @ISOWeek, dosh_year = @ISOYear, [{dow}] = @ShiftNR
									where
									dosh_do_no = @EmpNr and dosh_week_number = @ISOWeek and dosh_year = @ISOYear";

                var x = await _connection.ExecuteAsync(insert, employeeShift);
				return x;
            }

        }

        public async Task<List<EmployeeShift>> GetMultipleEmpsAsync(int superior,IEnumerable<int> emps)
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
            return (List<EmployeeShift>)data;
        }

        public async Task<List<EmployeeShift>> GetSingleEmpAsync(int superior,int emp)
        {
            string authjoins = $@"
								join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
								join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no";
            string authwhere = $@" and authdomain.audoen_en_no = @emp and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1";


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
            return (List<EmployeeShift>)data;
        }
    }
}
