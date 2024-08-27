using BookInventory.BusinessLogicAcessLayer.Models.PulisherModels;
using BookInventory.DataAccess.Entities;
using BookInventory.DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookInventory.LogicAcessLayer.Models.AuthorModels
{
    public class AuthorUpdateModel
    {
        [Required]
        [MaxLength(70)]
        public string Name { get; set; } = null!;
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Required]
        [MaxLength(70)]
        public string Nationality { get; set; } = null!;
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        [Required]
        public PublisherUpdateModel Publisher {  get; set; }    
    }
}
