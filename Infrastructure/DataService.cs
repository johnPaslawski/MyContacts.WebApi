using MyContacts.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyContacts.WebApi.Infrastructure
{
    public class DataService
    {
        public static DataService Current { get; } = new DataService();
        public List<ContactDto> Contacts { get; set; }
        public DataService()
        {
            Contacts = new List<ContactDto>
            {
                new ContactDto
                {
                    Id = 1,
                    FirstName = "Jan",
                    LastName = "Kowalski",
                    Email = "jkowalski@gmail.com",
                },
                new ContactDto
                {
                    Id = 2,
                    FirstName = "Ania",
                    LastName = "Nowak",
                    Email = "anowak@gmail.com",
                },
                new ContactDto
                {
                    Id = 3,
                    FirstName = "Bartosz",
                    LastName = "Tomaszewski",
                    Email = "btomaszewski@gmail.com",
                }
            };
        }
    }
}
