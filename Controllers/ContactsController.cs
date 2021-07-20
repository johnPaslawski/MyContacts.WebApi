using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyContacts.WebApi.Infrastructure;
using MyContacts.WebApi.Models;

namespace MyContacts.WebApi.Controllers
{
    [ApiController]
    // do czego ten kontroler się odnosi tutaj umieszczamy, ale można też nad metodami, kwestia przejrzystości kodu
    [Route("api/contacts")]
    public class ContactsController : ControllerBase
    {
        //GET http://localhost:33333/api/contacts?like=ski
        // dekorator zakomentowany, bo api/contacts jest już w 12 linijce do całego kontrolera
        //[HttpGet("api/contacts")]
        [HttpGet]
        public IActionResult GetContacts([FromQuery] string like)
        {
            var contactsDto = DataService.Current.Contacts.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(like))
            {
                contactsDto = contactsDto.Where(x => x.Name.Contains(like));
            }

            return Ok(contactsDto);

            //return new JsonResult(
            //    new List<object>()
            //    {
            //        new { Id = 1, FirstName = "Jan", LastName = "Kowalski", Email = "jkowalski@gmail.com"},
            //        new { Id = 1, FirstName = "Anna", LastName = "Nowak", Email = "anowak@gmail.com"}
            //    }
            //);
        }

        //GET http://localhost:33333/api/contacts/1
        [HttpGet("{id:int}", Name = "GetContact")]
        public IActionResult GetContact(int id)
        {
            var contactDto = DataService.Current.Contacts.FirstOrDefault(x => x.Id == id);

            // żeby nie dostać response 206 czyli noContent, tylko 404 że nie znaleziono i że jest lipa
            if (contactDto == null)
            {
                return NotFound();
            }

            return Ok(contactDto);
        }

        [HttpPost]
        public IActionResult CreateContact([FromBody] CreateContactDto createContactDto)
        {
            //ale też możemy recznie sprawdzać poprawność danych i przesłać do usera wiadomość
            if (createContactDto.FirstName == createContactDto.LastName)
            {
                ModelState.AddModelError(key:"Description", errorMessage:"Imie nie moze byc takie samo jak nazwisko");
            }
            
            //mamy zawsze do dyspozycji właściwość ModelState i możemy ją tutaj zweryfikować-
            // czy stan tego modelu jest odpowiedni (podany błędne dane, zbyt duże itd)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var maxId = DataService.Current.Contacts.Max(x => x.Id);

            var contactDto = new ContactDto
            {
                Id = maxId + 1,
                FirstName = createContactDto.FirstName,
                LastName = createContactDto.LastName,
                Email = createContactDto.Email
            };

            DataService.Current.Contacts.Add(contactDto);

            //poniższe informacje będą zwrócone w Headers zapytania
            return CreatedAtRoute("GetContact", new { id = contactDto.Id }, contactDto);
        }
    }
}
