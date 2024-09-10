using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.AuthorModels;
using System.ComponentModel.DataAnnotations;

namespace BookInventory.LogicAcessLayer.Models.BookModels
{
    public class BookGetModel
    {
        public int Id { get; set; }
        public string Title { get; set; } 
        public int PublicationYear { get; set; }
        public string[] Genres { get; set; } 
        public string Language { get; set; }     
        public int PageCount { get; set; }      
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Edition { get; set; } 
        public string Format { get; set; }
        public string BookPhoto = "NoPhoto.png";
        public string BookPhotoUrl = "NoUrl";
        public virtual AuthorGetModel Author { get; set; }
    }
}
