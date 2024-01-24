using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Update_Grade
{
    /// <summary>
    /// Interaction logic for Add_Class.xaml
    /// </summary>
    public partial class Add_Class : Window
    {

        EnrollmentDataDataContext dbase = new EnrollmentDataDataContext();
        public Add_Class()
        {
            InitializeComponent();
            InitializeListView();

            dbase = new EnrollmentDataDataContext(Properties.Settings.Default.EnrollmentDatabaseConnectionString);

        }

        public class GradesData
        {
            public int schedID { get; set; }
            public string classPeriod { get; set; }
            public string classDay { get; set; }
            public int subjectID { get; set; }
            public int employeeID { get; set; }
            public int gradeLevelID { get; set; }
            public int section { get; set; }

        }



        private void InitializeListView()
        {
            try
            {
                var grades = dbase.tblSchedules.ToList();

                var dataGrades = new List<GradesData>();

                foreach (var grade in grades)
                {
                    dataGrades.Add(new GradesData
                    {
                        schedID = grade.scheduleID,
                        classPeriod = grade.classPeriod,
                        classDay = grade.classDay,
                        subjectID = grade.subjectID,
                        employeeID = grade.employeeID,
                        gradeLevelID = grade.gradeLevelID,
                        section = grade.sectionID

                    });
                }
                var subjects = new List<int>();

                foreach (var grade in grades)
                {
                    subjects.Add(grade.subjectID);
                }

                cbx_Subject.ItemsSource = subjects;

                var teachers = new List<int>();

                foreach (var grade in grades)
                {
                    teachers.Add(grade.employeeID);
                }

                cbx_Teacher.ItemsSource = teachers;

                var gradelvls = new List<int>();

                foreach (var grade in grades)
                {
                    gradelvls.Add(grade.gradeLevelID);
                }

                cbx_Gradelvl.ItemsSource = gradelvls;

                var sections = new List<int>();

                foreach (var grade in grades)
                {
                    sections.Add(grade.sectionID);
                }

                cbx_Sec2.ItemsSource = sections;


                List_grades.ItemsSource = dataGrades;
            }
             
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddClassData(string cPeriod, string cDay, int subID, int employID, int gradelvlID, int secID )
        {
            try
            {
                var sched = from s in dbase.tblSchedules select s;
                tblSchedule pdt = new tblSchedule
                {
                    classPeriod = cPeriod,
                    classDay = cDay,
                    subjectID = subID,
                    employeeID = employID,
                    gradeLevelID = gradelvlID,
                    sectionID = secID
                };

                    dbase.tblSchedules.InsertOnSubmit(pdt);

                    dbase.SubmitChanges();
                    MessageBox.Show("Schedule added successfully!");
            }
            catch (FormatException)
            {
                MessageBox.Show("Invalid format in one of the input fields.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btn_Add_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(cbx_Subject.Text, out int subjectID) &&
                int.TryParse(cbx_Teacher.Text, out int teacherID) &&
                int.TryParse(cbx_Gradelvl.Text, out int gradeLevelID) &&
                int.TryParse(cbx_Sec2.Text, out int sectionID))
            {
                AddClassData(tbx_ClassP.Text, tbx_ClassD.Text, subjectID, teacherID, gradeLevelID, sectionID);
            }
            else
            {
                MessageBox.Show("Invalid input format for numeric fields.");
            }
        }

        public void AccessClassDetails(object sender, EventArgs e)
        {
            if (chbx_Add.IsChecked == true)
            {
                tbx_ClassD.IsEnabled = true;
                tbx_ClassP.IsEnabled = true;
                cbx_Gradelvl.IsEnabled = true;
                cbx_Sec2.IsEnabled = true;
                cbx_Subject.IsEnabled = true;
                cbx_Teacher.IsEnabled = true;

                if (tbx_ClassP.Text.Length > 0 && tbx_ClassD.Text.Length > 0)
                {

                        btn_Add.IsEnabled = true;
                   
                }
                else
                {
                    btn_Add.IsEnabled = false;
                }
            }
            else if (chbx_Add.IsChecked == false)
            {
                btn_Add.IsEnabled = false;
                tbx_ClassD.IsEnabled = false;
                tbx_ClassP.IsEnabled = false;
                cbx_Gradelvl.IsEnabled = false;
                cbx_Sec2.IsEnabled = false;
                cbx_Subject.IsEnabled = false;
                cbx_Teacher.IsEnabled = false;
            }
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            InitializeListView();
            tbx_ClassP.TextChanged += AccessClassDetails;
            tbx_ClassD.TextChanged += AccessClassDetails;
            chbx_Add.Checked += AccessClassDetails;
            chbx_Add.Unchecked += AccessClassDetails;
        }

    }
}
