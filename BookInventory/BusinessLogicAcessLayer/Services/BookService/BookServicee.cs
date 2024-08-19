﻿using AutoMapper;
using BookInventory.DataAccess.Database;
using BookInventory.DataAccess.Entities;
using BookInventory.LogicAcessLayer.Models.BookModels;
using Microsoft.EntityFrameworkCore;

namespace BookInventory.LogicAcessLayer.Services.BookService
{
    public class BookServicee : IBookService
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public BookServicee(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookGetModel>> GetAllBooks()
        {
            try
            {
                var books = await _context.Books.Include(a => a.Author).ToListAsync();
                var mappedBooks = _mapper.Map<IEnumerable<BookGetModel>>(books);
                return mappedBooks;
            } catch(Exception ex)
            {
                throw new Exception("An error occuredd while trying to fetch books.", ex);
            }
        }

        public async Task<BookGetModel> GetBookById(int id)
        {
            var book = await _context.Books.Include(a => a.Author).FirstOrDefaultAsync(x => x.Id == id);
            if(book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found!");
            }
            var mappedBook = _mapper.Map<BookGetModel>(book);
            return mappedBook;
        }
        public async Task CreateBook(BookCreateModel bookModel)
        {
            try
            {
                var bookEntity = new Book
                {
                    Title = bookModel.Title,
                    PublicationYear = bookModel.PublicationYear,
                    Genres = bookModel.Genres,
                    Language = bookModel.Language,
                    PageCount = bookModel.PageCount,
                    Price = bookModel.Price,
                    Stock = bookModel.Stock,
                    Edition = bookModel.Edition,
                    Format = bookModel.Format,
                    AuthorId = bookModel.AuthorId,
                };

                _context.Books.Add(bookEntity);
                await _context.SaveChangesAsync();
            } catch(Exception ex)
            {
                throw new Exception("Error while trying to create a book", ex);
            }
        }


        public async Task UpdateBook(int id, BookUpdateModel bookModel)
        {
            var newBook = await _context.Books.FirstOrDefaultAsync(_ => _.Id == id);

            if(newBook == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found!");
            }
            
            newBook.Title = bookModel.Title;    
            newBook.PublicationYear = bookModel.PublicationYear;
            newBook.Genres = bookModel.Genres;
            newBook.Language = bookModel.Language;
            newBook.PageCount = bookModel.PageCount;
            newBook.Price = bookModel.Price;
            newBook.Stock = bookModel.Stock;
            newBook.Edition = bookModel.Edition;
            newBook.Format = bookModel.Format;
            newBook.AuthorId = bookModel.AuthorId;  

            _context.Books.Update(newBook);
            await _context.SaveChangesAsync();  

        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                throw new KeyNotFoundException($"Book with ID {id} not found!");
            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
