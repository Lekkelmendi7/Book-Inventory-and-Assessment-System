using BookInventory.DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookInventory.DataAccess.Entities
{
    public class Author
    {
        public int Id { get; set; }
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
        public IEnumerable<Book> Books { get; set; } = null!;
        public Publisher? Publisher { get; set; }

    }
}
