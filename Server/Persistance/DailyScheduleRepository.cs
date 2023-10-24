using Dapper;
using System.Data;
using Agendo.Shared.DTOs;

namespace Agendo.Server.Persistance
{
    public interface IDailyScheduleRepository
    {
        Task<List<DailyScheduleDTO>> GetAllAsync();

        Task<List<DailyScheduleDTO>> GetSingleShiftAsync(int Shift);

        Task<int> AddNewShift(string Name, int Hrs);

        
    }

    public class DailyScheduleRepository : IDailyScheduleRepository
    {
        private IDbConnection _connection;

        public DailyScheduleRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public async Task<List<DailyScheduleDTO>> GetAllAsync()
        {
            _connection.Open();
            string selectQuery = "select ds_no AS 'Nr',ds_name AS 'Name' ,ds_hours AS 'Hours'   from [dbo].[csti_daily_schedule]";
            IEnumerable<DailyScheduleDTO> data = await _connection.QueryAsync<DailyScheduleDTO>(selectQuery);
            _connection.Close();
            return (List<DailyScheduleDTO>)data;
        }

        public async Task<List<DailyScheduleDTO>> GetSingleShiftAsync(int Shift)
        {
            string shiftQuery = @$"select ds_no AS 'Nr',ds_name AS 'Name' ,ds_hours AS 'Hours' from [dbo].[csti_daily_schedule] where ds_no = {Shift}";
            _connection.Open();
            string selectQuery = shiftQuery;
            IEnumerable<DailyScheduleDTO> data = await _connection.QueryAsync<DailyScheduleDTO>(selectQuery);
            _connection.Close();
            return (List<DailyScheduleDTO>)data;
        }

        public async Task<int> AddNewShift(string Name, int Hrs)
        {
            string shiftAdd = $@"insert into [dbo].[csti_daily_schedule] ([ds_name], [ds_hours]) values ( @name, @hrs)";

            string insertQuery = shiftAdd;

            int data = await _connection.ExecuteAsync(insertQuery, new
            {
                name = Name,
                hrs = Hrs
            });
            _connection.Close();
            return data;
        }
    }
}
