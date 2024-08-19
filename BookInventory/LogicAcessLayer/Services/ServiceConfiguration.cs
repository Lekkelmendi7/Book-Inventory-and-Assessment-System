using BookInventory.LogicAcessLayer.Services.AuthorService;
using System.Runtime.CompilerServices;

namespace BookInventory.LogicAcessLayer.Services
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorService, AuthorServicee>();    
        }
    }
}
