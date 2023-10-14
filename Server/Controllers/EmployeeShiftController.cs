using Agendo.Server.Models;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Mvc;


namespace Agendo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeShiftController : ControllerBase
    {
        private readonly IEmployeeShiftService _employeeShiftService;

        public EmployeeShiftController(IEmployeeShiftService employeeShiftService)
        {
            _employeeShiftService = employeeShiftService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeShift>>> Get()
        {
            return Ok( await _employeeShiftService.GetAllAsync());
        }
        [HttpGet("{emp}")]

        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetSingle(int Emp)
        {
            return Ok( await _employeeShiftService.GetSingleEmpAsync(Emp));
        }
    }
}
