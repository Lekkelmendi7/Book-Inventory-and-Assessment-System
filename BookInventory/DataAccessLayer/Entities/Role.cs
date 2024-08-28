namespace BookInventory.DataAccessLayer.Entities
{
    public class Role
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public virtual IEnumerable<RolePermission> RolePermissions { get; set; }
        public virtual IEnumerable<RoleUser> RoleUsers { get; set; }
    }
}
