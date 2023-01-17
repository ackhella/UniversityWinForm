using Guna.UI2.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Windows.Forms;

namespace NewForm
{
    public partial class Form1 : Form 
    {
        private TeachersList teachersList = new TeachersList();

        private DateTime DeadLine = new CourseWork().DeadLine;

        private List<Student> studentsList = new List<Student>();
        private List<Teacher> teachersList2 = new List<Teacher>();

        private Image img = Image.FromFile("C:\\Users\\АНЯ\\Desktop\\Form\\NewForm\\NewForm\\img\\user.png");

        public Form1()
        {
            InitializeComponent();

            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            this.Load += new EventHandler(Form1_Load);

            StudentPanel.Location = TeacherPanel.Location;
            layoutAttention.Location = flowStudentList.Location;
        }
        void Form1_Load(object sender, EventArgs e)
        {
            hidePanels();
            showTableColumns();
            AddDefault();
        }
        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            if (checkTeacherBoxes())
            {
                MessageBox.Show("Перевірте чи всі поля заповнені");
            }
            else
            {
                Teacher teacher = new Teacher();
                Adress adr = new Adress(city.Text, country.Text);
                string imgLine = ToBase64(pictureTeacher.Image, System.Drawing.Imaging.ImageFormat.Png);
                

                try
                {
                    int newAge = int.Parse(age.Text);
                    double newSalary = double.Parse(salary.Text);
                    int newSeats = int.Parse(availableSeats.Text);

                    teacher = new Teacher(name.Text, surname.Text, newAge, adr, email.Text, newSalary, (Key)Enum.Parse(typeof(Key), setKey.Text), imgLine, newSeats);

                    teachersList.AddTeacher(teacher);
                    addTeacherToTable(teacher);
                    CreatePanel(teacher);
                    clearTeacherBoxes();
                }
                catch
                {
                    MessageBox.Show("Поля вік, зарплата та місця повинні містити тільки цифри");
                }
            }
        }
        private void btnAdd_Click(object sender, EventArgs e, Teacher teacher)
        {
            if (checkStudentBoxes())
            {
                MessageBox.Show("Перевірте чи всі поля заповнені");
            }
            else
            {
                Student student = new Student();
                string imgLine = ToBase64(pictureStudent.Image, System.Drawing.Imaging.ImageFormat.Png);
                Adress adr = new Adress(cityStudent.Text, countryStudent.Text);
                DateTime newDeadline = DateTime.Parse(deadline.Text);
                CourseWork courseWork = new CourseWork(title.Text, description.Text, newDeadline);
                
                try
                {
                    int newAge = int.Parse(ageStudent.Text);

                    student = new Student(nameStudent.Text, surnameStudent.Text, newAge, adr, emailStudent.Text,
                    (Key)Enum.Parse(typeof(Key), setKeySt.Text), imgLine, courseWork);

                    if(CheckStudentGroup(teacher, student) == true)
                    {
                        CheckCourseWork();
                        studentsList.Add(student);
                        teacher.AddStudent(student);
                        addStudentToTable(student);
                        clearStudentBoxes();
                    }
                }
                catch
                {
                    MessageBox.Show("Поле вік повенен містити тільки цифри");
                }
            }
        }
        
