using Agendo.Server.Controllers;
using Agendo.Server.Persistance;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTesting.Server.controller
{
    internal class DomainControllerTest
    {
        private Mock<IDomainService> ArrangeRepository()
        {
            var mockService= new Mock<IDomainService>();

            List<Agendo.Server.Models.DomainDTO> data = new List<Agendo.Server.Models.DomainDTO>() {
               new Agendo.Server.Models.DomainDTO{Nr = 1, Name ="Oliwier Nowak" },
               new Agendo.Server.Models.DomainDTO{Nr = 2, Name ="Anton Schubhart" },
               new Agendo.Server.Models.DomainDTO{Nr = 3, Name ="Philipp Schaffer" },
            };


            mockService.Setup(r => r.GetAllAsync()).ReturnsAsync(data);


            return mockService;
        }


        [Test]
        public void GetAllAsync()
        {
            //ArrangeRepository
            var service = ArrangeRepository();
            var controller = new DomainController(service.Object);

            //act
            var x = controller.Get().Result;
            // assert
            var okObjectResult = x.Result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);

            var model = okObjectResult.Value as IEnumerable<Agendo.Server.Models.DomainDTO>;
            Assert.IsNotNull(model);
            Assert.AreEqual(3, model.Count());

        }
    }
}
