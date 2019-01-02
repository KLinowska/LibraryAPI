using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryAPI.Models
{
    public class HumanDto
    {
        [Required(ErrorMessage ="You should provide a name value")]
        [MaxLength(50)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
