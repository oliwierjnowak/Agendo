using Agendo.Server;
using Agendo.Server.Persistance;
using Dapper;
using Moq;
using Moq.Dapper;
using NUnit.Framework.Legacy;
using NUnitTesting;
using System.Data;
using System.Data.SqlClient;
using System.Net.WebSockets;


namespace Testing.Server.Persistance
{
    
    internal class DomainRepositoryTest
    {
        
       

        [Test]
        public async Task GetAllAsync()
        {
            //arrange
            SqlConnection connection = TestDbConnection.GetInstance().Connection;
            var repository = new DomainRepository(connection);

            //act
            var x = repository.GetAllAsync(1).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x.Count, 5);
            var kettel = x[1];
            Assert.That(kettel.Nr, Is.EqualTo(2));
            Assert.That(kettel.Name, Is.EqualTo("Kettel"));
            await connection.CloseAsync();

        } 
        [Test]
        public async Task GetListAsync()
        {
            //arrange
            SqlConnection connection = TestDbConnection.GetInstance().Connection;
            var repository = new DomainRepository(connection);

            //act
            var x = repository.GetListAsync(1, new int[] { 1, 2, 3 }).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x.Count, 3);
            var kettel = x[1];
            Assert.That(kettel.Nr, Is.EqualTo(2));
            Assert.That(kettel.Name, Is.EqualTo("Kettel"));
            await connection.CloseAsync();
        }

        [Test]
        public async Task GetShiftEmployees()
        {
            //arrange
            SqlConnection connection = TestDbConnection.GetInstance().Connection;
            var repository = new DomainRepository(connection);

            //act
            var x = repository.GetShiftEmployees(1, 2023,1,(DayOfWeek)1,3).Result.ToList();

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x.Count, 1);
            var kettel = x[0];
            Assert.That(kettel.Nr, Is.EqualTo(2));
            Assert.That(kettel.Name, Is.EqualTo("Kettel"));

            await connection.CloseAsync();
        }

        // write here hello world and


    }
}