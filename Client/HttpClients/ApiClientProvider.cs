﻿using Agendo.Server.Models;
using Agendo.Shared.Form.CreateEmployeeShift;
using Agendo.Shared.Form.CreateShift;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace Agendo.Client.HttpClients
{
    public interface IApiClient
    {
       Task<IEnumerable<EmployeeShiftDTO>> GetSingleEmployeeShift(int EmpNr);
       Task<IEnumerable<DomainDTO>> GetDomains();
       Task<IEnumerable<DailyScheduleDTO>> GetDailySchedule();
       Task<IEnumerable<EmployeeShiftDTO>> GetEmployeeShifts(List<int> EmpNrs);
       Task<HttpResponseMessage> CreateEmployeeShift(CreateEmployeeShift body);
       Task<IEnumerable<DomainDTO>> GetDomainOfShift(DateTime dateOfShift, int shiftNr); 
       Task<HttpResponseMessage> PutShift(CreateEmployeeShift body);
       Task<HttpResponseMessage> PostDailySchedule(CreateShift body);
    }

    public class ApiClientProvider : IApiClient
    {

        private readonly HttpClient _httpClient;

        public ApiClientProvider(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<EmployeeShiftDTO>> GetSingleEmployeeShift(int EmpNr) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeShiftDTO>>("api/EmployeeShift?Emp=" + EmpNr);

        public async Task<IEnumerable<DomainDTO>> GetDomains() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<DomainDTO>>("api/Domain");

        public async Task<IEnumerable<DailyScheduleDTO>> GetDailySchedule() =>
            await _httpClient.GetFromJsonAsync<IEnumerable<DailyScheduleDTO>>("api/DailySchedule");

        public async Task<IEnumerable<EmployeeShiftDTO>> GetEmployeeShifts(List<int> EmpNrs) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeShiftDTO>>("/api/EmployeeShift/shiftmanagment?" +EmpArrayQuery(EmpNrs));


        Func<List<int>, string> EmpArrayQuery = (a) =>
        {
            string query = "";
            foreach (int i in a)
            {
                query += "Emps=" + i + "&";
            }
            return query[..^1];
        };
        
        public async Task<HttpResponseMessage> CreateEmployeeShift(CreateEmployeeShift body) =>
             await _httpClient.PutAsJsonAsync("api/EmployeeShift", body);

        public async Task<IEnumerable<DomainDTO>> GetDomainOfShift(DateTime dateOfShift, int shiftNr) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<DomainDTO>>($"api/domain/shiftemployees?Start={dateOfShift.ToString("yyyy-MM-ddTHH:mm:ss")}&shiftNR={shiftNr}");
    
        public async Task<HttpResponseMessage> PutShift(CreateEmployeeShift body) =>
            await _httpClient.PutAsJsonAsync("api/EmployeeShift", body);

        public async Task<HttpResponseMessage> PostDailySchedule(CreateShift body) =>
            await _httpClient.PostAsJsonAsync("api/DailySchedule", body);
    
    }
}
