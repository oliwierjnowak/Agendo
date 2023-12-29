using Agendo.Shared.DTOs;
using Agendo.Server.Persistance;
using Agendo.Shared.Form.CreateEmployeeShift;
using System.Globalization;
using Agendo.Server.Models;
using Agendo.Server.Services.enums;

namespace Agendo.Server.Services
{
    using ResultMangeShift = (ShiftPutCode code, EmployeeShiftDTO? value);

    public interface IShiftService 
    {
        Task<ResultMangeShift> ManageShift(int sup,CreateEmployeeShift empshift);
        Task<List<EmployeeShiftDTO>> GetMultipleEmpsAsync(int superior, IEnumerable<int> emps);
        Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int superior, int emp);
    }
    
    
    public class ShiftService(IShiftRepository _employeeShiftRepository, IDomainService _domainService) : IShiftService
    {

        public async Task<ResultMangeShift> ManageShift(int sup,CreateEmployeeShift empshift)
        {
            // out of datetime generate iso week iso year and day
            var isoweek =  ISOWeek.GetWeekOfYear(empshift.ShiftDate);
            var isoyear = empshift.ShiftDate.Year;
            DayOfWeek dayOfWeek = empshift.ShiftDate.DayOfWeek;
            var empNR = empshift.EmpNr;
            var shiftNR = empshift.ShiftNr;

             EmployeeShift employeeShift = new EmployeeShift()
             {
                 EmpNr = empshift.EmpNr,
                 ISOWeek = isoweek,
                 ISOYear = isoyear,
                 DOW = (int)dayOfWeek,
                 ShiftNR = shiftNR
             };
            var result = await _employeeShiftRepository.ManageShift(employeeShift);
            if(result.ShiftNR == 1){
                return (ShiftPutCode.Deleted, null);
            }
            else
            {
                var domains = await _domainService.GetShiftEmployees(sup, ISOWeek.ToDateTime(result.ISOYear, result.ISOWeek, (DayOfWeek)result.DOW).AddHours(8), result.ShiftNR);

                var dto = new EmployeeShiftDTO
                {
                    Domains = domains.ToList(),
                    Start = ISOWeek.ToDateTime(result.ISOYear, result.ISOWeek, (DayOfWeek)result.DOW).AddHours(8),
                    End = ISOWeek.ToDateTime(result.ISOYear, result.ISOWeek, (DayOfWeek)result.DOW).AddHours(8 + result.ShiftHours),
                    ShiftNR = result.ShiftNR,
                    ShiftName = result.ShiftName,
                    ShiftHours = result.ShiftHours
                };
                return (ShiftPutCode.Updated, dto);
            }
        }

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




    }
}
