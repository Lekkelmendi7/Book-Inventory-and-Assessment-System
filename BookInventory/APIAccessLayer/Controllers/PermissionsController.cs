using BookInventory.BusinessLogicAcessLayer.Models.PermissionModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookInventory.APIAccessLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionService _service;

        public PermissionsController(IPermissionService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        [Authorize(Policy = "Permission_Read")]
        public async Task<IActionResult> GetPermissions()
        {
            var permissions = await _service.GetAllPermissions();
            return Ok(permissions);
        }

        [HttpPost]
        [Authorize(Policy = "Permission_Create")]
        public async Task<IActionResult> CreatePermission(PermissionCreateModel model)
        {
            await _service.CreatePermisssion(model);
            return Ok(model);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "Permission_Edit")]
        public async Task<IActionResult> UpdatePermission(PermissionUpdateModel model, int id)
        {
            await _service.UpdatePermission(id, model);
            return Ok("Permission updated");
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Permission_Delete")]
        public async Task<IActionResult> DeletePermission(int id)
        {
            await _service.DeletePermission(id);
            return Ok("Permission was deleted successfully!");

        }

    }
}
