using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class BookForUpdateDto
    {
        [Required(ErrorMessage = "You should provide a title value")]
        [MaxLength(50)]
        public string Title { get; set; }
        public PublisherDto Publisher { get; set; }
        public long ISBN { get; set; }
    }
}
