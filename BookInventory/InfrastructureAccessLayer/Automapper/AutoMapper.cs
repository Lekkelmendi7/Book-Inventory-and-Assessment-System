using AutoMapper;
using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.AuthorModels;

namespace BookInventory.InfrastructureLayer.Automapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Mapping Author entity to models and vice versa!

            CreateMap<Author, AuthorGetModel>();
            CreateMap<AuthorCreateModel, Author>();
            CreateMap<AuthorUpdateModel, Author>();


        }
    }
}
