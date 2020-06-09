using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ContactWeb.Database;
using ContactWeb.Domain;
using ContactWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContactWeb.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactDatabase _contactDatabase;

        public ContactController(IContactDatabase contacts)
        {
            _contactDatabase = contacts;
        }

        public IActionResult Index()
        {
            var contacts = _contactDatabase.GetContacts()
                .Select(item => new ContactIndexViewModel()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                });

            return View(contacts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ContactCreateViewModel contact)
        {
            if (!TryValidateModel(contact))
            {
                return View(contact);
            }

            var contactToDb = new Contact()
            {
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                PhoneNumber = contact.PhoneNumber,
                Address = contact.Address,
                Email = contact.Email,
                Description = contact.Description,
                BirthDate = contact.BirthDate
            };

            _contactDatabase.Insert(contactToDb);

            return RedirectToAction("Index");
        }
    }
}
