using Agendo.Server.Persistance;
using Agendo.Server.Services;
using Moq;
using NUnit.Framework.Legacy;
using Agendo.Shared.DTOs;
using Agendo.Server.Models;


namespace NUnitTesting.Server.Services;

internal class ShiftServiceTest
{
    private Mock<IShiftRepository> ArrangeRepository()
    {
        var mockRepo = new Mock<IShiftRepository>();

        List<DomainDTO> dtos = new List<DomainDTO>() {
            new DomainDTO{Nr = 1, Name ="Oliwier Nowak" },
            new DomainDTO{Nr = 2, Name ="Anton Schubhart" },
            new DomainDTO{Nr = 3, Name ="Philipp Schaffer" },
        };


        List<EmployeeShiftDTO> data = new List<EmployeeShiftDTO>()
        {
            new EmployeeShiftDTO { Domains = dtos, Start = DateTime.Now, End = DateTime.Now.AddHours(6),ShiftNR = 1,ShiftHours = 6,ShiftName = "Shift1"},
            new EmployeeShiftDTO { Domains = dtos, Start = DateTime.Now, End = DateTime.Now.AddHours(7),ShiftNR = 2,ShiftHours = 7,ShiftName = "Shift2"},
            new EmployeeShiftDTO { Domains = dtos, Start = DateTime.Now, End = DateTime.Now.AddHours(8),ShiftNR = 3,ShiftHours = 8,ShiftName = "Shift3"},
        };

      
        
        mockRepo.Setup(r => r.GetSingleEmpAsync(1000, 1))
            .ReturnsAsync(data.Select(dto => new Agendo.Server.Models.EmployeeShift
            {
                ISOYear = 2022,
                ISOWeek = 1,    
                DOW = 1,        
                ShiftNR = dto.ShiftNR,
                ShiftName = dto.ShiftName,
                ShiftHours = dto.ShiftHours
            }).ToList());
        

        mockRepo.Setup(r => r.GetMultipleEmpsAsync(It.IsAny<int>(), It.IsAny<IEnumerable<int>>(), It.IsAny<List<int>>(), It.IsAny<int>()))
            .ReturnsAsync((int supervisor, IEnumerable<int> employees, List<int> isoWeekNumbers, int year) =>
            {
                return new List<EmployeeShift>
                {
                new EmployeeShift{EmpNr = 1,ISOYear = 2022,ISOWeek = 1,DOW = 1,ShiftNR = 1,ShiftHours = 1,ShiftName = "Shift1"},
                new EmployeeShift{EmpNr = 2,ISOYear = 2022,ISOWeek = 1,DOW = 1,ShiftNR = 2,ShiftHours = 1,ShiftName = "Shift2"}
                };
            });


        return mockRepo;

    }

    [Test]
    public void GetSingleEmp()
    {
        // Arrange
        var repo = ArrangeRepository();
       
        var mockDomainService = new Mock<IDomainService>();
        mockDomainService.Setup(ds => ds.GetShiftEmployees(It.IsAny<int>(), It.IsAny<DateTime>(), It.IsAny<int>()))
            .ReturnsAsync(new List<DomainDTO>()); 

        var service = new ShiftService(repo.Object, mockDomainService.Object);
        // Act
        var result = service.GetSingleEmpAsync(1000, 1).Result;

        // Assert
        Assert.That(result,Is.Not.Null);
      
        Assert.That(result[0].ShiftNR, Is.EqualTo(1));
        Assert.That(result[0].ShiftName, Is.EqualTo("Shift1"));
      
    }

    [Test]
    public void GetShiftsAsync()
    {
        // Arrange
        var repo = ArrangeRepository();
        var mockDomainService = new Mock<IDomainService>();
        var service = new ShiftService(repo.Object, mockDomainService.Object);

        // Act
        var result = service.GetShiftsAsync(1000, new List<int> { 1, 2 }, DateTime.Now).Result;

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.EqualTo(2));

        Assert.That(result[0].ShiftNR, Is.EqualTo(1));
        Assert.That(result[0].ShiftName, Is.EqualTo("Shift1"));

    }

}