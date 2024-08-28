using System.ComponentModel.DataAnnotations;

namespace BookInventory.BusinessLogicAcessLayer.Models.RoleModel
{
    public class RoleUpdateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IEnumerable<int> UsersIds { get; set; }
        [Required]
        public IEnumerable<int> PermissionIds { get; set; }
    }
}
