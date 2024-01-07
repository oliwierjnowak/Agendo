using Agendo.Shared.DTOs;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Agendo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "719,1000")]
    public class DailyScheduleController(IDailyScheduleService _dailyScheduleService) : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = "719,1000")]
        public async Task<ActionResult<IEnumerable<DailyScheduleDTO>>> Get()
        {
            return Ok(await _dailyScheduleService.GetAllAsync());
        }

        [HttpGet("{shift}")]
        [Authorize(Roles = "719,1000")]
        public async Task<ActionResult<IEnumerable<DailyScheduleDTO>>> GetSingle(int Shift)
        {
            return Ok(await _dailyScheduleService.GetSingleShiftAsync(Shift));
        }

        [HttpPost]
        [Authorize(Roles ="719")]
        public async Task<ActionResult<IEnumerable<DailyScheduleDTO>>> AddNewShift([FromBody] DailyScheduleDTO dailyScheduleDTO)
        {
            return Ok(await _dailyScheduleService.AddNewShift(dailyScheduleDTO.Name, dailyScheduleDTO.Hours, dailyScheduleDTO.Color));
        }
    }
}
