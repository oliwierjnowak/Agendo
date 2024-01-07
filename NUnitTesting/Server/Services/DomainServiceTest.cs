using Agendo.Server.Persistance;
using Agendo.Server.Services;
using Moq;
using NUnit.Framework.Legacy;
using System.Data;
using Agendo.Shared.DTOs;

namespace NUnitTesting.Server.Services
{
    internal class DomainServiceTest
    {


        private Mock<IDomainRepository> ArrangeRepository()
        {
            var mockRepository = new Mock<IDomainRepository>();

            List<DomainDTO> data = new List<DomainDTO>() {
               new DomainDTO{Nr = 1, Name ="Oliwier Nowak" },
               new DomainDTO{Nr = 2, Name ="Anton Schubhart" },
               new DomainDTO{Nr = 3, Name ="Philipp Schaffer" },
            };


            mockRepository.Setup(r => r.GetAllAsync(1)).ReturnsAsync(data);

            mockRepository.Setup(r => r.GetListAsync(1, new List<int> { 1, 2 })).ReturnsAsync(data.Where(x => x.Nr == 1 || x.Nr == 2).ToList());

            return mockRepository;
        }


        [Test]
        public void GetAllAsync()
        {
            //ArrangeRepository
            var repo = ArrangeRepository();
            var service = new DomainService(repo.Object);

            //act
            var x = service.GetAllAsync(1).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x.Count, 3);

        }

        //write test for GetListAsync here 
        [Test]
        public void GetListAsync()
        {
            //ArrangeRepository
            var repo = ArrangeRepository();
            var service = new DomainService(repo.Object);

            //act
            var x = service.GetListAsync(1, new List<int> { 1, 2 }).Result;

            //assert
            ClassicAssert.IsNotNull(x);
            ClassicAssert.AreEqual(x.Count, 2);
            //assert that the first element in the list has the name "Oliwier Nowak"    
            ClassicAssert.AreEqual(x[0].Name, "Oliwier Nowak");

        }   


    }
}
