using Agendo.Server;
using Agendo.Server.Models;
using Agendo.Server.Persistance;
using Dapper;
using Moq;
using Moq.Dapper;
using NUnit.Framework.Legacy;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace NUnitTesting.Server.Persistance
{
    internal class ShiftRepositoryTest
    {
  


		//write test for GetSingleEmpAsync here
		[Test]
		public async Task GetSingleEmpAsync()
		{
            //arrange
            SqlConnection connection = new TestDbConnection().Connection;

            var rightsRepository = new RightsRepository(connection);
            var repository = new ShiftRepository(connection, rightsRepository);

            //act
            var x = repository.GetSingleEmpAsync(1, 1).Result;
            var only1 = x.Where(x => x.EmpNr == 1).ToList();
            var not1 = x.Where(x => x.EmpNr != 1).ToList();
            var weekk32 = only1.Where(x => x.ISOWeek == 32 && x.ISOYear == 2022 && x.DOW == 4).ToList();
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
            SqlConnection connection = new TestDbConnection().Connection;


            var rightsRepository = new RightsRepository(connection);
            var repository = new ShiftRepository(connection, rightsRepository);

            //act
            var x = repository.GetMultipleEmpsAsync(1,new int[] { 1, 2, 3 }).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            var not123 = x.Where(x => x.EmpNr != 1 && x.EmpNr != 2 && x.EmpNr != 3).ToList();

            ClassicAssert.AreEqual(not123.Count, 0);
            var only1 = x.Where(x => x.EmpNr == 1).ToList();

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
            SqlConnection connection = new TestDbConnection().Connection;
            var rightsRepository = new RightsRepository(connection);
            var repository = new ShiftRepository(connection, rightsRepository);

            //not exsisting week
            var firstAdd = new Shift
            {
                DOW = 1,
                ISOWeek = 1,
                ISOYear = 1,
                ShiftNR = 4
            };
            //act

            var result = await repository.ManageEmployeesShift(1, [2], firstAdd);


            Assert.That(result.Select(x => new EmployeeShift {ShiftNR = x.ShiftNR, DOW = x.DOW, EmpNr = x.EmpNr, ISOYear = x.ISOYear, ISOWeek = x.ISOWeek }).First(), 
                Is.EqualTo(
                new EmployeeShift { ShiftNR = 4, DOW = 1, EmpNr = 2, ISOWeek = 1,ISOYear =1 }
                ));

            var secondChange= new Shift
            {
                DOW = 1,
                ISOWeek = 1,
                ISOYear = 1,
                ShiftNR = 2
            };

         
            var multiple = await repository.ManageEmployeesShift(1, [2,3], secondChange);
            Assert.That(multiple.Select(x => x.EmpNr).ToArray(), Is.EqualTo((int[])[3, 2]));
            

            foreach (var x in multiple)
            {
                Assert.That(new Shift
                {
                    DOW = 1,
                    ISOWeek = 1,
                    ISOYear = 1,
                    ShiftNR = 2
                }, Is.EqualTo(secondChange));
            }

            var thirdChange = new Shift
            {
                DOW = 1,
                ISOWeek = 1,
                ISOYear = 1,
                ShiftNR = 5
            };


            var multiple3 = await repository.ManageEmployeesShift(1, [2, 3], thirdChange);
            Assert.That(multiple.Select(x => x.EmpNr).ToArray(), Is.EqualTo((int[])[3, 2]));


            foreach (var x in multiple)
            {
                Assert.That(new Shift
                {
                    DOW = 1,
                    ISOWeek = 1,
                    ISOYear = 1,
                    ShiftNR = 5
                }, Is.EqualTo(thirdChange));
            }


            var notAllowed = await repository.ManageEmployeesShift(1, [2, 3,8], thirdChange);

            Assert.That(notAllowed, Is.EqualTo(Enumerable.Empty<EmployeeShift>()));

            var notAllowed3 = await repository.ManageEmployeesShift(1, [8], thirdChange);

            Assert.That(notAllowed, Is.EqualTo(Enumerable.Empty<EmployeeShift>()));
        }
    }



}