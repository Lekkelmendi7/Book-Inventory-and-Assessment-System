using System.ComponentModel.DataAnnotations;

namespace BookInventory.BusinessLogicAcessLayer.Models.PermissionModels
{
    public class PermissionUpdateModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public IEnumerable<int> RoleIds { get; set; }
    }
}
