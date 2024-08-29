using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models.RoleModel;

namespace BookInventory.BusinessLogicAcessLayer.Services.RoleService
{
    public interface IRoleService
    {
        Task<PaginatedResult<RoleGetModel>> GetRoles(int page, int size);
        Task<RoleGetModel> GetRoleById(int id);
        Task CreateRole(RoleCreateModel model);
        Task UpdateRole(int id, RoleUpdateModel model);
        Task DeleteRole(int id);
    }
}
