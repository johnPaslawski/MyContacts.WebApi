using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.WebApi.Models
{
    public class UpdatePhoneDto
    {
        
        public string Number { get; set; }
        public string Description { get; set; }
    }
}
