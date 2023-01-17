using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewForm
{
    public class Adress
    {
        private string country;
        private string city;

        public Adress()
        {
            country = "";
            city = "";
        }
        public Adress(string city, string country = "Україна")
        {
            this.country = country;
            this.city = city;
        }
        public string ToStr()
        {
            return city;
        }
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        public string City
        {
            get { return city; }
            set { city = value; }
        }
    }
}
