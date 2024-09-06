using BookInventory.BusinessLogicAcessLayer.Models.PulisherModels;
using BookInventory.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookInventory.LogicAcessLayer.Models.AuthorModels
{
    public class AuthorCreateModel
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
        public IFormFile ImageUrl { get; set; } = null!;
        [Required]
        public PublisherCreateModel Publisher { get; set; }
    }
}