        private void CheckCourseWork()
        {
            DateTime newDeadline = DateTime.Parse(deadline.Text);
            if (newDeadline > DeadLine)
            {
                MessageBox.Show($"Робота просрочена — {newDeadline.ToString("MM-dd-yyyy")}\nДедлайн — {DeadLine.ToString("MM-dd-yyyy")}");
                try
                {
                    MailAddress fromAdress = new MailAddress("zin8375anna@gmail.com", "University.com");
                    MailAddress toAdress = new MailAddress(emailStudent.Text, nameStudent.Text);
                    SmtpClient smtpClient = new SmtpClient();

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new NetworkCredential(fromAdress.Address, "hywguaunpkzkohqt");

                    MailMessage message = new MailMessage(fromAdress, toAdress);
                    message.Body = "Курсова робота " + title.Text + " просрочена";
                    message.Subject = "Повідомлення від університету";

                    smtpClient.Send(message);
                }
                catch
                {
                    MessageBox.Show("Повідомлення не відправлене");
                }
            }
        }
        private bool CheckStudentGroup(Teacher teacher, Student student)
        {
            int result = teacher.CheckStudent(student);
            if (result == -1)
            {
                MessageBox.Show("Недостатньо вільних місць");
                return false;
            }
            else if (result == 0)
            {
                MessageBox.Show("Спеціалізація не співпадає");
                return false;
            }
            else
            {
                return true;
            }
        }
        private bool checkTeacherBoxes()
        {
            if (string.IsNullOrEmpty(name.Text) || string.IsNullOrEmpty(surname.Text) || string.IsNullOrEmpty(age.Text) || string.IsNullOrEmpty(email.Text) ||
                string.IsNullOrEmpty(country.Text) || string.IsNullOrEmpty(city.Text) || string.IsNullOrEmpty(salary.Text) || string.IsNullOrEmpty(availableSeats.Text) ||
                string.IsNullOrEmpty(setKey.Text) || (pictureTeacher.Image == null))
            {
                return true;
            }
            return false;
        }
        private bool checkStudentBoxes()
        {
            if (string.IsNullOrEmpty(nameStudent.Text) || string.IsNullOrEmpty(surnameStudent.Text) || string.IsNullOrEmpty(ageStudent.Text) || string.IsNullOrEmpty(emailStudent.Text) ||
                string.IsNullOrEmpty(countryStudent.Text) || string.IsNullOrEmpty(cityStudent.Text) || string.IsNullOrEmpty(title.Text) || string.IsNullOrEmpty(description.Text) ||
                string.IsNullOrEmpty(setKeySt.Text) || (pictureStudent.Image == null))
            {
                return true;
            }
            return false;
        }

