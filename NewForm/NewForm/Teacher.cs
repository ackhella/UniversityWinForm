using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewForm
{
    public class Teacher : Human
    {
        private List<Student> students;
        private double salary;
        private int availableSeats;

        public Teacher()
        {
            students = new List<Student>(); 
        }
        public Teacher(string name, string surname, int age, Adress adress, string email, double salary, Key key, string photo, int availableSeats)
        : base(name, surname, age, email, adress, key, photo)
        {
            students = new List<Student>();
            this.salary = salary;
            this.availableSeats = availableSeats;
        }
        public Teacher(string name, string surname, int age, Adress adress, string email, double salary, Key key, int availableSeats = 1)
        : base(name, surname, age, email, adress, key)
        {
            students = new List<Student>();
            this.salary = salary;
            this.availableSeats = availableSeats;
        }
        public void AddStudent(Student student)
        {
            students.Add(student);
            availableSeats--;
        }
        public int CheckStudent(Student student)
        {
            if (availableSeats > 0)
            {
                if (Language == student.Language)
                {
                    return 1;
                }
                return 0;
            }
            return -1;
        }
        public List<Student> Students
        {
            get { return students; }
            set { students = value; }
        }
        public double Salary
        {
            get { return salary; }
            set { salary = value; }
        }
        public int AvailableSeats
        {
            get { return availableSeats; }
            set { availableSeats = value; }
        }
    }
}
