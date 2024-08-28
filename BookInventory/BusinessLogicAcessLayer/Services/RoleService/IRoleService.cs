using BookInventory.BusinessLogicAcessLayer.Models.RoleModel;

namespace BookInventory.BusinessLogicAcessLayer.Services.RoleService
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleGetModel>> GetRoles();
        Task<RoleGetModel> GetRoleById(int id);
        Task CreateRole(RoleCreateModel model);
        Task UpdateRole(int id, RoleUpdateModel model);
        Task DeleteRole(int id);
    }
}
