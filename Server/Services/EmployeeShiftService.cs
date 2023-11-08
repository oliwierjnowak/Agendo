using Agendo.Server.Models;
using Agendo.Server.Persistance;
using Agendo.Shared.Form.CreateEmployeeShift;
using System;
using System.Globalization;

namespace Agendo.Server.Services
{
    public interface IEmployeeShiftService 
    {
        Task<int> CreateShift(CreateEmployeeShift empshift);
        Task<List<EmployeeShift>> GetAllAsync();
        Task<List<EmployeeShiftDTO>> GetMultipleEmpsAsync(IEnumerable<int> emps);
        Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int Emp);
    }
    
    
    public class EmployeeShiftService : IEmployeeShiftService
    {
        private readonly IEmployeeShiftRepository _employeeShiftRepository;

        public EmployeeShiftService(IEmployeeShiftRepository employeeShiftRepository)
        {
            _employeeShiftRepository = employeeShiftRepository;
        }

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

    public async Task<List<EmployeeShift>> GetAllAsync()
        {
            return await _employeeShiftRepository.GetAllAsync();
        }

        public async Task<List<EmployeeShiftDTO>> GetMultipleEmpsAsync(IEnumerable<int> emps)
        {
            var shifts = await _employeeShiftRepository.GetMultipleEmpsAsync(emps);
            var ShiftsDTO = new List<EmployeeShiftDTO>();
            foreach (var shift in shifts)
            {
                var fromISOWeek = ISOWeek.ToDateTime(shift.ISOYear, shift.ISOWeek, (DayOfWeek)shift.DOW);
                var startTime = new DateTime(fromISOWeek.Year, fromISOWeek.Month, fromISOWeek.Day, 8, 0, 0);

                var EmpDTO = new EmployeeShiftDTO
                {
                    EmpNr = shift.EmpNr,
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

        public async Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int Emp)
        {
            var shifts = await _employeeShiftRepository.GetSingleEmpAsync(Emp);
            var ShiftsDTO = new List<EmployeeShiftDTO>(); 
            foreach(var shift in shifts)
            {
                var fromISOWeek = ISOWeek.ToDateTime(shift.ISOYear, shift.ISOWeek, (DayOfWeek)shift.DOW);
                var startTime = new DateTime(fromISOWeek.Year, fromISOWeek.Month, fromISOWeek.Day,8,0,0);
               
                var EmpDTO = new EmployeeShiftDTO
                {
                    EmpNr = shift.EmpNr,
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
