using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactWeb.Domain
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public ContactType ContactType { get; set; }
    }

    public enum ContactType
    {
        Family,
        Friend,
        Colleague,
        Stranger,
        Boss
    }
}
