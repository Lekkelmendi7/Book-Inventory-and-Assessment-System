namespace BookInventory.DataAccessLayer.Entities
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RolePermission> RolePermissions { get; set; }
    }
}
