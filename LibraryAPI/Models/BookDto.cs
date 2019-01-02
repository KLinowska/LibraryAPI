using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public long ISBN { get; set; }

        public PublisherDto Publisher { get; set; } = new PublisherDto();
    }
}
