using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models.PermissionModels;

namespace BookInventory.BusinessLogicAcessLayer.Services.PermissionService
{
    public interface IPermissionService
    {
        Task<PaginatedResult<PermissionGetModel>> GetAllPermissions(int page, int size);
        Task CreatePermisssion(PermissionCreateModel permission);
        Task UpdatePermission(int id, PermissionUpdateModel permission);
        Task DeletePermission(int id);
    }
}
