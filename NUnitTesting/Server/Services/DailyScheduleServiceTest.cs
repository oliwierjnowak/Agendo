using Moq;
using Agendo.Shared.DTOs;
using Agendo.Server.Persistance;
using Agendo.Server.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitTesting.Server.Services
{
    internal class DailyScheduleServiceTest
    {
        private Mock<IDailyScheduleRepository> ArrangeRepository()
        {
            var MockRepo = new Mock<IDailyScheduleRepository>();

            List<DailyScheduleDTO> dtos = new List<DailyScheduleDTO>() {
            new DailyScheduleDTO{Nr = 1, Name ="FulltimeCS2", Hours =1, Color = "#e9c46a" },
            new DailyScheduleDTO{Nr = 2, Name ="ParttimeCS2", Hours =1, Color = "#e9c46a" },
            new DailyScheduleDTO{Nr = 3, Name ="LifetimeCS2", Hours =1, Color = "#e9c46a" },
            };


            MockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(dtos);

            MockRepo.Setup(r => r.GetSingleShiftAsync(dtos[1].Nr)).ReturnsAsync(dtos[1]);

            DailyScheduleDTO addDto = new DailyScheduleDTO() { Nr = 1, Name = "FulltimeSilver",Hours = 5, Color = "#e9c46a" };
            MockRepo.Setup(r => r.AddNewShift(addDto.Name, addDto.Hours, addDto.Color)).ReturnsAsync(It.IsAny<int>());


            return MockRepo;
        }



        [Test]
        public void GetAllAsync()
        {
            //Arrange
            var repo = ArrangeRepository();
            var service = new DailyScheduleService(repo.Object);

            //Act
            var result = service.GetAllAsync().Result;

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(3));

            Assert.That(result[0].Nr, Is.EqualTo(1));
            Assert.That(result[0].Name, Is.EqualTo("FulltimeCS2"));


        }

        [Test]
        public void GetSingleShiftAsync() 
        {
            //Arrange
            var repo = ArrangeRepository();
            var service = new DailyScheduleService(repo.Object);
            //Act
            var result = service.GetSingleShiftAsync(2).Result;

            //Assert
            Assert.That (result, Is.Not.Null);
            Assert.That(result.Nr, Is.EqualTo(2));
            Assert.That(result.Name, Is.EqualTo("ParttimeCS2"));
        }

        [Test]
        public void AddNewShift() 
        {
            //Arrange
            var repo = ArrangeRepository();
            var service = new DailyScheduleService(repo.Object);

            //Act
            var result = service.AddNewShift("FulltimeSilver", 5, "#e9c46a").Result;
            Console.WriteLine("This is the result:"+result);
            //Assert
            Assert.That(result, Is.EqualTo(default(int)));

            repo.Verify(r => r.AddNewShift("FulltimeSilver", 5, "#e9c46a"), Times.Once);
            
            

        }

    }
}
