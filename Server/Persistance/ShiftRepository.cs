using Agendo.Client.Pages.Shifts;
using Agendo.Server.Models;
using Agendo.Shared.Form;
using Dapper;
using System.Data;

namespace Agendo.Server.Persistance
{
    public interface IShiftRepository
    {

        Task<IEnumerable<EmployeeShift>> GetMultipleEmpsAsync(int superior,IEnumerable<int> emps, List<int> weeks,int year);
        Task<IEnumerable<EmployeeShift>> GetSingleEmpAsync(int superior,int emp);
        Task<IEnumerable<EmployeeShift>> ManageEmployeesShift(int superior,IEnumerable<int> domains, Shift shift);
		Task<int> DeleteEmployeesShift(int superior, IEnumerable<int> remove, Shift shift);

        Task DaySequenceCreate(int superior, SequenceForm sequence);
        Task DatesSequenceCreate(int superior, MultipleSelectionForm sequence, IEnumerable<DayWeekYear> dayWeekYears);
    }
    public class ShiftRepository(IDbConnection _connection, IRightsRepository _rightsRepository) : IShiftRepository
    {

        public async Task<IEnumerable<EmployeeShift>> GetMultipleEmpsAsync(int superior, IEnumerable<int> emps, List<int> weeks, int year)
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
								where dosh_do_no in @emps and dosh_monday !=  1 and dosh_week_number in @weeks and dosh_year = @year {authwhere} 
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 2) as 'DOW',dosh_tuesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_tuesday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_tuesday !=  1 and dosh_week_number in @weeks and dosh_year = @year {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 3) as 'DOW',dosh_wednesday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_wednesday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_wednesday !=  1 and dosh_week_number in @weeks and dosh_year = @year {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 4) as 'DOW',dosh_thursday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_thursday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_thursday !=  1 and dosh_week_number in @weeks and dosh_year = @year {authwhere}
								union all


								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 5) as 'DOW',dosh_friday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours' 	
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_friday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_friday !=  1 and dosh_week_number in @weeks and dosh_year = @year	{authwhere}	
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 6) as 'DOW',dosh_saturday as 'ShiftNR' , EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_saturday = EmpShift.ds_no	
								{authjoins}
								where dosh_do_no in @emps and dosh_saturday !=  1 and dosh_week_number in @weeks and dosh_year = @year {authwhere}
								union all

								select dosh_do_no as 'EmpNR', dosh_week_number as 'ISOWeek', dosh_year as 'ISOYear', (Select 7) as 'DOW',dosh_sunday as 'ShiftNR', EmpShift.ds_name as 'ShiftName' , EmpShift.ds_hours as 'ShiftHours'
								from [dbo].[csti_do_shift] 
								join [dbo].[csti_daily_schedule] as EmpShift on dosh_sunday = EmpShift.ds_no 
								{authjoins}
								where dosh_do_no in @emps and dosh_sunday !=  1 and dosh_week_number in @weeks and dosh_year = @year {authwhere}";
            _connection.Open();
            string selectQuery = shiftOverview;
            IEnumerable<EmployeeShift> data = await _connection.QueryAsync<EmployeeShift>(selectQuery, new
            {
                emps = emps,
                superior = superior,
                weeks = weeks,
                year = year
            });
            _connection.Close();
            return data;
        }

        public async Task<IEnumerable<EmployeeShift>> GetSingleEmpAsync(int superior, int emp)
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

        public async Task<IEnumerable<EmployeeShift>> ManageEmployeesShift(int superior, IEnumerable<int> domains, Shift shift)
        {
            var right = await _rightsRepository.RightsOverEmps(domains, superior);
            if (right.Count() != domains.Count())
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
"; _connection.Open();
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

        public async Task DaySequenceCreate(int superior, SequenceForm sequence)
        {
            var right = (await _rightsRepository.RightsOverEmps(sequence.DomainsIDs, superior)).Select(x => x.Emp).ToList();
            sequence.DomainsIDs.Sort();
            right.Sort();
            if (!right.SequenceEqual(sequence.DomainsIDs))
            {
                throw new InvalidOperationException("user requested access to not owned domains");
            }



            string checkExistence = $@"
            select dosh_do_no, dosh_week_number from csti_do_shift 
            join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
            join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no
            where (dosh_week_number BETWEEN  @ISOWeekFrom  and  @ISOWeekTo) and dosh_year = @ISOYear
            and authdomain.audoen_en_no in @emps and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1
            ";
            _connection.Open();
            var existence = await _connection.QueryAsync<(int, int)>(checkExistence, new
            {
                emps = sequence.DomainsIDs,
                superior = superior,
                ISOYear = sequence.Year,
                ISOWeekFrom = sequence.ISOWeekFrom,
                ISOWeekTo = sequence.ISOWeekTo,
            });
            _connection.Close();

            string whereForExistence = "";
            foreach (var i in sequence.DomainsIDs)
            {
                whereForExistence += $"(dosh_do_no = {i} and (dosh_week_number BETWEEN @from AND @to))  or ";
            }
            //remove last 'or'
            whereForExistence = whereForExistence.Remove(whereForExistence.Length - 3, 3);
            var days = sequence.WeekDays;
            var daysSetString = "";
            foreach (var day in days)
            {
                var dow = "";
                switch (day)
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

                daysSetString += dow + " = @ShiftNR, ";
            }
            //remove last ','
            daysSetString = daysSetString.Remove(daysSetString.Length - 2, 2);


            if (existence.Count() > 0)
            {
                string update = $@"
			    update  [dbo].[csti_do_shift] 
			    set {daysSetString}
			    where
			    dosh_year = @ISOYear and ({whereForExistence})
			    
                ";


                _connection.Open();
                var updateResult = await _connection.ExecuteAsync(update, new
                {
                    ISOYear = sequence.Year,
                    ShiftNR = sequence.ShiftNR,
                    from = sequence.ISOWeekFrom,
                    to = sequence.ISOWeekTo
                }
                 );
                _connection.Close();
            }

            // all combinations of ids and week numbers for the inserts (already existent one will be removed from combinations int notExistingcombinations)
            List<int> idsCombination = new List<int>(sequence.DomainsIDs);
            List<int> weeksCombination = new List<int>(Enumerable.Range(sequence.ISOWeekFrom, sequence.ISOWeekTo).ToList());
            List<(int, int)> combinations = new List<(int, int)>();
            foreach (var domain in idsCombination)
            {
                foreach (var week in weeksCombination)
                {
                    combinations.Add((domain, week));
                }
            }

            var notExistingCombinations = new List<(int, int)>(combinations);
            notExistingCombinations.RemoveAll(x => existence.Any(y => y.Equals(x)));


            if (notExistingCombinations.Count() > 0)
            {
                string insert = @$"
                insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday] ) 
                values 
                ";

                var insertvalues = "";
                foreach (var combination in notExistingCombinations)
                {
                    insertvalues += $@" ({combination.Item1}, {combination.Item2}, @ISOYear, {(sequence.WeekDays.Contains(1) ? "@ShiftNR" : "1")},
									{(sequence.WeekDays.Contains(2) ? "@ShiftNR" : "1")},
									{(sequence.WeekDays.Contains(3) ? "@ShiftNR" : "1")},
									{(sequence.WeekDays.Contains(4) ? "@ShiftNR" : "1")},
									{(sequence.WeekDays.Contains(5) ? "@ShiftNR" : "1")},
									{(sequence.WeekDays.Contains(6) ? "@ShiftNR" : "1")},
									{(sequence.WeekDays.Contains(7) ? "@ShiftNR" : "1")}),";
                }

                insert += insertvalues[..^1];
                _connection.Open();
                var insertResult = await _connection.ExecuteAsync(insert, new
                {
                    ISOYear = sequence.Year,
                    ShiftNR = sequence.ShiftNR
                }
                 );
                _connection.Close();
            }

        }

        public async Task DatesSequenceCreate(int superior, MultipleSelectionForm sequence, IEnumerable<DayWeekYear> dayWeekYears)
        {
            var right = (await _rightsRepository.RightsOverEmps(sequence.Domains, superior)).Select(x => x.Emp).ToList();
            sequence.Domains.Sort();
            right.Sort();
            if (!right.SequenceEqual(sequence.Domains))
            {
                throw new InvalidOperationException("user requested access to not owned domains");
            }
            var onlyWeekAndYear = dayWeekYears.Select(x => new { year = x.Year, week = x.WeekNumber });
            var datesdono = sequence.Domains.SelectMany(x => onlyWeekAndYear.Select(y => new { week = y.week, year = y.year, dono = x })).ToList();
            var h = 1 + 1;

            string whereforExistence = "";
            foreach (var i in dayWeekYears)
            {
                whereforExistence += $"(dosh_week_number = {i.WeekNumber}  and dosh_year = {i.Year}) or ";
            }

            //remove last 'or'
            whereforExistence = whereforExistence.Remove(whereforExistence.Length - 3, 3);


            string checkExistence = $@"
            select dosh_do_no, dosh_week_number, dosh_year from csti_do_shift 
            join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
            join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no
            where ({whereforExistence})
            and authdomain.audoen_en_no in @dono and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1
            ";
            _connection.Open();
            var existence = await _connection.QueryAsync<(int?, int?, int?)?>(checkExistence, new
            {
                superior = superior,
                dono = sequence.Domains
            });
            _connection.Close();

            var x = 1 + 1;

            var dates = sequence.Dates;
            var dateSetString = "";

            foreach (var d in dates)
            {
                var weekday = d.DayOfWeek;
                var dow = "";
                switch (weekday)
                {
                    case (DayOfWeek)1:
                        dow = "dosh_monday";
                        break;
                    case (DayOfWeek)2:
                        dow = "dosh_tuesday";
                        break;
                    case (DayOfWeek)3:
                        dow = "dosh_wednesday";
                        break;
                    case (DayOfWeek)4:
                        dow = "dosh_thursday";
                        break;
                    case (DayOfWeek)5:
                        dow = "dosh_friday";
                        break;
                    case (DayOfWeek)6:
                        dow = "dosh_saturday";
                        break;
                    case (DayOfWeek)7:
                        dow = "dosh_sunday";
                        break;
                }

                dateSetString += dow + " = @ShiftNR, ";

            }

            dateSetString = dateSetString.Remove(dateSetString.Length - 2, 2);

            if (existence.Count() > 0)
            {
                string update = $@"
			    update  [dbo].[csti_do_shift] 
			    set  {dateSetString} 
			    where ({whereforExistence})";

                _connection.Open();
                var updateResult = await _connection.QueryAsync<(int?, int?, int?)?>(update, new
                {
                    superior = superior,
                    dono = sequence.Domains
                });
                _connection.Close();
            }

            var xydf = 1 + 1;

            // all combinations of ids and week numbers for the inserts (already existent one will be removed from combinations int notExistingcombinations)
            List<int> domaincombination = new List<int>(sequence.Domains);
            List<int> datesCombination = new List<int>(sequence.Dates.Select(d => d.DayNumber));
            List<int> shiftCombination = new List<int>(sequence.ShiftNR);
            List<(int?, int?, int?)?> combinations = new List<(int?, int?, int?)?>();
            foreach (var domain in domaincombination)
            {
                foreach (var week in datesCombination)
                {
                    foreach (var shift in shiftCombination)
                    {
                        combinations.Add((domain, week, shift));
                    }

                }
            }

            var notExistingCombinations = new List<(int?, int?, int?)?>(combinations);
            notExistingCombinations.RemoveAll(x => existence.Any(y => y.Equals(x)));

            if (notExistingCombinations.Count() > 0)
            {
                string insert = @$"
                insert into [dbo].[csti_do_shift] ([dosh_do_no], [dosh_week_number], [dosh_year], [dosh_monday], [dosh_tuesday], [dosh_wednesday], [dosh_thursday], [dosh_friday], [dosh_saturday], [dosh_sunday] ) 
                values 
                ";

                var insertvalues = "";
                foreach (var combination in notExistingCombinations)
                {
                   


                }

                    _connection.Open();
                var updateResult = await _connection.QueryAsync<(int?, int?, int?)?>(insert, new
                {
                    superior = superior,
                    dono = sequence.Domains
                });
                _connection.Close();
            }
        }
    }
}
