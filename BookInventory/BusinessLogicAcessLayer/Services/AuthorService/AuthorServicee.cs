using AutoMapper;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.AuthorModels;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.LogicAcessLayer.Services.AuthorService
{
    public class AuthorServicee : IAuthorService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public AuthorServicee(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AuthorGetModel>> GetAllAuthors()
        {
            try
            {
                var authors = await _context.Authors.ToListAsync();
                return _mapper.Map<IEnumerable<AuthorGetModel>>(authors);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching authors", ex);
            }
        }

        public async Task<AuthorGetModel> GetAuthorById(int id)
        {
            var author = await _context.Authors.FindAsync(id);

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
            try
            {
                var newAuthor = new Author
                {
                    Name = authorCreateModel.Name,
                    DateOfBirth = authorCreateModel.DateOfBirth,
                    Nationality = authorCreateModel.Nationality,
                    EmailAddress = authorCreateModel.EmailAddress,
                    ImageUrl = authorCreateModel.ImageUrl,
                };
                _context.Authors.Add(newAuthor);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the author", ex);
            }
        }

        public async Task UpdateAuthor(int id, AuthorUpdateModel authorUpdateModel)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with id {id} not found!");
            }
            author.Name = authorUpdateModel.Name;
            author.DateOfBirth = authorUpdateModel.DateOfBirth;
            author.Nationality = authorUpdateModel.Nationality;
            author.EmailAddress = authorUpdateModel.EmailAddress;
            author.ImageUrl = authorUpdateModel.ImageUrl;

            _context.Authors.Update(author);
            _context.SaveChanges();
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if (author == null)
            {
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}