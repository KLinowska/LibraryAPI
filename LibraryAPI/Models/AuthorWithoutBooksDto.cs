using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class AuthorWithoutBooksDto : HumanDto
    {
        public int Id { get; set; }
    }
}
