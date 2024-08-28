using BookInventory.BusinessLogicAcessLayer.Models.AccountModels;
using BookInventory.BusinessLogicAcessLayer.Models.PermissionModels;

namespace BookInventory.BusinessLogicAcessLayer.Models.RoleModel
{
    public class RoleGetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<UserGetModel> Users { get; set; }
        public virtual IEnumerable<PermissionGetModel> Permissions { get; set; }
    }
}
