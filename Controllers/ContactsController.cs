using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyContacts.WebApi.Infrastructure;

namespace MyContacts.WebApi.Controllers
{
    [ApiController]
    // do czego ten kontroler się odnosi tutaj umieszczamy, ale można też nad metodami, kwestia przejrzystości kodu
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        //GET http://localhost:33333/api/contacts
        // dekorator zakomentowany, bo jest już w 12 linijce do całego kontrolera
        //[HttpGet("api/contacts")]
        [HttpGet]
        public IActionResult GetContacts()
        {
            var contacts = DataService.Current.Contacts;
            return Ok(contacts);
            
            //return new JsonResult(
            //    new List<object>()
            //    {
            //        new { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jkowalski@gmail.com"},
            //        new { Id = 1, FirstName = "Anna", LastName = "Nowak", Email = "anowak@gmail.com"}
            //    }
            //);
        }
    }
}
