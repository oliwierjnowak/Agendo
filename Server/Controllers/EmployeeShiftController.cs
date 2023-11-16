using Agendo.Server.Models;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Agendo.Shared.Form.CreateEmployeeShift;
using Microsoft.AspNetCore.Authorization;
using Agendo.AuthAPI.Model;
using System.Security.Claims;

namespace Agendo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeShiftController : ControllerBase
    {
        private readonly IEmployeeShiftService _employeeShiftService;
        private readonly IRightsService _rightsService;
        public EmployeeShiftController(IEmployeeShiftService employeeShiftService, IRightsService rightsService)
        {
            _employeeShiftService = employeeShiftService;
            _rightsService = rightsService;
        }


        [HttpGet]
        [Authorize(Roles = "719,1000")]
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetSingle([FromQuery] int? Emp)
        {
          
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
            var emp = Emp == null ? userid : (int) Emp;
            
            var right = await _rightsService.RightsOverEmp(emp, userid);
            if (right == false && userid != emp) {
                return Forbid();
            }
            else
            {
                return Ok(await _employeeShiftService.GetSingleEmpAsync(emp));
            }
           
        }

        [HttpPut]
        [Authorize(Roles = "719")]
        public async Task<int> Create([FromBody] CreateEmployeeShift empshift )
        {
            var x = await _employeeShiftService.CreateShift(empshift);
            return x;
        }

        [HttpGet("shiftmanagment")]
        [Authorize(Roles ="719")]
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetMultiple([FromBody] IEnumerable<int> Emps)
        {
            var userid = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value;
            var rights = await _rightsService.RightsOverEmps(Emps, int.Parse(userid));
            if (rights == false)
            {
                return Forbid();
            }
            else
            {
                return Ok(await _employeeShiftService.GetMultipleEmpsAsync(Emps));
            }
        }

    }
}
