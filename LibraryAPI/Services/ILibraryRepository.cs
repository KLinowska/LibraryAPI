﻿using LibraryAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Services
{
    public interface ILibraryRepository
    {
        bool AuthorExists(int authorId);
        bool AuthorExists(Author author);
        IEnumerable<Author> GetAuthors();
        Author GetAuthor(int authorId, bool includeBooks);
        IEnumerable<Book> GetBooksForAuthor(int authorId);
        Book GetBookForAuthor(int authorId, int bookId);
        void AddBookForAuthor(int authorId, Book book);
        void AddAuthor(Author author);
        void DeleteBook(Book book);
        void DeleteAuthor(Author author);
        bool Save();
    }
}
