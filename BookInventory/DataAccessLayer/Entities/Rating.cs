using System.ComponentModel.DataAnnotations;

namespace BookInventory.DataAccess.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        [Required]
        [Range(1,10, ErrorMessage ="Rating score not correct, the rating score must be from 1 to 10!")]
        public int Rate { get; set; }
        public int BookId { get; set; } 
        public Book Book { get; set; }  
        public int UserId { get; set; } 
       // public User User {get; set;}
    }
}
