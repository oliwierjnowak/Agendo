using Agendo.Server.Models;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Agendo.Client.HttpClients
{
    public interface IScheduleManagementClient
    {
       Task<IEnumerable<EmployeeShiftDTO>> GetEmployeeShift(int EmpNr);
    }

    public class ScheduleManagementClient : IScheduleManagementClient
    {

        private readonly HttpClient _httpClient;

        public ScheduleManagementClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<EmployeeShiftDTO>> GetEmployeeShift(int EmpNr)
        {
            var x = await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeShiftDTO>>("api/EmployeeShift?Emp=" + EmpNr);
            return x;
        }
    }
}
