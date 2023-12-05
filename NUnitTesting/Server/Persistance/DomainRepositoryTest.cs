using Agendo.Server;
using Agendo.Server.Persistance;
using Dapper;
using Moq;
using Moq.Dapper;
using NUnit.Framework.Legacy;
using System.Data;
using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Testing.Server.Persistance
{
    
    internal class DomainRepositoryTest
    {
        
        private Mock<IDbConnection> ArrangeDb()
        {
            var mockRepository = new Mock<IDbConnection>();
            string selectQuery = "select do_no AS 'Nr',do_name AS 'Name'   from [dbo].[csmd_domain]";
            List<Agendo.Server.Models.DomainDTO> data = new List<Agendo.Server.Models.DomainDTO>() {
               new Agendo.Server.Models.DomainDTO{Nr = 1, Name ="Oliwier Nowak" },
               new Agendo.Server.Models.DomainDTO{Nr = 2, Name ="Anton Schubhart" },
               new Agendo.Server.Models.DomainDTO{Nr = 3, Name ="Philipp Schaffer" },
            };


            mockRepository.SetupDapperAsync(c => c.QueryAsync<Agendo.Server.Models.DomainDTO>(selectQuery, null, null, null, null))
            .ReturnsAsync(data);


            return mockRepository;
        }

        [Test]
        public void GetAllAsync()
        {
            //arrange
            var connection = ArrangeDb();
            var repository = new DomainRepository(connection.Object);

            //act
            var x = repository.GetAllAsync(1).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x.Count, 3);

        }
    }
}