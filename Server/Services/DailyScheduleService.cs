using Agendo.Shared.DTOs;
using Agendo.Server.Persistance;


namespace Agendo.Server.Services
{
    public interface IDailyScheduleService
    {
        Task<List<DailyScheduleDTO>> GetAllAsync();
        Task<DailyScheduleDTO> GetSingleShiftAsync(int Shift);
        Task<int> AddNewShift(string Name, int Hrs, string Color);

    }
    public class DailyScheduleService(IDailyScheduleRepository _dailyScheduleRepository) : IDailyScheduleService
    {
        public async Task<List<DailyScheduleDTO>> GetAllAsync()
        {
            return await _dailyScheduleRepository.GetAllAsync();
        }

        public async Task<DailyScheduleDTO> GetSingleShiftAsync(int Shift)
        {
            return await _dailyScheduleRepository.GetSingleShiftAsync(Shift);
     
        }

        public async Task<int> AddNewShift(string Name, int Hrs, string Color)
        {
            return await _dailyScheduleRepository.AddNewShift(Name, Hrs,Color);
        }
        
    }
}
