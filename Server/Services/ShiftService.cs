using Agendo.Shared.DTOs;
using Agendo.Server.Persistance;
using Agendo.Shared.Form.CreateEmployeeShift;
using System.Globalization;
using Agendo.Server.Models;
using Agendo.Server.Services.enums;
using Agendo.Shared.Form;

namespace Agendo.Server.Services
{
    public interface IShiftService 
    {
        Task<EmployeeShiftDTO> ManageMultipleEmpShift(int sup, CreateMultipleEmpShift details);
        Task<List<EmployeeShiftDTO>> GetShiftsGroupedAsync(int superior, IEnumerable<int> emps, DateTime ViewSelectedDate);
        Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int superior, int emp);
        Task<List<EmployeeShiftDTO>> GetShiftsAsync(int superior, IEnumerable<int> emps, DateTime ViewSelectedDate);
        Task DaySequenceCreate(int userid, SequenceForm sequence);
    }
    
    
    public class ShiftService(IShiftRepository _shiftRepository, IDomainService _domainService) : IShiftService
    {

        public async Task<List<EmployeeShiftDTO>> GetShiftsGroupedAsync(int sup,IEnumerable<int> emps, DateTime selectedDate)
        {
            var shifts = await _shiftRepository.GetMultipleEmpsAsync(sup,emps, GetISOWeekNumbers(selectedDate), selectedDate.Year);
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
        public async Task<List<EmployeeShiftDTO>> GetShiftsAsync(int sup, IEnumerable<int> emps, DateTime selectedDate)
        {
            var shifts = await _shiftRepository.GetMultipleEmpsAsync(sup, emps, GetISOWeekNumbers(selectedDate), selectedDate.Year);
            var employeeShiftDTOs = new List<EmployeeShiftDTO>();

            foreach (var group in shifts.GroupBy(es => new { es.ISOWeek, es.ISOYear, es.DOW, es.ShiftNR, es.ShiftName, es.ShiftHours }))
            {
                var domainIds = group.Select(g => g.EmpNr);

                // because of that if statement we only select empshift where all of the emps are working and not just only one  
                
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

            return employeeShiftDTOs;
        }

        public async Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int superior, int emp)
        {
            var shifts = await _shiftRepository.GetSingleEmpAsync(superior,emp);
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
            if (details.RemovedDomains != null )
            {
                var deletionResult = await _shiftRepository.DeleteEmployeesShift(sup, details.RemovedDomains, shift with { ShiftNR = details.OldShiftNr});
            }

            var added = details.AddedDomains ?? Enumerable.Empty<int>();
            var notChanged = details.NotChangedDomains ?? Enumerable.Empty<int>();
            var notChangedAndAdded = notChanged.Concat(added);


            var updateResult = await _shiftRepository.ManageEmployeesShift(sup,notChangedAndAdded,shift);


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

        public async Task DaySequenceCreate(int userid, SequenceForm sequence)
        {
            await _shiftRepository.DaySequenceCreate(userid, sequence);
        }

        public static List<int> GetISOWeekNumbers(DateTime firstSelected)
        {
            List<int> isoWeekNumbers = new List<int>();
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            DateTime firstDayOfMonth = new DateTime(firstSelected.Year, firstSelected.Month, 1);

            int firstWeekOfMonth = cal.GetWeekOfYear(firstDayOfMonth, dfi.CalendarWeekRule, DayOfWeek.Monday);

            for (int i = 0; i < 5; i++) 
            {
                int currentWeekNumber = firstWeekOfMonth + i;

                DateTime firstDayOfWeek = cal.AddWeeks(firstDayOfMonth, i);
                if (firstDayOfWeek.Month == firstSelected.Month)
                {
                    isoWeekNumbers.Add(currentWeekNumber);
                }
                else
                {
                    break;
                }
            }

            return isoWeekNumbers;
        }
    }
}
