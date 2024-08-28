using BookInventory.BusinessLogicAcessLayer.Models.RoleModel;

namespace BookInventory.BusinessLogicAcessLayer.Models.PermissionModels
{
    public class PermissionGetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RoleGetModel> Roles { get; set; }
    }
}
