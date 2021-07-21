using Microsoft.AspNetCore.Mvc;
using MyContacts.WebApi.Infrastructure;
using MyContacts.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.WebApi.Controllers
{
    [ApiController]
    [Route("api/contacts/{contactId:int}/phones")]
    public class PhonesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetPhones(int contactId)
        {
            var contactDto = DataService.Current.Contacts.FirstOrDefault(x => x.Id == contactId);

            if (contactDto == null)
            {
                return NotFound(contactDto);
            }

            return Ok(contactDto.Phones);
        }

        [HttpGet("{id}", Name = "GetPhone")]
        public IActionResult GetPhone(int contactId, int id)
        {
            var contactDto = DataService.Current.Contacts.FirstOrDefault(x => x.Id == contactId);

            if (contactDto == null)
            {
                return NotFound(contactDto);
            }

            var phoneDto = contactDto.Phones.FirstOrDefault(x => x.Id == id);

            if (phoneDto == null)
            {
                return NotFound();
            }

            return Ok(phoneDto);
        }

        [HttpPost]
        public IActionResult CreatePhone(int contactId, [FromBody] CreatePhoneDto createPhoneDto)
        {
            var maxId = DataService.Current.Contacts
                .FirstOrDefault(x => x.Id == contactId)
                .Phones
                .Max(x => x.Id);

            var phoneDto = new PhoneDto
            {
                Id = maxId + 1,
                Number = createPhoneDto.Number,
                Description = createPhoneDto.Description
            };

            var contactDto = DataService.Current.Contacts.FirstOrDefault(x => x.Id == contactId);

            contactDto.Phones.Add(phoneDto);

            return CreatedAtRoute("GetPhone", new { contactId = contactId, id = phoneDto.Id }, phoneDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePhone(int contactId, int id, [FromBody] UpdatePhoneDto updatePhoneDto)
        {
            var contactDtoPhone = DataService.Current.Contacts
                .FirstOrDefault(x => x.Id == contactId)
                .Phones
                .FirstOrDefault(x => x.Id == id);

            contactDtoPhone.Number = updatePhoneDto.Number;
            contactDtoPhone.Description = updatePhoneDto.Description;

            return NoContent();
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeletePhone(int contactId, int id)
        {
            var contactDtoPhone = DataService.Current.Contacts
                .FirstOrDefault(x => x.Id == contactId)
                .Phones
                .FirstOrDefault(x => x.Id == id);

            DataService.Current.Contacts
                .FirstOrDefault(x => x.Id == contactId)
                .Phones
                .Remove(contactDtoPhone);

            return NoContent();
        }

        //[HttpPatch]

    }
}
