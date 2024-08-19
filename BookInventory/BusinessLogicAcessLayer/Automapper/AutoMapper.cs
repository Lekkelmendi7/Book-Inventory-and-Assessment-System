using AutoMapper;
using BookInventory.DataAccess.Entities;
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


        }
    }
}
