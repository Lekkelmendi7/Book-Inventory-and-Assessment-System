using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models;
using BookInventory.BusinessLogicAcessLayer.Services.FileService;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.AuthorModels;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BookInventory.LogicAcessLayer.Services.AuthorService
{
    public class AuthorServicee : IAuthorService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;
        private readonly IFileStorageService _fileStorageService;
        private readonly string containerName = "authors";

        public AuthorServicee(DatabaseContext context, IMapper mapper, IFileStorageService fileStorageService)
        {
            _context = context;
            _mapper = mapper;
            _fileStorageService = fileStorageService;
        }

        public async Task<PaginatedResult<AuthorGetModel>> GetAllAuthors(int page, int size, string? sortBy, string? sortOrder)
        {
            if (size > PaginationModel.MaxPageSize)
            {
                size = PaginationModel.MaxPageSize; // Ensure page size does not exceed max limit
            }

            try
            {
                

                // Define the query
                var queryable = _context.Authors.Include(a => a.Publisher).AsQueryable();


                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    switch (sortBy.ToLower())
                    {
                        case "name":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.Name) : queryable.OrderBy(a => a.Name);
                            break;
                        case "dateofbirth":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.DateOfBirth) : queryable.OrderBy(a => a.DateOfBirth);
                            break;
                        case "nationality":
                            queryable = sortOrder == "desc" ? queryable.OrderByDescending(a => a.Nationality) : queryable.OrderBy(a => a.Nationality);
                            break;
                        default:
                            queryable = queryable.OrderBy(a => a.Name); // Default sorting
                            break;
                    }
                }
                else
                {
                    queryable = queryable.OrderBy(a => a.Name); // Default sorting if no sortBy is provided
                }

                // Apply pagination
                var paginatedAuthors = queryable.Paginate(page, size);

                // Map the results
                var mappedAuthors = _mapper.Map<IEnumerable<AuthorGetModel>>(paginatedAuthors.Items);

                

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
                
                throw new Exception("An error occurred while trying to fetch authors.", ex);
            }
        }


        public async Task<AuthorGetModel> GetAuthorById(int id)
        {

            var author = await _context.Authors.Include(a => a.Publisher).FirstOrDefaultAsync(x => x.Id == id);

            if (author == null)
            {

                throw new KeyNotFoundException($"Author with id {id} not found!");
          
            }

            var authorGetModel = new AuthorGetModel
            {
                Id = author.Id,
                Name = author.Name,
                DateOfBirth = author.DateOfBirth,
                Nationality = author.Nationality,
                EmailAddress = author.EmailAddress,
                ImageUrl = author.ImageUrl,
            };

            return authorGetModel;
        }

        public async Task CreateAuthor(AuthorCreateModel authorCreateModel)
        {

            var existingPublisher = await _context.Publishers
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == authorCreateModel.Publisher.AuthorId);

            if(existingPublisher != null)
            {
               
                throw new InvalidOperationException("This author already has an associated publisher.");
            }

            var newAuthor = _mapper.Map<Author>(authorCreateModel);


            if (authorCreateModel.ImageUrl != null)
            {
                newAuthor.ImageUrl = await _fileStorageService.SaveFile(containerName, authorCreateModel.ImageUrl);
            }

            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAuthor(int id, AuthorUpdateModel authorUpdateModel)
        {

            var authorModel = _context.Authors.Include(a => a.Publisher).FirstOrDefault(a => a.Id == id);
            if(authorModel == null)
            {
                throw new InvalidOperationException("Author not found!");
            }

            var existingPublisher = await _context.Publishers
                .Include(c => c.Author)
                .FirstOrDefaultAsync(a => a.AuthorId == authorUpdateModel.Publisher.AuthorId && a.AuthorId != id);

            if(existingPublisher != null)
            {
               
                throw new InvalidOperationException("Another author is associated with this publisher");
            }

            if(authorUpdateModel.ImageUrl != null)
            {
                authorModel.ImageUrl = await _fileStorageService.EditFile(containerName,
                    authorUpdateModel.ImageUrl, authorModel.ImageUrl);
            }


            _mapper.Map(authorUpdateModel, authorModel);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            
            var author = await _context.Authors.Include(a => a.Publisher).FirstOrDefaultAsync(a => a.Id == id);


            if (author == null)
            {
              
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            await _fileStorageService.DeleteFile(author.ImageUrl, containerName);
            return true;
        }

        public async Task<IEnumerable<AuthorGetModel>> GetAuthorsByNationality(string nationality)
        {


            var filteredAuthors = await _context.Authors.Include(x => x.Publisher).Where(x => x.Nationality == nationality).ToListAsync();
           
            if(!filteredAuthors.Any())
            {
              
                throw new KeyNotFoundException($"No authors found with nationality: {nationality}");
            }

            var mappedFilteredAuthors = _mapper.Map<IEnumerable<AuthorGetModel>>(filteredAuthors);
            return mappedFilteredAuthors;
        }
    }
}