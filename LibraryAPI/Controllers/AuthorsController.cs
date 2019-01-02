using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/library/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private ILibraryRepository _libraryRepository;

        public AuthorsController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authorsEntities = _libraryRepository.GetAuthors();
            var result = Mapper.Map<IEnumerable<AuthorWithoutBooksDto>>(authorsEntities);
            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetAuthor")]
        public IActionResult GetAuthor(int id, bool includeBooks = false)
        {
            var author = _libraryRepository.GetAuthor(id, includeBooks);
            if (author == null)
            {
                return NotFound();
            }
            if (includeBooks)
            {
                var authorResult = Mapper.Map<AuthorDto>(author);
                return Ok(authorResult);
            }
            var authorWithoutBooks = Mapper.Map<AuthorWithoutBooksDto>(author);
            return Ok(authorWithoutBooks);
        }

        [HttpPost]
        public IActionResult CreateAuthor([FromBody] AuthorForCreationDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var finalAuthor = Mapper.Map<Entities.Author>(author);

            if (_libraryRepository.AuthorExists(finalAuthor))
            {
                return StatusCode(409, "Author already exists");
            }

            _libraryRepository.AddAuthor(finalAuthor);

            if (!_libraryRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
           
            var createdAuthorToReturn = Mapper.Map<AuthorDto>(finalAuthor);
            return CreatedAtRoute("GetAuthor", new { id = createdAuthorToReturn.Id, includeBooks = false}, createdAuthorToReturn);

        }

        [HttpPut("{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] AuthorForCreationDto author)
        {
            if (author == null)
            {
                return BadRequest();
            }
            if (!_libraryRepository.AuthorExists(id))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authorEntity = _libraryRepository.GetAuthor(id, false);
            if (authorEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(author, authorEntity);

            if (!_libraryRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            if (!_libraryRepository.AuthorExists(id))
            {
                return NotFound();
            }
            var authorEntity = _libraryRepository.GetAuthor(id, true);
            if (authorEntity == null)
            {
                return NotFound();
            }

            _libraryRepository.DeleteAuthor(authorEntity);
            if (!_libraryRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }
    }
}
