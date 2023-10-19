using Agendo.Server.Models;
using Dapper;
using Moq;
using Moq.Dapper;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Testing.Server.Persistance
{
    [TestClass]
    public class DomainRepository
    {
        
        public Mock<IDbConnection> ArrangeDb()
        {
            var mockRepository = new Mock<IDbConnection>();
            string selectQuery = "select do_no AS 'Nr',do_name AS 'Name'   from [dbo].[csmd_domain]";
            List<Agendo.Server.Models.DomainDTO> data = new List<Agendo.Server.Models.DomainDTO>() {
               new DomainDTO{Nr = 1, Name ="Oliwier Nowak" },
               new DomainDTO{Nr = 2, Name ="Anton Schubhart" },
               new DomainDTO{Nr = 3, Name ="Philipp Schaffer" },
            };

            Task<IEnumerable<DomainDTO>> taskResult = Task.FromResult<IEnumerable<DomainDTO>>(data);



            mockRepository.SetupDapperAsync(c => c.QueryAsync<int>(It.IsAny<string>(), null, null, null, null))
          .ReturnsAsync("");
            return mockRepository;
        }

        [TestMethod]
        public void TestMethod1()
        {
        }
    }
}