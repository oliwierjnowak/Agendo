using Agendo.Server.Services;
using Agendo.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;


namespace Agendo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyScheduleController : ControllerBase
    {
        private readonly IDailyScheduleService _dailyScheduleService;

        public DailyScheduleController(IDailyScheduleService dailyScheduleService)
        {
            _dailyScheduleService = dailyScheduleService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DailyScheduleDTO>>> Get()
        {
            return Ok(await _dailyScheduleService.GetAllAsync());
        }

        [HttpGet("{shift}")]
        public async Task<ActionResult<IEnumerable<DailyScheduleDTO>>> GetSingle(int Shift)
        {
            return Ok(await _dailyScheduleService.GetSingleShiftAsync(Shift));
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<DailyScheduleDTO>>> AddNewShift([FromBody] DailyScheduleDTO dailyScheduleDTO)
        {
            return Ok(await _dailyScheduleService.AddNewShift(dailyScheduleDTO.Name, dailyScheduleDTO.Hours));
        }
    }
}
