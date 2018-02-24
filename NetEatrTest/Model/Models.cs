using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetEatrTest.Model
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public bool Primary { get; set; }
    }

    public class Blob
    {
        public string Container { get; set; }
        public string Reff { get; set; }
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Speed { get; set; }
        public double Bearing { get; set; }
        public long Time { get; set; }
    }

    public class Phone
    {
        public string Number { get; set; }
        public bool Primary { get; set; }
    }

    public class User
    {
        public Blob ProfilePicture { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public IList<Phone> Phones { get; set; }
        public IList<Address> Addresses { get; set; }
        public Location Location { get; set; }
        public string ProfileDescription { get; set; }
    }
}
