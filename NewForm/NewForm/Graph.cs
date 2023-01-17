using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NewForm
{
    public partial class Graph : Form
    {
        private TeachersList listOfTeachers = new TeachersList(); //_teacherList
        private List<Teacher> listOfTeachers_;
        public Graph(TeachersList listOfTeachers)
        {
            this.listOfTeachers = listOfTeachers;
            InitializeComponent();
            showChart();
            showTree();
        }
        public Graph()
        {
            InitializeComponent();
        }
        private void close_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void showChart()
        {
            listOfTeachers_ = listOfTeachers._teacherList;
            foreach (var teacher in listOfTeachers_)
            {
                chartTeacher.Series["Teachers"].Points.AddXY(teacher.Name, teacher.Students.Count());
            }
        }
        public void showTree()
        {
            listOfTeachers_ = listOfTeachers._teacherList;
            for (int i = 0; i < listOfTeachers_.Count; i++)
            {
                var teacher = listOfTeachers_[i];

                treeView1.Nodes.Add(teacher.Name + " " + teacher.Surname);
                for (int j = 0; j < teacher.Students.Count; j++)
                {
                    var student = teacher.Students[j];

                    treeView1.Nodes[i].Nodes.Add(student.Name + " " + student.Surname);
                    treeView1.Nodes[i].Nodes[j].Nodes.Add(student.CourseWrk.DeadLine.ToString("MM-dd-yyyy"));

                    if (student.CourseWrk.DeadLine > new Student().CourseWrk.DeadLine)
                    {
                        treeView1.Nodes[i].Nodes[j].Nodes[0].ForeColor = Color.IndianRed;
                    }
                }
            }
        }
    }
}
