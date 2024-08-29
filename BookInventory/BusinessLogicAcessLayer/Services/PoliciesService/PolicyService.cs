namespace BookInventory.BusinessLogicAcessLayer.Services.PoliciesService
{
    public class PolicyService : IPolicyService
    {
        public void ConfigurePolicies(IServiceCollection services)
        {
            foreach (var permission in new[]
            {
                "Author_Read", "Author_Create", "Author_Update", "Author_Delete",
                "Book_Read", "Book_Create", "Book_Edit", "Book_Delete",
                "Publisher_Read", "Publisher_Create", "Publisher_Edit", "Publisher_Delete",
                "User_Read","User_Edit","User_Create","User_Delete",
                "Permission_Read", "Permission_Create", "Permission_Edit", "Permission_Delete",
                "Role_Read", "Role_Create", "Role_Edit", "Role_Delete"
            })
            {
                services.AddAuthorization(options =>
                {
                    options.AddPolicy(permission, policy => policy.RequireClaim("permissions", permission));
                });
            }
        }
    }
}
