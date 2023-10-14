using Agendo.Server.Models;
using Agendo.Server.Persistance;
using System.Globalization;

namespace Agendo.Server.Services
{
    public interface IEmployeeShiftService 
    {
        Task<List<EmployeeShift>> GetAllAsync();
        Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int Emp);
    }
    
    
    public class EmployeeShiftService : IEmployeeShiftService
    {
        private readonly IEmployeeShiftRepository _employeeShiftRepository;

        public EmployeeShiftService(IEmployeeShiftRepository employeeShiftRepository)
        {
            _employeeShiftRepository = employeeShiftRepository;
        }
        public async Task<List<EmployeeShift>> GetAllAsync()
        {
            return await _employeeShiftRepository.GetAllAsync();
        }
        public async Task<List<EmployeeShiftDTO>> GetSingleEmpAsync(int Emp)
        {
            var shifts = await _employeeShiftRepository.GetSingleEmpAsync(Emp);
            var ShiftsDTO = new List<EmployeeShiftDTO>(); 
            foreach(var shift in shifts)
            {
                var fromISOWeek = ISOWeek.ToDateTime(shift.ISOYear, shift.ISOWeek, (DayOfWeek)shift.DayOfWeek);
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
