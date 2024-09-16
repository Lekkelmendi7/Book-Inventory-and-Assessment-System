

using AuthorInventory.DataAccessLayer.Repository.AuthorRepository;
using BookInventory.DataAccessLayer.Repository.AuthorRepository;
using BookInventory.DataAccessLayer.Repository.BookRepository;
using BookInventory.DataAccessLayer.Repository.Repo;

namespace BookInventory.DataAccessLayer.Repository
{
    public static class RepositoryConfiguration
    {
        public static void ConfigureRepositorys(this IServiceCollection services)
        {
            services.AddScoped<IBookRepository, BookRepositoryy>();
            services.AddScoped<IAuthorRepository, AuthorRepositoryy>();
            // services.AddScoped<IPublisherRepository, PublisherRepository>();
            //  services.AddScoped<IAuthRepository, AuthRepository>();
          /*  services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IFileStorageRepository, FileStorageRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();*/
        }
    }
}
