using BookInventory.LogicAcessLayer.Models.AuthorModels;
using System.Collections;
using System.Collections.Generic;

namespace BookInventory.LogicAcessLayer.Services.AuthorService
{
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorGetModel>> GetAllAuthors();
        Task<AuthorGetModel> GetAuthorById(int id);
        Task CreateAuthor(AuthorCreateModel authorCreateModel);
        Task UpdateAuthor(int id, AuthorUpdateModel authorUpdateModel);
        Task<bool> DeleteAuthor(int id);
    }
}
