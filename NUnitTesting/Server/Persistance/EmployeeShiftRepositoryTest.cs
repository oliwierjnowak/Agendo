using Agendo.Server;
using Agendo.Server.Models;
using Agendo.Server.Persistance;
using Dapper;
using Moq;
using Moq.Dapper;
using NUnit.Framework.Legacy;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace NUnitTesting.Server.Persistance
{
    internal class EmployeeShiftRepositoryTest
    {
  


		//write test for GetSingleEmpAsync here
		[Test]
		public async Task GetSingleEmpAsync()
		{
            //arrange
            SqlConnection connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");

            var repository = new EmployeeShiftRepository(connection);

            //act
            var x = repository.GetSingleEmpAsync(1, 1).Result;
            var only1 = x.FindAll(x => x.EmpNr == 1);
            var not1 = x.FindAll(x => x.EmpNr != 1);
            var weekk32 = only1.FindAll(x => x.ISOWeek == 32 && x.ISOYear == 2022 && x.DOW == 4);
            //assert
            ClassicAssert.IsNotNull(x);

            Assert.That(not1.Count, Is.EqualTo(0));
            Assert.That(weekk32.Count, Is.EqualTo(1));

            Assert.That(weekk32[0].ShiftName, Is.EqualTo("part time shift"));
            Assert.That(weekk32[0].ShiftHours, Is.EqualTo(4));

            //6 is not superior of 2

            // act 
            var notSuperior = repository.GetSingleEmpAsync(6, 2).Result;

            ClassicAssert.IsNotNull(x);

            Assert.That(notSuperior.Count, Is.EqualTo(0));
            await connection.CloseAsync();


        }

		//write test for GetMultipleEmpsAsync here
		[Test]
		public async Task GetMultipleEmpsAsync()
		{
            //arrange
            SqlConnection connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");



            var repository = new EmployeeShiftRepository(connection);

            //act
            var x = repository.GetMultipleEmpsAsync(1,new int[] { 1, 2, 3 }).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            var not123 = x.FindAll(x => x.EmpNr != 1 && x.EmpNr != 2 && x.EmpNr != 3);

            ClassicAssert.AreEqual(not123.Count, 0);
            var only1 = x.FindAll(x => x.EmpNr == 1);

            var weekk32 = only1.FindAll(x => x.ISOWeek == 32 && x.ISOYear == 2022 && x.DOW == 4);
            Assert.That(weekk32.Count, Is.EqualTo(1));

            Assert.That(weekk32[0].ShiftName, Is.EqualTo("part time shift"));
            Assert.That(weekk32[0].ShiftHours, Is.EqualTo(4));


            // not superior
            var notSuperior = repository.GetMultipleEmpsAsync(6, new int[] { 1, 2, 3 }).Result;
            ClassicAssert.IsNotNull(x);

            Assert.That(notSuperior.Count, Is.EqualTo(0));
            await connection.CloseAsync();
        }
        [Test]
        public async Task CreateShift()
        {
            //arrange
            SqlConnection connection = new SqlConnection("Server=localhost,1433;User ID=SA;Password=A@123!23sda;Trusted_Connection=False;Encrypt=False;");

            var repository = new EmployeeShiftRepository(connection);

            //not exsisting week
            var shift = new EmployeeShift { EmpNr = 2, ISOWeek = 1, ISOYear = 2021, DOW = 1, ShiftNR = 4, ShiftName = "babymonat", ShiftHours = 3 };
            //act

          repository.CreateShift(shift).Wait();

          //  Assert.That(result, Is.EqualTo(1));
            var find2 = repository.GetSingleEmpAsync(2, 2).Result;
            var week1year2021Monday = find2.FindAll(x => x.ISOWeek == 1 && x.ISOYear == 2021 && x.DOW == 1).ToList();
            Assert.That(week1year2021Monday.Count, Is.EqualTo(1));
            Assert.That(week1year2021Monday[0].ShiftNR, Is.EqualTo(4));
            Assert.That(week1year2021Monday[0].ShiftName, Is.EqualTo("babymonat"));
            Assert.That(week1year2021Monday[0].EmpNr, Is.EqualTo(2));


            //exsisting week for update


            var findWeek452023 = find2.FindAll(x => x.ISOWeek == 45 && x.ISOYear == 2023 ).ToList();
            var updateshift = new EmployeeShift { EmpNr = 2, ISOWeek = 45, ISOYear = 2023, DOW = 5, ShiftNR = 4, ShiftName = "babymonat", ShiftHours = 4 };

            repository.CreateShift(updateshift).Wait();

            var findAfterUpdate = repository.GetSingleEmpAsync(2, 2).Result;
            var week452023 = findAfterUpdate.FindAll(x => x.ISOWeek == 45 && x.ISOYear == 2023 && x.DOW == 5).ToList();
            Assert.That(week452023.Count, Is.EqualTo(1));
            Assert.That(week452023[0].ShiftNR, Is.EqualTo(4));
            Assert.That(week452023[0].ShiftName, Is.EqualTo("babymonat"));
            Assert.That(week452023[0].EmpNr, Is.EqualTo(2));

            await connection.CloseAsync();
        }
    }



}