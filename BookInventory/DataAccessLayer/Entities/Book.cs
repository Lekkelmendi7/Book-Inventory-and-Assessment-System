using System.ComponentModel.DataAnnotations;

namespace BookInventory.DataAccess.Entities
{
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;
        [Required]
        public int PublicationYear { get; set; }
        [Required]
        public string[] Genres { get; set; } = null!;
        [Required]
        [MaxLength(40)]
        public string Language { get; set;} = null!;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Page count wrong, it must be at least 1!")]
        public int PageCount { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage ="Price can't be a negative number!")]
        public double Price { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage ="Stock can't be negative!")]
        public int Stock { get; set; }
        [Required]
        [MaxLength(30)]
        public string Edition { get; set; } = null!;
        [Required]
        [MaxLength(20)]
        public string Format { get; set; } = null!;
        public int AuthorId { get; set; }
        public Author Author { get; set; } = null!;

    }
}
