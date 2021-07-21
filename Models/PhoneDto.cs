using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.WebApi.Models
{
    public class PhoneDto
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public string Description { get; set; }
    }
}