        public string ToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat imageType)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imageType);
                byte[] imageArr = ms.ToArray();
                string imgbase64 = Convert.ToBase64String(imageArr);
                return imgbase64;
            }
        }
        public System.Drawing.Image ToImage(string imgbase64)
        {
            byte[] imagArr = Convert.FromBase64String(imgbase64);
            using (var ms = new MemoryStream(imagArr, 0, imagArr.Length))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                return image;
            }
        }

        private void hidePanels()
        {
            layoutAttention.Visible = true;
            studentItem.Visible = false;
            flowStudentList.Visible = false;
            TeacherPanel.Visible = false;
            StudentPanel.Visible = false;
            teacherItem.Visible = false;
        }
        private void addTeacher_Click(object sender, EventArgs e)
        {
            hidePanels();
            TeacherPanel.Visible = true;
        }
        private void addStudent_Click(object sender, EventArgs e)
        {
            hidePanels();
            StudentPanel.Visible = true;
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {
            hidePanels();
        }

        private void btnShow_Click(object sender, EventArgs e, Teacher teacher)
        {
            flowStudentList.Controls.Clear();
            layoutAttention.Visible = false;
            flowStudentList.Visible = true;
            guna2TabControl1.SelectTab(tabPage2);

            foreach (var student in teacher.Students)
            {
                CreatePanelStudent(student, teacher);
            }
        }

        private void toMainpage_Click(object sender, EventArgs e)
        {
            guna2TabControl1.SelectTab(tabPage1);
        }
        private void hideList_Click(object sender, EventArgs e)
        {
            layoutAttention.Visible = true;
            flowStudentList.Visible = false;
        }

        private void CreatePanelStudent(Student student, Teacher teacher)
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
            PictureBox picBoxStudent = new PictureBox();
            Guna2Button btnClearStudent = new Guna2Button();
            CourseWork work = new CourseWork();
            work = student.CourseWrk;

            p.Name = $"{student.Name} {student.Surname}\n{student.Language}\n\n{student.Email}\n\nРобота:\n{work.Title}\n{work.DeadLine.ToString("MM-dd-yyyy")}";
            p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            p.BackColor = Color.Transparent;
            p.Size = new Size(270, 338);
            p.Paint += (ss, ee) => ee.Graphics.DrawString(p.Name, new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204))), Brushes.Black, 8, 157);

            picBoxStudent.BackColor = Color.White;
            picBoxStudent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            if (student.Photo == "")
            {
                picBoxStudent.Image = ToImage(ToBase64(img, System.Drawing.Imaging.ImageFormat.Png));
            }
            else
            {
                picBoxStudent.Image = ToImage(student.Photo);
            }
            picBoxStudent.Location = new Point(80, 26);
            picBoxStudent.Margin = new Padding(2, 3, 2, 3);
            picBoxStudent.Name = "picBoxStudent";
            picBoxStudent.Size = new Size(98, 119);
            picBoxStudent.SizeMode = PictureBoxSizeMode.CenterImage;
            picBoxStudent.TabStop = false;

            btnClearStudent.FillColor = Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            btnClearStudent.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            btnClearStudent.ForeColor = Color.White;
            btnClearStudent.Location = new Point(0, 295);
            btnClearStudent.Name = "btnClearStudent";
            btnClearStudent.Size = new Size(270, 42);
            btnClearStudent.Text = "Видалити";
            btnClearStudent.Click += (sender, EventArgs) => { btnClearStudent_Click(sender, EventArgs, p.Name, student, teacher); };

            p.Controls.Add(picBoxStudent);
            p.Controls.Add(btnClearStudent);
            flowStudentList.Controls.Add(p);
            flowStudentList.Controls.SetChildIndex(p, 0);
            flowStudentList.Invalidate();
        }
        private void CreatePanel(Teacher teacher)
        {
            System.Windows.Forms.Panel p = new System.Windows.Forms.Panel();
            PictureBox picBoxTeacher = new PictureBox();
            Guna2Button btnShow = new Guna2Button();
            Guna2Button btnAdd = new Guna2Button();
            Guna2Button btnClear = new Guna2Button();

            picBoxTeacher.BackColor = Color.White;
            picBoxTeacher.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            if (teacher.Photo == "")
            {
                picBoxTeacher.Image = ToImage(ToBase64(img, System.Drawing.Imaging.ImageFormat.Png));
            }
            else
            {
                picBoxTeacher.Image = ToImage(teacher.Photo);
            }
            picBoxTeacher.Location = new Point(21, 18);
            picBoxTeacher.Margin = new Padding(2, 3, 2, 3);
            picBoxTeacher.Name = "picBoxTeacher";
            picBoxTeacher.Size = new Size(98, 130);
            picBoxTeacher.SizeMode = PictureBoxSizeMode.CenterImage;
            picBoxTeacher.TabStop = false;

            btnShow.FillColor = Color.Transparent;
            btnShow.Font = new Font("Segoe UI", 9F);
            btnShow.ForeColor = Color.Black;
            btnShow.Location = new Point(267, 108);
            btnShow.Name = "btnShow";
            btnShow.Size = new Size(112, 46);
            btnShow.Text = "Показати список";
            btnShow.Click += (sender, EventArgs) => { btnShow_Click(sender, EventArgs, teacher); };

            btnAdd.FillColor = Color.Transparent;
            btnAdd.Font = new Font("Segoe UI", 9F);
            btnAdd.ForeColor = Color.Black;
            btnAdd.Location = new Point(137, 108);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(112, 46);
            btnAdd.Text = "Зберегти";
            btnAdd.Click += (sender, EventArgs) => { btnAdd_Click(sender, EventArgs, teacher); };

            btnClear.FillColor = Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            btnClear.Font = new Font("Segoe UI Black", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(204)));
            btnClear.ForeColor = Color.White;
            btnClear.Location = new Point(336, 6);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(43, 34);
            btnClear.Text = "X";
            btnClear.Click += (sender, EventArgs) => { btnClear_Click(sender, EventArgs, p.Name, teacher); };

            p.Name = $"{teacher.Name} {teacher.Surname}\n{teacher.Email}\n\n{teacher.Language.ToString()}";
            p.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            p.BackColor = Color.Transparent;
            p.Size = new Size(400, 166);
            p.Paint += (ss, ee) => ee.Graphics.DrawString(p.Name, new Font("Microsoft Sans Serif", 10.2F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204))), Brushes.Black, 133, 28);

            p.Controls.Add(btnShow);
            p.Controls.Add(btnAdd);
            p.Controls.Add(btnClear);
            p.Controls.Add(picBoxTeacher);
            flowPanel.Controls.Add(p);
            flowPanel.Controls.SetChildIndex(p, 0);
            flowPanel.Invalidate();
        }

        private void btnClear_Click(object sender, EventArgs e, string name, Teacher teacher)
        {
            DeleteTeacher(name, teacher);
        }
        private void btnClearStudent_Click(object sender, EventArgs e, string name, Student student, Teacher teacher)
        {
            DeleteStudent(name, student, teacher);
        }
        private void DeleteStudent(string name, Student student, Teacher teacher)
        {
            var panel = flowStudentList.Controls[name];

            if (flowStudentList.Controls.Contains(panel))
            {
                panel.Dispose();
            }
            else
            {
                flowStudentList.Visible = false;
                layoutAttention.Visible = true;
            }

            int index = studentsList.IndexOf(student);
            DataGrid2.Rows.RemoveAt(index);

            studentsList.Remove(student);

            teacher.Students.Remove(student);
            teacher.AvailableSeats = teacher.AvailableSeats + 1;
        }
        private void DeleteTeacher(string name, Teacher teacher)
        {
            foreach (var student in teacher.Students.ToList())
            {
                DeleteStudent(name, student, teacher);
            }
            teacher.Students.Clear();

            var panel = flowPanel.Controls[name];
            panel.Dispose();

            int index = teachersList._teacherList.IndexOf(teacher);
            DataGrid.Rows.RemoveAt(index);

            teachersList._teacherList.Remove(teacher);
        }
        public void showTableColumns() 
        {
            for (int i = 0; i < DataGrid.Columns.Count; i++)
            {
                DataGrid.Columns[i].ValueType = typeof(string);
            }
            for (int i = 0; i < DataGrid2.Columns.Count; i++)
            {
                DataGrid2.Columns[i].ValueType = typeof(string);
            }
        }
        private void addTeacherToTable(Teacher teacher)
        {
            DataGrid.Rows.Add(teacher.Name, teacher.Surname, teacher.Email, teacher.Age, teacher.Language,
                    teacher._adress.ToStr(), teacher.Salary);
        }
        private void addStudentToTable(Student student)
        {
            DataGrid2.Rows.Add(student.Name, student.Surname, student.Email, student.Age, student.Language,
                    student._adress.ToStr(), student.CourseWrk.ToStr());
        }
        private void AddDefault()
        {

            string img = ToBase64(Image.FromFile("C:\\Users\\АНЯ\\Desktop\\Form\\NewForm\\NewForm\\img\\user.png"), System.Drawing.Imaging.ImageFormat.Png);

            Teacher teacher1 = new Teacher("Катерина", "Іванюк", 36, new Adress("Львів"), "ivanyuk22@gamil.com", 15000, (Key)Enum.Parse(typeof(Key), "Python"), img, 5);
            Teacher teacher2 = new Teacher("Денис", "Мельник", 40, new Adress("Київ"), "den4is@gamil.com", 14500, (Key)Enum.Parse(typeof(Key), "Scala"), img, 3);

            teachersList.AddTeacher(teacher1);
            addTeacherToTable(teacher1);
            CreatePanel(teacher1);
            teachersList.AddTeacher(teacher2);
            addTeacherToTable(teacher2);
            CreatePanel(teacher2);

            Student student1 = new Student("Марина", "Тарасенко", 36, new Adress("Херсон"), "ivanyuk22@gamil.com", (Key)Enum.Parse(typeof(Key), "Python"),
                img, new CourseWork("Розробка web-застосунків", "Дослідження та впровадження", new DateTime(2023, 01, 18)));
            Student student2 = new Student("Антон", "Добриця", 40, new Adress("Київ"), "den4is@gamil.com",  (Key)Enum.Parse(typeof(Key), "Scala"),
                img, new CourseWork("Статистика та обробка данних", "Дослідження та впровадження", new DateTime(2023, 01, 15)));
            Student student3 = new Student("Яна", "Шевчук", 36, new Adress("Херсон"), "ivanyuk22@gamil.com",  (Key)Enum.Parse(typeof(Key), "Python"),
                img, new CourseWork("Машинний інтелект", "Дослідження та впровадження", new DateTime(2023, 01, 10)));

            teacher1.AddStudent(student1);
            addStudentToTable(student1);
            studentsList.Add(student1);
            teacher2.AddStudent(student2);
            addStudentToTable(student2);
            studentsList.Add(student2);
            teacher2.AddStudent(student3);
            addStudentToTable(student3);
            studentsList.Add(student3);

            pictureStudent.Image = null;
            pictureTeacher.Image = null;
        }
        private void clearStudentBoxes()
        {
            pictureStudent.Image = null;
            nameStudent.Text = String.Empty;
            surnameStudent.Text = String.Empty;
            ageStudent.Text = String.Empty;
            emailStudent.Text = String.Empty;
            setKeySt.Text = String.Empty;
            countryStudent.Text = String.Empty;
            cityStudent.Text = String.Empty;

            deadline.Text = String.Empty;
            title.Text = String.Empty;
            description.Text = String.Empty;
        }
        private void clearTeacherBoxes()
        {
            pictureTeacher.Image = null;
            name.Text = String.Empty;
            surname.Text = String.Empty;
            age.Text = String.Empty;
            email.Text = String.Empty;
            setKey.Text = String.Empty;
            country.Text = String.Empty;
            city.Text = String.Empty;

            salary.Text = String.Empty;
            availableSeats.Text = String.Empty;
        }
        private void showGraph_Click(object sender, EventArgs e)
        {
            Graph graph = new Graph(teachersList);
            graph.Show();
        }

        private void pictureTeacher_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureTeacher.Image = new Bitmap(open.FileName);
                pictureTeacher.Text = open.FileName;
            }
        }
        private void pictureStudent_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                pictureStudent.Image = new Bitmap(open.FileName);
                pictureStudent.Text = open.FileName;
            }
        }

        
        private void ToJson_Click(object sender, EventArgs e)
        {
            WriteToJson();
        }
        private void ToTxt_Click(object sender, EventArgs e)
        {
            WriteToText();
        }
        private void FromJson_Click(object sender, EventArgs e)
        {
            ReadFromJson();

            if (teachersList2 == null)
            {
                MessageBox.Show("Файл порожній");
            }
            else
            {
                foreach (var teacher in teachersList2)
                {
                    addTeacherToTable(teacher);
                    teachersList.AddTeacher(teacher);
                    CreatePanel(teacher);

                    foreach (var student in teacher.Students)
                    {
                        studentsList.Add(student);
                        addStudentToTable(student);
                    }
                }
            }
        }
        private void FromTxt_Click(object sender, EventArgs e)
        {
            ReadFromTxt();
        }
        public void WriteToJson()
        {
            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Json files (*.json)|*.json";
            fileDialog.ShowDialog();
            string data = JsonConvert.SerializeObject(teachersList._teacherList);

            if (fileDialog.FileName != "")
            {
                File.WriteAllText(fileDialog.FileName, data);
            }
        }
        public void ReadFromJson()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Json files (*.json)|*.json";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != "")
            {
                string data = File.ReadAllText(fileDialog.FileName);
                teachersList2 = JsonConvert.DeserializeObject<List<Teacher>>(data);
            }
        }
        private void ReadFromTxt()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text files (*.txt)|*.txt";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != "")
            {
                StreamReader file = new StreamReader(fileDialog.FileName);
                string newline;
                while ((newline = file.ReadLine()) != null)
                {
                    string[] values = newline.Split(',');
                    try
                    {
                        //DataGrid.Rows.Add(values[0], values[1], values[2], values[3], values[4],
                        //values[5], values[6]);

                        Teacher teacher = new Teacher(values[0], values[1], int.Parse(values[3]), new Adress(values[5]), values[2], double.Parse(values[6]),
                        (Key)Enum.Parse(typeof(Key), values[4]));

                        addTeacherToTable(teacher);
                        teachersList.AddTeacher(teacher);
                        CreatePanel(teacher);
                    }
                    catch
                    {
                        MessageBox.Show("Помилка в утворенні всіх або одного з рядків");
                    }
                }
                file.Close();
            }
        }
        private void WriteToText()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Text files (*.txt)|*.txt";
            fileDialog.ShowDialog();

            if (fileDialog.FileName != "")
            {
                using (TextWriter line = new StreamWriter(fileDialog.FileName))
                {
                    for (int i = 0; i < DataGrid.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < DataGrid.Columns.Count; j++)
                        {
                            line.Write($"{DataGrid.Rows[i].Cells[j].Value}");

                            if (!(j == DataGrid.Columns.Count - 1))
                            {
                                line.Write(',');
                            }
                        }
                        line.WriteLine();
                    }
                }
            } 
        }
    }
}