using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewForm
{
    public class TeachersList
    {
        public List<Teacher> teacherList;

        public TeachersList()
        {
            teacherList = new List<Teacher>();
        }
        public void AddTeacher(Teacher teacher)
        {
            teacherList.Add(teacher);
        }
        public List<Teacher> _teacherList
        {
            get { return teacherList; }
            set { teacherList = value; }
        }
    }
}
