using Agendo.Server.Persistance;
using Agendo.Shared.DTOs;


namespace Agendo.Server.Services
{
    public interface IDailyScheduleService
    {
        Task<List<DailyScheduleDTO>> GetAllAsync();
        Task<List<DailyScheduleDTO>> GetSingleShiftAsync(int Shift);
        Task<int> AddNewShift(string Name, int Hrs);

    }
    public class DailyScheduleService : IDailyScheduleService
    {
        private readonly IDailyScheduleRepository _dailyScheduleRepository;

        public DailyScheduleService(IDailyScheduleRepository dailyScheduleRepository)
        {
            _dailyScheduleRepository = dailyScheduleRepository;
        }

        public async Task<List<DailyScheduleDTO>> GetAllAsync()
        {
            return await _dailyScheduleRepository.GetAllAsync();
        }

        public async Task<List<DailyScheduleDTO>> GetSingleShiftAsync(int Shift)
        {
            return await _dailyScheduleRepository.GetSingleShiftAsync(Shift);
     
        }

        public async Task<int> AddNewShift(string Name, int Hrs)
        {
            return await _dailyScheduleRepository.AddNewShift(Name, Hrs);
        }
        
    }
}
