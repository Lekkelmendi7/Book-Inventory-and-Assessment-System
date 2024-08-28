using System.ComponentModel.DataAnnotations;

namespace BookInventory.BusinessLogicAcessLayer.Models.AccountModels
{
    public class UserUpdateModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        [Required]
        public IEnumerable<int> RolesIds { get; set; }
    }
}
