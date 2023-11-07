using Agendo.Server.Models;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Agendo.Shared.Form.CreateEmployeeShift;
using Microsoft.AspNetCore.Authorization;
using Agendo.AuthAPI.Model;

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
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetSingle([FromQuery] int Emp, [FromQuery] int User)
        {
            var right = await _rightsService.RightsOverEmp(Emp, User);
            if (right == false) {
                return Forbid();
            }
            else
            {
                return Ok(await _employeeShiftService.GetSingleEmpAsync(Emp));
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
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetMultiple([FromBody] IEnumerable<int> Emps, [FromQuery] int User)
        {
            var rights = await _rightsService.RightsOverEmps(Emps, User);
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
