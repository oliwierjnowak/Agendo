using Agendo.Shared.DTOs;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Agendo.Shared.Form.CreateEmployeeShift;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Agendo.Server.Services.enums;
using System.Globalization;

namespace Agendo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "719,1000")]
    public class ShiftController(IShiftService _employeeShiftService) : ControllerBase
    {

        [HttpGet]
        [Authorize(Roles = "1000")]
        [Route("{Emp:int}")]
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetSingle( int Emp)
        {
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
           
            var emp = Emp == null ? userid : (int) Emp;

            return Ok(await _employeeShiftService.GetSingleEmpAsync(userid,emp));
           
        }

        [HttpGet]
        [Authorize(Roles ="719")]
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetMultiple([FromQuery] IEnumerable<int> Emps, [FromQuery] DateTime ViewFirstDay, [FromQuery] bool Together = false)
        {
            //asads
            var userid = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value;
            var x = await _employeeShiftService.GetMultipleEmpsAsync(int.Parse(userid), Emps, ViewFirstDay);
            return Ok(x);
        }

        [HttpPut]
        [Authorize(Roles = "719")]
        public async Task<ActionResult<EmployeeShiftDTO?>> ManageEmployeesShift([FromBody] CreateMultipleEmpShift empshift)
        {
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
            var managed = await _employeeShiftService.ManageMultipleEmpShift(userid, empshift);
            return Ok(managed);

        }


        private int GetISOWeeksFromMonth(DateTime day)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(new DateTime(day.Year, day.Month, 1), CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            return weekNum;
        }
    }
}

