using Agendo.Server.Models;
using Agendo.Server.Persistance;
using Agendo.Shared.Form.CreateEmployeeShift;
using System.Globalization;

namespace Agendo.Server.Services
{
    public interface IEmployeeShiftService 
    {
        Task<int> CreateShift(CreateEmployeeShift empshift);
        Task<List<EmployeeShiftDTO>> GetMultipleEmpsAsync(int superior, IEnumerable<int> emps);
        Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int superior, int emp);
    }
    
    
    public class EmployeeShiftService(IEmployeeShiftRepository _employeeShiftRepository, IDomainService _domainService) : IEmployeeShiftService
    {

        public async Task<int> CreateShift(CreateEmployeeShift empshift)
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
            return await _employeeShiftRepository.CreateShift(employeeShift);
        }

        public async Task<List<EmployeeShiftDTO>> GetMultipleEmpsAsync(int sup,IEnumerable<int> emps)
        {
            var shifts = await _employeeShiftRepository.GetMultipleEmpsAsync(sup,emps);
            List<EmployeeShiftDTO> employeeShiftDTOs = new List<EmployeeShiftDTO>();

            foreach (var group in shifts.GroupBy(es => new { es.ISOWeek, es.ISOYear, es.DOW, es.ShiftNR, es.ShiftName, es.ShiftHours }))
            {
                var domainIds = group.Select( g =>  g.EmpNr);

                // because of that if statement we only select empshift where all of the emps are working and not just only one  
                if ((domainIds.All(emps.Contains) && domainIds.Count() == emps.Count()))
                {
                    var domains = await _domainService.GetListAsync(sup, domainIds);

                    employeeShiftDTOs.Add(new EmployeeShiftDTO
                    {
                        Domains = domains,
                        Start = ISOWeek.ToDateTime(group.Key.ISOYear, group.Key.ISOWeek, (DayOfWeek)group.Key.DOW).AddHours(8),
                        End = ISOWeek.ToDateTime(group.Key.ISOYear, group.Key.ISOWeek, (DayOfWeek)group.Key.DOW).AddHours(8 + group.Key.ShiftHours),
                        ShiftNR = group.Key.ShiftNR,
                        ShiftName = group.Key.ShiftName,
                        ShiftHours = group.Key.ShiftHours
                    }); ;
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
