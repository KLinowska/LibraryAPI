using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Services
{
    public class LibraryRepository : ILibraryRepository
    {
        private LibraryContext _context;

        public LibraryRepository(LibraryContext context)
        {
            _context = context;
        }

        public void AddAuthor(Author author)
        {
            _context.Authors.Add(author);
        }

        public void AddBookForAuthor(int authorId, Book book)
        {
            var author = GetAuthor(authorId, false);
            author.Books.Add(book);
        }

        public void AddPublisher(Publisher publisher)
        {
            _context.Publishers.Add(publisher);
        }

        public bool AuthorExists(int authorId)
        {
            return _context.Authors.Any(a => a.Id == authorId);
        }

        public bool AuthorExists(Author author)
        {
            return _context.Authors.Any(a => a.FirstName == author.FirstName && a.LastName == author.LastName);
        }

        public void DeleteAuthor(Author author)
        {
            _context.Authors.Remove(author);
        }

        public void DeleteBook(Book book)
        {
            _context.Books.Remove(book);
        }

        public void DeletePublisher(Publisher publisher)
        {
            _context.Publishers.Remove(publisher);
        }

        public Author GetAuthor(int authorId, bool includeBooks)
        {
            if (includeBooks)
            {
                return _context.Authors.Include(a => a.Books)
                    .ThenInclude(b => b.Publisher)
                    .Where(a => a.Id == authorId).FirstOrDefault();
            }

            return _context.Authors.Where(a => a.Id == authorId).FirstOrDefault();
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.OrderBy(a => a.LastName).ToList();
        }

        public Book GetBookForAuthor(int authorId, int bookId)
        {
            return _context.Books.Include(b => b.Publisher).Where(b => b.AuthorId == authorId && b.Id == bookId).FirstOrDefault();
        }

        public IEnumerable<Book> GetBooksForAuthor(int authorId)
        {
            return _context.Books.Include(b => b.Publisher).Where(b => b.AuthorId == authorId).ToList();
        }

        public Publisher GetPublisher(int publisherId)
        {
            return _context.Publishers.FirstOrDefault(p => p.Id == publisherId);
        }

        public IEnumerable<Publisher> GetPublishers()
        {
            return _context.Publishers.OrderBy(p => p.Name).ToList();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
