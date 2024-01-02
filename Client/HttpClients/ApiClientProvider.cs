using Agendo.Shared.DTOs;
using Agendo.Shared.Form.CreateEmployeeShift;
using Agendo.Shared.Form.CreateShift;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using static System.Net.WebRequestMethods;

namespace Agendo.Client.HttpClients
{
    public interface IApiClient
    {
       Task<IEnumerable<EmployeeShiftDTO>> GetSingleEmployeeShift(int EmpNr);
       Task<IEnumerable<DomainDTO>> GetDomains();
       Task<IEnumerable<DailyScheduleDTO>> GetDailySchedule();
       Task<IEnumerable<EmployeeShiftDTO>> GetEmployeeShifts(List<int> EmpNrs, DateTime ViewFirstDay);
       Task<IEnumerable<DomainDTO>> GetDomainOfShift(DateTime dateOfShift, int shiftNr);
       Task<HttpResponseMessage> PostDailySchedule(CreateShift body);
       Task<EmployeeShiftDTO> ManageEmployeesShift(CreateMultipleEmpShift body);
    }

    public class ApiClientProvider : IApiClient
    {

        private readonly HttpClient _httpClient;

        public ApiClientProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<EmployeeShiftDTO>> GetSingleEmployeeShift(int EmpNr) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeShiftDTO>>("api/Shift/" + EmpNr);

        public async Task<IEnumerable<DomainDTO>> GetDomains() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<DomainDTO>>("api/Domain");

        public async Task<IEnumerable<DailyScheduleDTO>> GetDailySchedule() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<DailyScheduleDTO>>("api/DailySchedule");

        public async Task<IEnumerable<EmployeeShiftDTO>> GetEmployeeShifts(List<int> EmpNrs, DateTime FirstDayOfMonth) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeShiftDTO>>("/api/Shift?" +EmpArrayQuery(EmpNrs)+$"&FirstDayOfMonth={FirstDayOfMonth.ToString("yyyy-MM-ddTHH:mm:ss")}");


        Func<List<int>, string> EmpArrayQuery = (a) =>
        {
            string query = "";
            foreach (int i in a)
            {
                query += "Emps=" + i + "&";
            }
            return query[..^1];
        };
        
        public async Task<EmployeeShiftDTO> ManageEmployeesShift(CreateMultipleEmpShift body)
        {
            var response = await _httpClient.PutAsJsonAsync("api/shift", body);

            return await response.Content.ReadFromJsonAsync<EmployeeShiftDTO>();
        }

        public async Task<IEnumerable<DomainDTO>> GetDomainOfShift(DateTime dateOfShift, int shiftNr) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<DomainDTO>>($"api/domain/shiftemployees?Start={dateOfShift.ToString("yyyy-MM-ddTHH:mm:ss")}&shiftNR={shiftNr}");
    

        public async Task<HttpResponseMessage> PostDailySchedule(CreateShift body) =>
            await _httpClient.PostAsJsonAsync("api/DailySchedule", body);
    
    }
}
