using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.IdentityModel.Protocols.Configuration;

namespace BookInventory.BusinessLogicAcessLayer.Helpers
{
    public class PaginatedResult<T>
    {
        public int TotalItems { get; set; }
        public IEnumerable<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public bool HasNextPage { get; set;}
        public int FirstPage { get; set; }
        public int LastPage { get; set; }   
    }
}
