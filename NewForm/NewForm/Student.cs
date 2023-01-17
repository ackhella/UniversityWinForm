using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewForm
{
    public class Student : Human
    {
        private CourseWork courseWork;

        public Student() : base()
        {
            courseWork = new CourseWork();
        }
        public Student(string name, string surname, int age, Adress adress, string email, Key key, string photo, CourseWork courseWork) : base(name, surname, age, email, adress, key, photo)
        {
            this.courseWork = courseWork;
        }
        public CourseWork CourseWrk
        {
            get { return courseWork; }
            set { courseWork = value; }
        }
    }
}
