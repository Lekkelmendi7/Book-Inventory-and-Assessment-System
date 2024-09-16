using AuthorInventory.DataAccessLayer.Repository.AuthorRepository;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

using BookInventory.LogicAcessLayer.Models.AuthorModels;
using BookInventory.LogicAcessLayer.Services.AuthorService;
using BookInventory.DataAccess.Database;
using BookInventory.BusinessLogicAcessLayer.Services.FileService;
using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.DataAccess.Entities;

namespace AuthorInventory.LogicAcessLayer.Services.AuthorService
{
    public class AuthorServicee : IAuthorService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly IAuthorRepository _authorRepository;
        private readonly string containerName = "authors";

        public AuthorServicee(DatabaseContext context, IMapper mapper, IFileStorageService fileStorageService, IAuthorRepository authorRepository)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
            _authorRepository = authorRepository;
        }

        
          public async Task<PaginatedResult<AuthorGetModel>> GetAllAuthors(int page, int size, string? sortBy, string? sortOrder)
        {
            size = Math.Min(size, PaginationModel.MaxPageSize);

            try
            {
               

                // Call the repository method to get paginated and sorted results
                var paginatedAuthors = await _authorRepository.GetAllAuthors(page, size, sortBy, sortOrder);

                // Map the result using AutoMapper
                var mappedAuthors = _mapper.Map<IEnumerable<AuthorGetModel>>(paginatedAuthors.Items);

               

                // Return paginated result
                return new PaginatedResult<AuthorGetModel>
                {
                    TotalItems = paginatedAuthors.TotalItems,
                    Items = mappedAuthors,
                    TotalPages = paginatedAuthors.TotalPages,
                    CurrentPage = paginatedAuthors.CurrentPage,
                    HasPreviousPage = paginatedAuthors.HasPreviousPage,
                    HasNextPage = paginatedAuthors.HasNextPage,
                    FirstPage = paginatedAuthors.FirstPage,
                    LastPage = paginatedAuthors.LastPage
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching authors.", ex);
            }
        }
         
        


        public async Task<AuthorGetModel> GetAuthorById(int id)
        {


            var author = await _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            var authorModel = _mapper.Map<AuthorGetModel>(author);
     

            return authorModel;
        }

        public async Task CreateAuthor(AuthorCreateModel authorCreateModel)
        {
            try
            {
                var authorEntity = _mapper.Map<Author>(authorCreateModel);
                await _authorRepository.AddAuthor(authorEntity);
            }
            catch (Exception ex)
            {
                throw new Exception("Error creating author.", ex);
            }

        }

        public async Task UpdateAuthor(int id, AuthorUpdateModel authorUpdateModel)
        {

            var authorEntity = await _authorRepository.GetAuthorById(id);
            if (authorEntity == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            _mapper.Map(authorUpdateModel, authorEntity);

            await _authorRepository.UpdateAuthor(authorEntity);
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            try
            {
              

                var author = await _authorRepository.GetAuthorById(id);
                if (author == null)
                {
                    return false;
                }


               
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting author.", ex);
            }
        }

        public async Task<IEnumerable<AuthorGetModel>> GetAuthorsByNationality(string nationality)
        {
            var authors = await _authorRepository.GetAuthorsByNationalityAsync(nationality);
            var authorModels = _mapper.Map<IEnumerable<AuthorGetModel>>(authors);

            return authorModels;
        }
    }
}