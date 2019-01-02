using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Entities;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/library/authors")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private ILibraryRepository _libraryRepository;
        private LibraryContext _context;
        public BooksController(ILibraryRepository libraryRepository, LibraryContext libraryContext)
        {
            _libraryRepository = libraryRepository;
            _context = libraryContext;
        }

        [HttpGet("{authorId}/[controller]")]
        public IActionResult GetBooks(int authorId)
        {
            try
            {
                if (!_libraryRepository.AuthorExists(authorId))
                {
                    return NotFound();
                }

                var booksForAuthor = _libraryRepository.GetBooksForAuthor(authorId);
                var booksForAuthorResults = Mapper.Map<IEnumerable<BookDto>>(booksForAuthor);

                return Ok(booksForAuthorResults);

            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{authorId}/[controller]/{id}", Name = "GetBook")]
        public IActionResult GetBook(int authorId, int id)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var book = _libraryRepository.GetBookForAuthor(authorId, id);

            if (book == null)
            {
                return NotFound();
            }

            var bookResult = Mapper.Map<BookDto>(book);
            return Ok(bookResult);
        }

        [HttpPost("{authorId}/[controller]")]
        public IActionResult CreateBook(int authorId, [FromBody] BookForCreationDto book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }           

            var finalBook = Mapper.Map<Entities.Book>(book);
            _libraryRepository.AddBookForAuthor(authorId, finalBook);

            var samePublisher = _context.Publishers.FirstOrDefault(p => p.Name == book.Publisher.Name);
            if (samePublisher != null)
            {
                finalBook.PublisherId = samePublisher.Id;
            }

            if (!_libraryRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var createdBookToReturn = Mapper.Map<BookDto>(finalBook);
            return CreatedAtRoute("GetBook", new { authorId = authorId, id = createdBookToReturn.Id }, createdBookToReturn );
        }

        [HttpPut("{authorId}/[controller]/{id}")]
        public IActionResult UpdateBook(int authorId, int id, [FromBody] BookForUpdateDto book)
        {
            if (book == null)
            {
                return BadRequest();
            }
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bookEntity = _libraryRepository.GetBookForAuthor(authorId, id);
            if (bookEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(book, bookEntity);

            if (!_libraryRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPatch("{authorId}/[controller]/{id}")]
        public IActionResult PartiallyUpdateBook(int authorId, int id,
            [FromBody] JsonPatchDocument<BookForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var bookEntity = _libraryRepository.GetBookForAuthor(authorId, id);
            if (bookEntity == null)
            {
                return NotFound();
            }

            var bookToPatch = Mapper.Map<BookForUpdateDto>(bookEntity);

            patchDoc.ApplyTo(bookToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            TryValidateModel(bookToPatch);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(bookToPatch, bookEntity);

            return NoContent();
        }

        [HttpDelete("{authorId}/[controller]/{id}")]
        public IActionResult DeleteBook(int authorId, int id)
        {
            if (!_libraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var bookEntity = _libraryRepository.GetBookForAuthor(authorId, id);
            if (bookEntity == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteBook(bookEntity);
            if (!_libraryRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
