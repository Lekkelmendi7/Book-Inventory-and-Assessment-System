using AutoMapper;
using BookInventory.BusinessLogicAcessLayer.Models.PulisherModels;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.BusinessLogicAcessLayer.Services.PublisherService
{
    public class PublisherServicee : IPublisherService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public PublisherServicee(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PublisherGetModel>> GetPublishers()
        {
            var publishers = await _context.Publishers.Include(a => a.Author).ToListAsync();
            return _mapper.Map<IEnumerable<PublisherGetModel>>(publishers);  
        }

        public async Task<PublisherGetModel> GetPublisher(int id)
        {
            var publisher = await _context.Publishers.Include(a => a.Author).FirstOrDefaultAsync(a => a.Id == id);
            if(publisher == null)
            {
                return null;
            }

            return _mapper.Map<PublisherGetModel>(publisher);
        }



        public async Task AddPublisher(PublisherCreateModel model)
        {
            var existingPublisher = await _context.Publishers
                .FirstOrDefaultAsync(c => c.AuthorId == model.AuthorId);

            if(existingPublisher != null)
            {
                throw new InvalidOperationException("This publisher has an associated author!");
            }

            var publisherEntity = new Publisher
            {
                Name = model.Name,  
                Address = model.Address,
                Website = model.Website,    
                AuthorId = model.AuthorId,  
            };
            _context.Publishers.Add(publisherEntity);
            await _context.SaveChangesAsync();
        }


        public async Task UpdatePublisher(int id, PublisherUpdateModel model)
        {
            var result = await _context.Publishers.FirstOrDefaultAsync(x => x.Id == id);
            if(result == null)
            {
                throw new InvalidOperationException("This publisher has an associated author!");
            }

            var existingPublisher = await _context.Publishers.FirstOrDefaultAsync(x => x.AuthorId == model.AuthorId && x.Id != id);

            if(existingPublisher != null)
            {
                throw new InvalidOperationException("This publisher is already associated with this author");
            }

            result.Name = model.Name;
            result.Address = model.Address; 
            result.Website = model.Website; 
            result.AuthorId = model.AuthorId;   

            _context.Publishers.Update(result);
            await _context.SaveChangesAsync();  
        }

        public async Task<bool> DeletePublisher(int id)
        {
            var publisher = await _context.Publishers.FindAsync(id);

            if (publisher == null)
            {
                throw new KeyNotFoundException($"Publisher with ID {id} not found!");
            }

            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
