using Agendo.Shared.DTOs;
using Agendo.Server.Persistance;
using Agendo.Shared.Form.CreateEmployeeShift;
using System.Globalization;
using Agendo.Server.Models;
using Agendo.Server.Services.enums;

namespace Agendo.Server.Services
{
    public interface IShiftService 
    {
        Task<EmployeeShiftDTO> ManageMultipleEmpShift(int sup, CreateMultipleEmpShift details);
        Task<List<EmployeeShiftDTO>> GetMultipleEmpsAsync(int superior, IEnumerable<int> emps);
        Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int superior, int emp);
        
    }
    
    
    public class ShiftService(IShiftRepository _employeeShiftRepository, IDomainService _domainService) : IShiftService
    {

        public async Task<List<EmployeeShiftDTO>> GetMultipleEmpsAsync(int sup,IEnumerable<int> emps)
        {
            var shifts = await _employeeShiftRepository.GetMultipleEmpsAsync(sup,emps);
            var employeeShiftDTOs = new List<EmployeeShiftDTO>();

            foreach (var group in shifts.GroupBy(es => new { es.ISOWeek, es.ISOYear, es.DOW, es.ShiftNR, es.ShiftName, es.ShiftHours }))
            {
                var domainIds = group.Select( g =>  g.EmpNr);

                // because of that if statement we only select empshift where all of the emps are working and not just only one  
                if ((domainIds.All(emps.Contains) && domainIds.Count() == emps.Count()))
                {
                    var domains = await _domainService.GetShiftEmployees(sup, ISOWeek.ToDateTime(group.Key.ISOYear, group.Key.ISOWeek, (DayOfWeek)group.Key.DOW).AddHours(8), group.Key.ShiftNR);

                    employeeShiftDTOs.Add(new EmployeeShiftDTO
                    {
                        Domains = domains.ToList(),
                        Start = ISOWeek.ToDateTime(group.Key.ISOYear, group.Key.ISOWeek, (DayOfWeek)group.Key.DOW).AddHours(8),
                        End = ISOWeek.ToDateTime(group.Key.ISOYear, group.Key.ISOWeek, (DayOfWeek)group.Key.DOW).AddHours(8 + group.Key.ShiftHours),
                        ShiftNR = group.Key.ShiftNR,
                        ShiftName = group.Key.ShiftName,
                        ShiftHours = group.Key.ShiftHours
                    }); 
                }
               
            }

            return employeeShiftDTOs;
        }


        public async Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int superior, int emp)
        {
            var shifts = await _employeeShiftRepository.GetSingleEmpAsync(superior,emp);
            var ShiftsDTO = new List<EmployeeShiftDTO>(); 
            foreach(var shift in shifts)
            {
                var fromISOWeek = ISOWeek.ToDateTime(shift.ISOYear, shift.ISOWeek, (DayOfWeek)shift.DOW);
                var startTime = new DateTime(fromISOWeek.Year, fromISOWeek.Month, fromISOWeek.Day,8,0,0);
               
                var EmpDTO = new EmployeeShiftDTO
                {
                    Domains = new List<DomainDTO>(),
                    ShiftHours = shift.ShiftHours,
                    ShiftName = shift.ShiftName,    
                    ShiftNR = shift.ShiftNR,    
                    Start = startTime,
                    End = startTime.AddHours(shift.ShiftHours),
                  
                };
                ShiftsDTO.Add(EmpDTO);
            }

                return ShiftsDTO;
        }

        public async Task<EmployeeShiftDTO> ManageMultipleEmpShift(int sup, CreateMultipleEmpShift details)
        {

            Shift shift = new Shift
            {
                ISOWeek = ISOWeek.GetWeekOfYear(details.ShiftDate),
                ISOYear = details.ShiftDate.Year,
                ShiftNR = details.ShiftNr,
                DOW = (int)details.ShiftDate.DayOfWeek
            };
            if (details.RemovedDomains != null)
            {
                var deletionResult = await _employeeShiftRepository.DeleteEmployeesShift(sup, details.RemovedDomains, shift);
            }
 

            var updateResult = await _employeeShiftRepository.ManageEmployeesShift(sup,details.AddedDomains,shift);


            var domains = await _domainService.GetShiftEmployees(sup, ISOWeek.ToDateTime(shift.ISOYear, shift.ISOWeek, (DayOfWeek)shift.DOW).AddHours(8), shift.ShiftNR);

            var dto = new EmployeeShiftDTO
            {
                Domains = domains.ToList(),
                Start = ISOWeek.ToDateTime(shift.ISOYear, shift.ISOWeek, (DayOfWeek)shift.DOW).AddHours(8),
                End = ISOWeek.ToDateTime(shift.ISOYear, shift.ISOWeek, (DayOfWeek)shift.DOW).AddHours(8 + 4),
                ShiftNR = shift.ShiftNR,
            };

            return dto;
        }
    }
}
