namespace BookInventory.BusinessLogicAcessLayer.Configurations
{
    public static class PolicyConfiguration
    {
        public static void ConfigurePolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Author_Read", policy => policy.RequireClaim("permissions", "Author_Read"));
                options.AddPolicy("Author_Create", policy => policy.RequireClaim("permissions", "Author_Create"));
                options.AddPolicy("Author_Update", policy => policy.RequireClaim("permissions", "Author_Update"));
                options.AddPolicy("Author_Delete", policy => policy.RequireClaim("permissions", "Author_Delete"));
                options.AddPolicy("Book_Read", policy => policy.RequireClaim("permissions", "Book_Read"));
                options.AddPolicy("Book_Create", policy => policy.RequireClaim("permissions", "Book_Create"));
                options.AddPolicy("Book_Edit", policy => policy.RequireClaim("permissions", "Book_Edit"));
                options.AddPolicy("Book_Delete", policy => policy.RequireClaim("permissions", "Book_Delete"));
                options.AddPolicy("Publisher_Read", policy => policy.RequireClaim("permissions", "Publisher_Read"));
                options.AddPolicy("Publisher_Create", policy => policy.RequireClaim("permissions", "Publisher_Create"));
                options.AddPolicy("Publisher_Edit", policy => policy.RequireClaim("permissions", "Publisher_Edit"));
                options.AddPolicy("Publisher_Delete", policy => policy.RequireClaim("permissions", "Publisher_Delete"));
                options.AddPolicy("User_Read", policy => policy.RequireClaim("permissions", "User_Read"));
                options.AddPolicy("User_Edit", policy => policy.RequireClaim("permissions", "User_Edit"));
                options.AddPolicy("User_Create", policy => policy.RequireClaim("permissions", "User_Create"));
                options.AddPolicy("User_Delete", policy => policy.RequireClaim("permissions", "User_Delete"));
                options.AddPolicy("Permission_Read", policy => policy.RequireClaim("permissions", "Permission_Read"));
                options.AddPolicy("Permission_Create", policy => policy.RequireClaim("permissions", "Permission_Create"));
                options.AddPolicy("Permission_Edit", policy => policy.RequireClaim("permissions", "Permission_Edit"));
                options.AddPolicy("Permission_Delete", policy => policy.RequireClaim("permissions", "Permission_Delete"));
                options.AddPolicy("Role_Read", policy => policy.RequireClaim("permissions", "Role_Read"));
                options.AddPolicy("Role_Create", policy => policy.RequireClaim("permissions", "Role_Create"));
                options.AddPolicy("Role_Edit", policy => policy.RequireClaim("permissions", "Role_Edit"));
                options.AddPolicy("Role_Delete", policy => policy.RequireClaim("permissions", "Role_Delete"));
            });
        }
    }
}
