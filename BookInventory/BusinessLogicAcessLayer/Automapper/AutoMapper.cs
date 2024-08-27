using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Models.PulisherModels;
using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Entities;
using BookInventory.LogicAcessLayer.Models.AuthorModels;
using BookInventory.LogicAcessLayer.Models.BookModels;

namespace BookInventory.LogicAcessLayer.Automapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Mapping Author entity to models and vice versa!

            CreateMap<Author, AuthorGetModel>();
            CreateMap<AuthorCreateModel, Author>();
            CreateMap<AuthorUpdateModel, Author>();

            //Mapping Book entity to models and vice versa!
            CreateMap<Book, BookGetModel>();
            CreateMap<BookCreateModel, Book>();
            CreateMap<BookUpdateModel, Book>();

            //Mapping Book entity to models and vice versa!
            CreateMap<Publisher, PublisherGetModel>();
            CreateMap<PublisherCreateModel, Publisher>();
            CreateMap<PublisherUpdateModel, Publisher>();


        }
    }
}
