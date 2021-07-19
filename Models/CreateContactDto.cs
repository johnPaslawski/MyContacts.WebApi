using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.WebApi.Models
{
    public class CreateContactDto
    {
        //ID chce generowac automatycznie, a Name nie chcę ustawiać - to różnice w stosunku do ContactDto
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
