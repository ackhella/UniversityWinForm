using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewForm
{
    public enum Key { CSharp, Python, JavaScript, Java, Scala, Php }
    public class Human
    {
        private string name;
        private string surname;
        private int age;
        private string email;
        private Adress adress;
        private string photo;
        private Key key;

        public Human()
        {
            name = "Unknown";
            surname = "Unknown";
            age = 18;
            email = "@gmail.com";
            adress = new Adress();
            key = Key.CSharp;
        }
        public Human(string name, string surname, int age, string email, Adress adress, Key key, string photo="")
        {
            this.name = name;
            this.surname = surname;
            this.age = age;
            this.email = email;
            this.adress = adress;
            this.key = key;
            this.photo = photo;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        public int Age
        {
            get { return age; }
            set { age = value; }
        }
        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }
        public Key Language
        {
            get { return key; }
            set { key = value; }
        }
        public Adress _adress
        {
            get { return adress; }
            set { adress = value; }
        }
    }
}
