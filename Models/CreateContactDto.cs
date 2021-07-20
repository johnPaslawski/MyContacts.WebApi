using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.WebApi.Models
{
    public class CreateContactDto
    {
        //ID chce generowac automatycznie, a Name nie chcę ustawiać - to różnice w stosunku do ContactDto
        
        // druga sprawa to dekoratory(atrybuty) nad propertami. Chcę się zabezpieczyć w przypadku
        // kilku kwestii, np przekazania nulla lub zabyt długich dziwnych wartości:
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
