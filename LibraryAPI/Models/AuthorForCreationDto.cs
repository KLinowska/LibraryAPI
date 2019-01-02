﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class AuthorForCreationDto : HumanDto
    {
        public ICollection<BookDto> Books { get; set; } = new List<BookDto>();
    }
}
