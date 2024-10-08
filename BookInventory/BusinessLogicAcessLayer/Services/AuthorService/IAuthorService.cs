﻿using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.LogicAcessLayer.Models.AuthorModels;
using System.Collections;
using System.Collections.Generic;

namespace BookInventory.LogicAcessLayer.Services.AuthorService
{
    public interface IAuthorService
    {
        Task<PaginatedResult<AuthorGetModel>> GetAllAuthors(int page, int size, string? sortBy, string? sortOrder);
        Task<AuthorGetModel> GetAuthorById(int id);
        Task CreateAuthor(AuthorCreateModel authorCreateModel);
        Task UpdateAuthor(int id, AuthorUpdateModel authorUpdateModel);
        Task<bool> DeleteAuthor(int id);
        Task <IEnumerable<AuthorGetModel>> GetAuthorsByNationality(string nationality);
        
    }
}
