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
        private readonly ILogger<AuthorServicee> _logger;

        public AuthorServicee(DatabaseContext context, IMapper mapper, ILogger<AuthorServicee> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<AuthorGetModel>> GetAllAuthors()
        {
            try
            {
                _logger.LogInformation("Fetching all authors!");
                var authors = await _context.Authors.Include(a => a.Publisher).ToListAsync();
                return _mapper.Map<IEnumerable<AuthorGetModel>>(authors);
            }
            catch (Exception ex)
            {
                _logger.LogError("An error occurred whilke trying to fetch all authors!");
                throw new Exception("An error occurred while fetching authors", ex);
            }
        }

        public async Task<AuthorGetModel> GetAuthorById(int id)
        {
            _logger.LogInformation($"Fetching the author with the id: {id}");
            var author = await _context.Authors.Include(a => a.Publisher).FirstOrDefaultAsync(x => x.Id == id);

            if (author == null)
            {
                _logger.LogWarning($"Author with id {id} not found!");
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
            _logger.LogInformation($"Creating an author!");
            var existingPublisher = await _context.Publishers
                .Include(a => a.Author)
                .FirstOrDefaultAsync(a => a.Id == authorCreateModel.Publisher.AuthorId);

            if(existingPublisher != null)
            {
                _logger.LogWarning("This author already has an associated publisher.");
                throw new InvalidOperationException("This author already has an associated publisher.");
            }

            var newAuthor = _mapper.Map<Author>(authorCreateModel);
            _context.Authors.Add(newAuthor);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Author created!");
        }

        public async Task UpdateAuthor(int id, AuthorUpdateModel authorUpdateModel)
        {
            _logger.LogInformation("Trying to update the author!");
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
                _logger.LogWarning("Another author is associated with this publisher");
                throw new InvalidOperationException("Another author is associated with this publisher");
            }

            _mapper.Map(authorUpdateModel, authorModel);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAuthor(int id)
        {
            _logger.LogInformation("Trying to delete the author!");
            var author = await _context.Authors.Include(a => a.Publisher).FirstOrDefaultAsync(a => a.Id == id);


            if (author == null)
            {
                _logger.LogInformation($"Author with ID {id} not found.");
                throw new KeyNotFoundException($"Author with ID {id} not found.");
            }

            _context.Authors.Remove(author);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Author with ID {id} deleted successfully.");
            return true;
        }
    }
}