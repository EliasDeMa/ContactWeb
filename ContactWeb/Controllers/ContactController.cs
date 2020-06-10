using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ContactWeb.Database;
using ContactWeb.Domain;
using ContactWeb.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactWeb.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactDatabase _contactDatabase;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ContactController(IContactDatabase contacts, IWebHostEnvironment hostEnvironment)
        {
            _contactDatabase = contacts;
            _hostEnvironment = hostEnvironment;
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
                BirthDate = contactFromDb.BirthDate,
                ContactType = contactFromDb.ContactType,
                PhotoUrl = contactFromDb.PhotoUrl
            };

            return View(contact);
        }

        public IActionResult Create()
        {
            var vm = new ContactCreateViewModel();
            vm.BirthDate = new DateTime(1990, 1, 1);

            return View(vm);
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
                BirthDate = contact.BirthDate,
                ContactType = contact.ContactType,
            };

            if (contact.Avatar != null)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(contact.Avatar.FileName);
                var path = Path.Combine(_hostEnvironment.WebRootPath, "photos", uniqueFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    contact.Avatar.CopyTo(stream);
                }

                contactToDb.PhotoUrl = "/photos/" + uniqueFileName;
            }

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
                BirthDate = contactFromDb.BirthDate,
                ContactType = contactFromDb.ContactType,
                PhotoUrl = contactFromDb.PhotoUrl
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
                ContactType = contact.ContactType,
                BirthDate = contact.BirthDate
            };

            if (contact.Avatar != null)
            {
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(contact.Avatar.FileName);
                var path = Path.Combine(_hostEnvironment.WebRootPath, "photos", uniqueFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    contact.Avatar.CopyTo(stream);
                }

                contactToDb.PhotoUrl = "/photos/" + uniqueFileName;
            }
            else
            {
                contactToDb.PhotoUrl = contact.PhotoUrl;
            }

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

        public Byte[] GetBytesFromFile(IFormFile file)
        {
            var extension = new FileInfo(file.FileName).Extension;
            if (extension == ".jpg" || extension == ".png" || extension == ".PNG")
            {
                using var memoryStream = new MemoryStream();
                file.CopyTo(memoryStream);

                return memoryStream.ToArray();
            }
            else
            {
                return new byte[]{ };
            }
        }
    }
}
