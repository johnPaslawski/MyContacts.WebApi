using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyContacts.WebApi.Controllers
{
    [ApiController]
    public class ContactsController : ControllerBase
    {
        //GET http://localhost:33333/api/contacts
        [HttpGet("api/contacts")]
        public IActionResult GetContacts()
        {
            return new JsonResult(
                new List<object>()
                {
                    new { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jkowalski@gmail.com"},
                    new { Id = 1, FirstName = "Anna", LastName = "Nowak", Email = "anowak@gmail.com"}
                }
            );
        }
    }
}
