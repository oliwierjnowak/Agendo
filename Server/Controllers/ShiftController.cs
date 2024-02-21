using Agendo.Shared.DTOs;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Agendo.Shared.Form.CreateEmployeeShift;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Agendo.Shared.Form;

namespace Agendo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "719,1000")]
    public class ShiftController(IShiftService _shiftService, IDomainService _domainService) : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = "1000")]
        [Route("{Emp:int}")]
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetSingle( int Emp)
        {
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
           
            var emp = Emp == null ? userid : (int) Emp;

            return Ok(await _shiftService.GetSingleEmpAsync(userid,emp));
           
        }

        [HttpGet]
        [Authorize(Roles ="719")]
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetMultiple([FromQuery] IEnumerable<int> Emps, [FromQuery] DateTime ViewFirstDay, [FromQuery] bool Grouped = true)
        {
            //asads
            var userid = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value;
            
            if ( Emps.Count() == 0)
            {
                var nrs = await _domainService.GetAllAsync(int.Parse(userid));
                Emps = nrs.Select(x => x.Nr);
            }

            var x = !Grouped ? await _shiftService.GetShiftsAsync(int.Parse(userid), Emps, ViewFirstDay) : await _shiftService.GetShiftsGroupedAsync(int.Parse(userid), Emps, ViewFirstDay);
            return Ok(x);
        }

        [HttpPut]
        [Authorize(Roles = "719")]
        public async Task<ActionResult<EmployeeShiftDTO?>> ManageEmployeesShift([FromBody] CreateMultipleEmpShift empshift)
        {
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
            var managed = await _shiftService.ManageMultipleEmpShift(userid, empshift);
            return Ok(managed);

        }

        [HttpPost]
        [Authorize(Roles = "719")]
        public async Task<ActionResult<EmployeeShiftDTO?>> DaySequenceCreate([FromForm] SequenceForm sequence )
        {
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
            await _shiftService.DaySequenceCreate(userid, sequence);
            return StatusCode(201);

        }

        [HttpPost("dates_sequence")]
        [Authorize(Roles = "719")]
        public async Task<ActionResult<EmployeeShiftDTO?>> DatesSequenceCreate([FromForm] MultipleSelectionForm sequence)
        {
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
            await _shiftService.DatesSequenceCreate(userid, sequence);
            return StatusCode(201);

        }
    }
}

