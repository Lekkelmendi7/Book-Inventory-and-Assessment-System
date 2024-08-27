using System.ComponentModel.DataAnnotations;

namespace BookInventory.BusinessLogicAcessLayer.Models.PulisherModels
{
    public class PublisherCreateModel
    {
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
        [Range(1, int.MaxValue, ErrorMessage = "Incorrect value given!")]
        public int AuthorId { get; set; }
    }
}
