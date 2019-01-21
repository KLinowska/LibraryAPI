using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LibraryAPI.Models;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/library/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private ILibraryRepository _libraryRepository;
        public PublishersController(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }
        [HttpGet]
        public IActionResult GetPublishers()
        {
            try
            {
                var publishers = _libraryRepository.GetPublishers();
                var publishersResult = Mapper.Map<IEnumerable<PublisherDto>>(publishers);

                return Ok(publishersResult);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{id}", Name = "GetPublisher")]
        public IActionResult GetPublisher(int id)
        {
            try
            {
                var publisher = _libraryRepository.GetPublisher(id);
                if (publisher == null)
                {
                    return NotFound();
                }
                var publisherResult = Mapper.Map<PublisherDto>(publisher);

                return Ok(publisherResult);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPost]
        public IActionResult CreatePublisher([FromBody] PublisherForCreationDto publisherDto)
        {
            try
            {
                if (publisherDto == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var publisherEntity = Mapper.Map<Entities.Publisher>(publisherDto);
                _libraryRepository.AddPublisher(publisherEntity);

                if (!_libraryRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                var createdPublisherToReturn = Mapper.Map<PublisherDto>(publisherEntity);
                return CreatedAtRoute("GetPublisher", new { id = createdPublisherToReturn.Id }, createdPublisherToReturn);
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePublisher(int id, [FromBody] PublisherForCreationDto publisherDto)
        {
            try
            {
                if (publisherDto == null)
                {
                    return BadRequest();
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var publisherEntity = _libraryRepository.GetPublisher(id);
                if (publisherEntity == null)
                {
                    return NotFound();
                }

                Mapper.Map(publisherDto, publisherEntity);                

                if (!_libraryRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePublisher(int id)
        {
            try
            {
                var publisherEntity = _libraryRepository.GetPublisher(id);
                if (publisherEntity == null)
                {
                    return NotFound();
                }
                _libraryRepository.DeletePublisher(publisherEntity);

                if (!_libraryRepository.Save())
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, "A problem happened while handling your request.");                
            }
        }
    }
}
