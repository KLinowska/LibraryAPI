using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class AuthorDto : HumanDto
    {
        public int Id { get; set; }
        public ICollection<BookDto> Books { get; set; } = new List<BookDto>();
    }
}
