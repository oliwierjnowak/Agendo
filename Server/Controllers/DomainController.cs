using Agendo.Server.Models;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Agendo.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DomainController : ControllerBase
    {
        private readonly IDomainService _domainService;

        public DomainController(IDomainService domainService)
        {
            _domainService = domainService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DomainDTO>>> Get()
        {
            return Ok( await _domainService.GetAllAsync());
        }
    }
}
