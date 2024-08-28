namespace BookInventory.DataAccessLayer.Entities
{
    public class RoleUser
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }
}
