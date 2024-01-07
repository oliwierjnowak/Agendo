using Agendo.Shared.DTOs;
using Agendo.Server.Persistance;
using System.Globalization;

namespace Agendo.Server.Services
{
    public interface IDomainService
    {
        Task<List<DomainDTO>> GetAllAsync(int superior);
        Task<List<DomainDTO>> GetListAsync(int superior, IEnumerable<int> domains);
        Task<IEnumerable<DomainDTO>> GetShiftEmployees(int user, DateTime start, int shiftNR);
    }

    public class DomainService(IDomainRepository _domainRepository) : IDomainService
    {

        public async Task<List<DomainDTO>> GetAllAsync(int superior)
        {
            return await _domainRepository.GetAllAsync(superior);
        }
        public async Task<List<DomainDTO>> GetListAsync(int superior, IEnumerable<int> domains)
        {
            return await _domainRepository.GetListAsync(superior,domains);
        }

        public async Task<IEnumerable<DomainDTO>> GetShiftEmployees(int user, DateTime start, int shiftNR)
        {
            var year = start.Year;

            // Create an instance of the GregorianCalendar class with the ISO 8601 rule
            Calendar calendar = new GregorianCalendar(GregorianCalendarTypes.USEnglish);

            // Get ISO week number
            int isoWeekNumber = calendar.GetWeekOfYear(start, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            DayOfWeek dayOfWeek = start.DayOfWeek;
            var employees = await _domainRepository.GetShiftEmployees(user, year, isoWeekNumber, dayOfWeek, shiftNR);
            return employees;
        }
    }
}
