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
            var authors = await _context.Authors.ToListAsync();
            return _mapper.Map<IEnumerable<AuthorGetModel>>(authors);      
        }

        public async Task<AuthorGetModel> GetAuthorById(int id)
        {
            var author = await _context.Authors.FindAsync(id);

            if(author == null)
            {
                return null;
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

        public async Task UpdateAuthor(int id, AuthorUpdateModel authorUpdateModel)
        {
            var author = await _context.Authors.FindAsync(id);

            if(author == null)
            {
                return;
            }
            author.Name = authorUpdateModel.Name;  
            author.DateOfBirth = authorUpdateModel.DateOfBirth;
            author.Nationality = authorUpdateModel.Nationality;
            author.EmailAddress = authorUpdateModel.EmailAddress;   
            author.ImageUrl = authorUpdateModel.ImageUrl;   

            _context.Authors.Update(author);
            _context.SaveChanges();  
        }

        public async Task DeleteAuthor(int id)
        {
            var author = _context.Authors.Find(id);

            if (author == null)
            {
                return;
            }

            _context.Authors.Remove(author);
            _context.SaveChanges();
        }
    }
}
