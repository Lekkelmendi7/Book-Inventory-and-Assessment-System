using BookInventory.BusinessLogicAcessLayer.Models.RoleModel;

namespace BookInventory.BusinessLogicAcessLayer.Models.AccountModels
{
    public class UserGetModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public virtual IEnumerable<RoleGetModel> Roles { get; set; }
    }
}
