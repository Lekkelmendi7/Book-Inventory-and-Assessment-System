using BookInventory.DataAccess.Entities;
using System.ComponentModel.DataAnnotations;

namespace BookInventory.DataAccessLayer.Entities
{
    public class Publisher
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [MaxLength(150)]
        public string Address { get; set; }
        [Required]
        [MaxLength(150)]
        public string Website { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage ="Incorrect value given!")]
        public int AuthorId { get; set; }
        public Author? Author { get; set; }
    }
}
