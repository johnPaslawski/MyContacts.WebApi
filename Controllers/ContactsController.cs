using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
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
            //ale też możemy recznie sprawdzać poprawność jakichś danych i przesłać do usera jakąś wiadomość, np
            if (createContactDto.FirstName == createContactDto.LastName)
            {
                ModelState.AddModelError(key: "Description", errorMessage: "Imie nie moze byc takie samo jak nazwisko");
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

        [HttpPut("{id}")]
        public IActionResult UpdateContact(int id, [FromBody] UpdateContactDto updateContactDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactDto = DataService.Current.Contacts.FirstOrDefault(x => x.Id == id);

            if (contactDto == null)
            {
                return NotFound();
            }

            contactDto.FirstName = updateContactDto.FirstName;
            contactDto.LastName = updateContactDto.LastName;
            contactDto.Email = updateContactDto.Email;

            //coś musimy zwrócić, ale inaczej niż w przypadku dodawania i usuwania tutaj nie zwrócimy nic , tak 
            //sugeruje konwencja w przypadku put
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteContact(int id)
        {
            // najpierw weryfikujemy czy istnieje
            var contactDto = DataService.Current.Contacts.FirstOrDefault(x => x.Id == id);

            if (contactDto == null)
            {
                return NotFound();
            }

            DataService.Current.Contacts.Remove(contactDto);

            return NoContent();
        }

        // PATCH api/contacts/1
        // http://jsonpatch.com/
        // [
        // {
        //     "op": "add",
        //     "path": "/name",
        //     "value": "new name"
        // },
        // {
        //     "op": "replace",
        //     "path": "/description",
        //     "value": "new description"
        // }
        // ]   
        [HttpPatch("{id}")]
        public IActionResult PartialUpdateContact(int id, [FromBody] JsonPatchDocument<UpdateContactDto> patchDocument)
        {
            var contactDto = DataService.Current.Contacts.FirstOrDefault(x => x.Id == id);

            if (contactDto == null)
            {
                return NotFound();
            }

            //tworzę pomocniczy obiekt żebym miał juz odczetane istniejące dane i sie nie przejmował tymi, których akurat nie chcę zmienić
            var contactToBePatched = new UpdateContactDto
            {
                FirstName = contactDto.FirstName,
                LastName = contactDto.LastName,
                Email = contactDto.Email
            };

            //to jest najważniejsze !!!!
            patchDocument.ApplyTo(contactToBePatched);
            

            //w tym momencie też możemy recznie sprawdzać poprawność jakichś danych i przesłać do usera jakąś wiadomość, np
            if (contactToBePatched.FirstName == contactToBePatched.LastName)
            {
                ModelState.AddModelError(key: "Description", errorMessage: "Imie nie moze byc takie samo jak nazwisko");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //weryfikujemy czy nasza zmienna jest zgodna z definicją i ograniczeniami modelu
            if (!TryValidateModel(contactToBePatched))
            {
                return BadRequest(ModelState);
            }

            //teraz dokonujemy przypisania:
            contactDto.FirstName = contactToBePatched.FirstName;
            contactDto.LastName = contactToBePatched.LastName;
            contactDto.Email = contactToBePatched.Email;


            return NoContent();
        }
    }
}
