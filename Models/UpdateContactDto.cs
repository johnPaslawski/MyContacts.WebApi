using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.WebApi.Models
{
    public class UpdateContactDto
    {
        [Required]
        [MinLength(2)]
        [MaxLength(32)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(64)]
        public string LastName { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(32)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
