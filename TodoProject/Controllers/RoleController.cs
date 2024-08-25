using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TodoProject.Attributes;
using TodoProject.Models.DTOs;
using TodoProject.Services;

namespace TodoProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [RoleAttribute("admin", "manager")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService roleService;

        public RoleController(RoleService roleService)
        {
            this.roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await roleService.GetAll());
        }

        [HttpPut]
        [Route("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignRoleRequest body)
        {
            if (body == null) return Ok(false);
            return Ok(await roleService.AssignRole(body.userId, body.roleId));
        }

        [HttpPut]
        [Route("unassign")]
        public async Task<IActionResult> Unassign([FromBody] AssignRoleRequest body)
        {
            if (body == null) return Ok(false);
            return Ok(await roleService.UnassignRole(body.userId, body.roleId));
        }
    }
}
