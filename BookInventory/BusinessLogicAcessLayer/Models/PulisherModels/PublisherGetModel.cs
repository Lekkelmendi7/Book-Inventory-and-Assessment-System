using BookInventory.LogicAcessLayer.Models.AuthorModels;
using System.ComponentModel.DataAnnotations;

namespace BookInventory.BusinessLogicAcessLayer.Models.PulisherModels
{
    public class PublisherGetModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Address { get; set; }
        public string Website { get; set; }
        public AuthorGetModel Author { get; set; }
    }
}
