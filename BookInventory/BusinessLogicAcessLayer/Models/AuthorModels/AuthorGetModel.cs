using BookInventory.BusinessLogicAcessLayer.Models.PulisherModels;
using BookInventory.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookInventory.LogicAcessLayer.Models.AuthorModels
{
    public class AuthorGetModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string Nationality { get; set; }
        public string EmailAddress { get; set; }
        public string ImageUrl { get; set; }
        public PublisherGetModel Publisher { get; set; }    
    }
}
