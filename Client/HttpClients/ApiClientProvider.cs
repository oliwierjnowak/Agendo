﻿using Agendo.Shared.DTOs;
using Agendo.Shared.Form;
using Agendo.Shared.Form.CreateEmployeeShift;
using Agendo.Shared.Form.CreateShift;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Agendo.Client.HttpClients
{
    public interface IApiClient
    {
        Task<IEnumerable<EmployeeShiftDTO>> GetSingleEmployeeShift(int EmpNr);
        Task<IEnumerable<DomainDTO>> GetDomains();
        Task<IEnumerable<DailyScheduleDTO>> GetDailySchedule();
        Task<IEnumerable<EmployeeShiftDTO>> GetEmployeeShifts(List<int>? EmpNrs, DateTime ViewFirstDay);
        Task<IEnumerable<DomainDTO>> GetDomainOfShift(DateTime dateOfShift, int shiftNr);
        Task<HttpResponseMessage> PostDailySchedule(CreateShift body);
        Task<EmployeeShiftDTO> ManageEmployeesShift(CreateMultipleEmpShift body);
        Task<HttpResponseMessage> PostSequence(CreateSequenceForm body);

        Task<IEnumerable<EmployeeShiftDTO>> GetGroupedEmployeeShifts(List<int>? EmpNrs, DateTime ViewFirstDay);
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

        public async Task<IEnumerable<EmployeeShiftDTO>> GetEmployeeShifts(List<int>? EmpNrs, DateTime ViewFirstDay) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeShiftDTO>>("/api/Shift?" + EmpArrayQuery(EmpNrs) + $"ViewFirstDay={ViewFirstDay.ToString("yyyy-MM-ddTHH:mm:ss")}&Grouped=false");

        public async Task<IEnumerable<EmployeeShiftDTO>> GetGroupedEmployeeShifts(List<int>? EmpNrs, DateTime ViewFirstDay) =>
            await _httpClient.GetFromJsonAsync<IEnumerable<EmployeeShiftDTO>>("/api/Shift?" + EmpArrayQuery(EmpNrs) + $"ViewFirstDay={ViewFirstDay.ToString("yyyy-MM-ddTHH:mm:ss")}&Grouped=true");


        Func<List<int>?, string> EmpArrayQuery = (a) =>
        {
            if (a == null)
            {
                return "";
            }
            string query = "";
            foreach (int i in a)
            {
                query += "Emps=" + i + "&";
            }
            return query[..^1] + "&";
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

        public async Task<HttpResponseMessage> PostSequence(CreateSequenceForm body)
        {
            var jsonContent = JsonSerializer.Serialize(body);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync("api/shift", content);

        }

    }
}
