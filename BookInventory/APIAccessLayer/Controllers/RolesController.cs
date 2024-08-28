using BookInventory.BusinessLogicAcessLayer.Models.RoleModel;
using BookInventory.BusinessLogicAcessLayer.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.APIAccessLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _service;

        public RolesController(IRoleService servicee)
        {
            _service = servicee;
        }

        [HttpGet("all")]
        [Authorize(Policy = "Role_Read")]
        public async Task<IActionResult> GetAll()
        {
            var roles = await _service.GetRoles();
            return Ok(roles);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = "Role_Read")]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var role = await _service.GetRoleById(id);
            if (role == null)
            {
                return NotFound("role was not found!");
            }

            return Ok(role);
        }

        [HttpPost]
        [Authorize(Policy = "Role_Create")]
        public async Task<IActionResult> CreateRole(RoleCreateModel model)
        {
            await _service.CreateRole(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Role_Edit")]
        public async Task<IActionResult> Updaterole(int id, RoleUpdateModel role)
        {
            await _service.UpdateRole(id, role);
            return Ok("role was updated successfully!");
        }


        [HttpDelete("{id}")]
        [Authorize(Policy = "Role_Delete")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            await _service.DeleteRole(id);
            return Ok("role was deleted successfully!");

        }
    }
}
