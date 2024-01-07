using Agendo.Server.Persistance;
using NUnit.Framework.Legacy;
using System.Data.SqlClient;

namespace NUnitTesting.Server.Persistance
{
    internal class DailyScheduleRepositoryTest
    {


        [Test]
        public async Task GetAllAsync()
        {
            //arrange
            SqlConnection connection =new TestDbConnection().Connection;
            var repository = new DailyScheduleRepository(connection);

            //act
            var x = repository.GetAllAsync().Result;

            //assert
            ClassicAssert.IsNotNull(x);
           
            Assert.That(x.Count, Is.AtLeast(4));
            Assert.That(x[0].Nr, Is.EqualTo(1));
            Assert.That(x[0].Name, Is.EqualTo("empty"));

            await connection.CloseAsync();

        }

        [Test]
        public async Task GetSingleShiftAsync()
        {
            //arrange
            SqlConnection connection = new TestDbConnection().Connection;
            var repository = new DailyScheduleRepository(connection);

            //act
            var x = repository.GetSingleShiftAsync(2).Result;

            //assert
            ClassicAssert.IsNotNull(x);

            Assert.That(x.Count, Is.EqualTo(1));
            Assert.That(x[0].Nr, Is.EqualTo(2));
            Assert.That(x[0].Name, Is.EqualTo("standard shift"));
            Assert.That(x[0].Hours, Is.EqualTo(8));
            Assert.That(x[0].Color, Is.EqualTo("#98f5e1"));

            await connection.CloseAsync();
        }

        [Test]
        public async Task AddNewShift()
        {
            //arrange
            SqlConnection connection = new TestDbConnection().Connection;
            var repository = new DailyScheduleRepository(connection);

            //act
            var insert =  repository.AddNewShift("NEW shift", 10, "#98f5e1").Result;

            //assert
            ClassicAssert.IsNotNull(insert);
            var x = repository.GetSingleShiftAsync(insert).Result;
            Assert.That(x.Count, Is.EqualTo(1));
            Assert.That(x[0].Nr, Is.EqualTo(insert));
            Assert.That(x[0].Name, Is.EqualTo("NEW shift"));
            Assert.That(x[0].Hours, Is.EqualTo(10));
            Assert.That(x[0].Color, Is.EqualTo("#98f5e1"));

            await connection.CloseAsync();
        }
    }
}
