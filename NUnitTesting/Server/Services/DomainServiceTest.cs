using Agendo.Server.Persistance;
using Agendo.Server.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTesting.Server.Services
{
    internal class DomainServiceTest
    {


        private Mock<IDomainRepository> ArrangeRepository()
        {
            var mockRepository = new Mock<IDomainRepository>();

            List<Agendo.Server.Models.DomainDTO> data = new List<Agendo.Server.Models.DomainDTO>() {
               new Agendo.Server.Models.DomainDTO{Nr = 1, Name ="Oliwier Nowak" },
               new Agendo.Server.Models.DomainDTO{Nr = 2, Name ="Anton Schubhart" },
               new Agendo.Server.Models.DomainDTO{Nr = 3, Name ="Philipp Schaffer" },
            };


            mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(data);


            return mockRepository;
        }


        [Test]
        public void GetAllAsync()
        {
            //ArrangeRepository
            var repo = ArrangeRepository();
            var service = new DomainService(repo.Object);

            //act
            var x = service.GetAllAsync().Result;

            //assert
            Assert.IsNotNull(x);
            Assert.AreEqual(x.Count, 3);

        }
    }
}
