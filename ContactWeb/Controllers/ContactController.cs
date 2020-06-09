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

        public IActionResult Detail(int id)
        {
            var contactFromDb = _contactDatabase.GetContact(id);

            var contact = new ContactDetailViewModel()
            {
                FirstName = contactFromDb.FirstName,
                LastName = contactFromDb.LastName,
                PhoneNumber = contactFromDb.PhoneNumber,
                Address = contactFromDb.Address,
                Email = contactFromDb.Email,
                Description = contactFromDb.Description,
                BirthDate = contactFromDb.BirthDate
            };

            return View(contact);
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

        public IActionResult Edit(int id)
        {
            var contactFromDb = _contactDatabase.GetContact(id);

            var contact = new ContactEditViewModel()
            {
                FirstName = contactFromDb.FirstName,
                LastName = contactFromDb.LastName,
                PhoneNumber = contactFromDb.PhoneNumber,
                Address = contactFromDb.Address,
                Email = contactFromDb.Email,
                Description = contactFromDb.Description,
                BirthDate = contactFromDb.BirthDate
            };

            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(int id, ContactEditViewModel contact)
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

            _contactDatabase.Update(id, contactToDb);

            return RedirectToAction("Detail", new { Id = id });
        }

        public IActionResult Delete(int id)
        {
            var contactFromDb = _contactDatabase.GetContact(id);

            var contact = new ContactDeleteViewModel()
            {
                Id = contactFromDb.Id,
                FirstName = contactFromDb.FirstName,
                LastName = contactFromDb.LastName,
            };

            return View(contact);
        }

        [HttpPost]
        public IActionResult ConfirmDelete(int id)
        {
            _contactDatabase.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
