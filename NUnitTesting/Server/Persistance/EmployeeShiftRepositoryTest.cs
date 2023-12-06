using Agendo.Server;
using Agendo.Server.Models;
using Agendo.Server.Persistance;
using Dapper;
using Moq;
using Moq.Dapper;
using NUnit.Framework.Legacy;
using System.Data;
using System.Data.Common;

namespace NUnitTesting.Server.Persistance
{
    internal class EmployeeShiftRepositoryTest
    {
  
        private Mock<IDbConnection> ArrangeDb()
        {
            var mockRepository = new Mock<IDbConnection>();
           
            // create mock data list of type EmployeeShift here 
            List<Agendo.Server.Models.EmployeeShift> data =
            [
                new Agendo.Server.Models.EmployeeShift { EmpNr = 1, ISOWeek = 1, ISOYear = 2021, DOW = 1, ShiftNR = 1, ShiftName = "Shift1", ShiftHours = 8},
                new Agendo.Server.Models.EmployeeShift { EmpNr = 2, ISOWeek = 1, ISOYear = 2021, DOW = 1, ShiftNR = 1, ShiftName = "Shift2", ShiftHours = 8},
                new Agendo.Server.Models.EmployeeShift { EmpNr = 3, ISOWeek = 1, ISOYear = 2021, DOW = 1, ShiftNR = 1, ShiftName = "Shift3", ShiftHours = 8}

            ];

            string authjoins = $@"
								join csmd_authorizations_domain_entity authdomain on authdomain.audoen_en_no = dosh_do_no
								join csmd_authorizations auth on auth.au_ri_no = authdomain.audoen_no";
            string authwhere = $@" and authdomain.audoen_en_no = @emp and audoen_do_no = @superior and CONVERT(DATE, GETDATE()) between auth.au_from and auth.au_to and auth.au_enabled = 1";


            string selectQuery1 = @$"
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

            string selectQuery2 = @$"
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

            mockRepository.SetupDapperAsync(c => c.QueryAsync<Agendo.Server.Models.EmployeeShift>(selectQuery1, new { emp = 1, superior = 1 },null,null,null)).ReturnsAsync(data.Where(x => x.EmpNr == 1).ToList());

			mockRepository.SetupDapperAsync(c => c.QueryAsync<Agendo.Server.Models.EmployeeShift>(selectQuery2, new { emps = new int[] { 1, 2, 3 }, superior = 1 },null,null,null)).ReturnsAsync(data.Where(x => x.EmpNr == 1 || x.EmpNr == 2 || x.EmpNr == 3).ToList());


            return mockRepository;
        }

		//write test for GetSingleEmpAsync here
		[Test]
		public void GetSingleEmpAsync()
		{
			   //arrange
			   var connection = ArrangeDb();
            var repository = new EmployeeShiftRepository(connection.Object);

            //act
            var x = repository.GetSingleEmpAsync(1, 1).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x.Count, 3);
			//assert that the first element in the list has the name "Oliwier Nowak"    
			ClassicAssert.AreEqual(x[0].ShiftName, "Shift1");
		}

		//write test for GetMultipleEmpsAsync here
		[Test]
		public void GetMultipleEmpsAsync()
		{
               //arrange
               var connection = ArrangeDb();
            var repository = new EmployeeShiftRepository(connection.Object);

            //act
            var x = repository.GetMultipleEmpsAsync(1,new int[] { 1, 2, 3 }).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x.Count, 3);
		}
        //write test for  public async Task<int> CreateShift(EmployeeShift employeeShift) from EmployeeShiftRepository here
        [Test]
        public void CreateShift()
        {
            //arrange
            var connection = ArrangeDb();
            var repository = new EmployeeShiftRepository(connection.Object);

            //act
            var x = repository.CreateShift(new EmployeeShift { EmpNr = 1, ISOWeek = 1, ISOYear = 2021, DOW = 1, ShiftNR = 1, ShiftName = "Shift1", ShiftHours = 8 }).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x, 1);
        }
    }



}