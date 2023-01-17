using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewForm
{
    public class CourseWork
    {
        private string title;
        private string description;
        private DateTime deadLine;
        public CourseWork()
        {
            title = "";
            description = "";
            deadLine = new DateTime(2023, 01, 16);
        }
        public CourseWork(string title, string description, DateTime deadLine)
        {
            this.title = title;
            this.description = description;
            this.deadLine = deadLine;
        }
        public string ToStr()
        {
            return title + " — " + deadLine;
        }
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public DateTime DeadLine
        {
            get { return deadLine; }
            set { deadLine = value; }
        }
    }
}
