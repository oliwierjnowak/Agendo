using Agendo.Server.Models;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Agendo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "719,1000")]
    public class DomainController(IDomainService _domainService) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles ="719")]
        public async Task<ActionResult<IEnumerable<DomainDTO>>> Get()
        {
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
            return Ok( await _domainService.GetAllAsync(userid));
        }

        [HttpGet("string")]
        public string GetString()
        {
            return "Hello World";
        }

        [HttpGet("shiftemployees")]
        [Authorize(Roles = "719")]
        public async Task<ActionResult<IEnumerable<DomainDTO>>> GetShiftEmployees([FromQuery] DateTime Start, [FromQuery] int shiftNR)
        {
            var userid = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value;
            return Ok(await _domainService.GetShiftEmployees(int.Parse(userid), Start, shiftNR));
        }

    }
}
