﻿using Agendo.Shared.DTOs;
using Agendo.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Agendo.Shared.Form.CreateEmployeeShift;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Agendo.Server.Services.enums;

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
        public async Task<ActionResult<IEnumerable<EmployeeShiftDTO>>> GetMultiple([FromQuery] IEnumerable<int> Emps)
        {
            var userid = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value;
            var x = Ok(await _employeeShiftService.GetMultipleEmpsAsync(int.Parse(userid), Emps));
            return x;
        }

        [HttpPut]
        [Authorize(Roles = "719")]
        public async Task<ActionResult<EmployeeShiftDTO?>> ManageShift([FromBody] CreateEmployeeShift empshift)
        {
            var userid = int.Parse(HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Actor).Value);
            var managed =  await _employeeShiftService.ManageShift(userid,empshift);
            switch(managed.code)
            {
                case ShiftPutCode.Updated: 
                    return Ok(managed.value);
                case ShiftPutCode.Deleted:
                    return StatusCode(204);
                default:
                    return BadRequest();
            }
        }
    }
}
