using Microsoft.AspNetCore.Mvc;
using MyContacts.WebApi.Infrastructure;
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

    }
}
