using BookInventory.BusinessLogicAcessLayer.Models.PermissionModels;

namespace BookInventory.BusinessLogicAcessLayer.Services.PermissionService
{
    public interface IPermissionService
    {
        Task<IEnumerable<PermissionGetModel>> GetAllPermissions();
        Task CreatePermisssion(PermissionCreateModel permission);
        Task UpdatePermission(int id, PermissionUpdateModel permission);
        Task DeletePermission(int id);
    }
}
