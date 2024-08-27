using BookInventory.BusinessLogicAcessLayer.Services.PublisherService;
using BookInventory.LogicAcessLayer.Services.AuthorService;
using BookInventory.LogicAcessLayer.Services.BookService;
using System.Runtime.CompilerServices;

namespace BookInventory.LogicAcessLayer.Services
{
    public static class ServiceConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthorService, AuthorServicee>();
            services.AddScoped<IBookService, BookServicee>();
            services.AddScoped<IPublisherService, PublisherServicee>();
        }
    }
}
