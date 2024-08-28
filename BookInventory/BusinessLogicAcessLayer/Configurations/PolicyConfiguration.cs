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
                options.AddPolicy("Post_Read", policy => policy.RequireClaim("permissions", "Post_Read"));
                options.AddPolicy("Post_Create", policy => policy.RequireClaim("permissions", "Post_Create"));
                options.AddPolicy("Post_Edit", policy => policy.RequireClaim("permissions", "Post_Edit"));
                options.AddPolicy("Post_Delete", policy => policy.RequireClaim("permissions", "Post_Delete"));
                options.AddPolicy("Comment_Read", policy => policy.RequireClaim("permissions", "Comment_Read"));
                options.AddPolicy("Comment_Create", policy => policy.RequireClaim("permissions", "Comment_Create"));
                options.AddPolicy("Comment_Edit", policy => policy.RequireClaim("permissions", "Comment_Edit"));
                options.AddPolicy("Comment_Delete", policy => policy.RequireClaim("permissions", "Comment_Delete"));
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
