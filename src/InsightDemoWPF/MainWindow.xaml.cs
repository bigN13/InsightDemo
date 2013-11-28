using Insight.Database;
using InsightDemoWPF.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InsightDemoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        string myConn;

        //private static string connectionString = ConfigurationManager.ConnectionStrings["Server = DIMITRIS-PC" + "; Database = School; Trusted_Connection = True;"].ConnectionString;
        private static ConnectionStringSettings myDatabase = ConfigurationManager.ConnectionStrings["School"];


        public MainWindow()
        {
            InitializeComponent();

            myConn = (txt_Connection.Text != "") ? txt_Connection.Text : "Server = DIMITRIS-PC" + "; Database = School; Trusted_Connection = True;";

        }

        private void btn_GetData_Click(object sender, RoutedEventArgs e)
        {
            List<Person> persons = new List<Person>();

            using (var c = new SqlConnection(myConn).OpenConnection())
            {
                // do stuff...
                c.QuerySql("SELECT * FROM Person", Parameters.Empty);
            }

            try
            {
                using (SqlConnection myConnection = new SqlConnection(myConn))
                {
                    SqlDataReader myReader = null;
                    string sqlString = "SELECT * FROM Person";

                    myConnection.Open();
                    using (SqlCommand sqlComm = new SqlCommand(sqlString, myConnection))
                    {

                        myReader = sqlComm.ExecuteReader();

                        int i = 1;

                        while (myReader.Read())
                        {
                            Person dp = new Person();
                            dp.FirstName = myReader["FirstName"].ToString();
                            dp.LastName = myReader["LastName"].ToString();
                            persons.Add(dp);
                            //i++;
                        }

                        lst_Persons.ItemsSource = persons;

                        myConnection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Connection error", MessageBoxButton.OK, MessageBoxImage.Error);
                //Application.Exit(); // finish the program  
            }
        }

        private void btn_GetData_Insight_Click(object sender, RoutedEventArgs e)
        {
            SqlConnectionStringBuilder database = new SqlConnectionStringBuilder(myConn);

            // run a query right off the connection (this performs an auto-open/close)
            database.Connection().QuerySql("SELECT * FROM Person", Parameters.Empty);

            IList<Enrollment> personscourses = database.Connection().QuerySql<Enrollment>("SELECT * FROM CourseInstructor");

            IList<Person> persons = database.Connection().QuerySql<Person>("SELECT * FROM Person");



            IList<Course> courses = database.Connection().QuerySql<Course>("SELECT * FROM Course");

            //List<Person> beers = new List<Person>();
            lst_Persons.ItemsSource = persons;

            lst_Courses.ItemsSource = courses;   
        }

        private void btn_Insight_Count_Click(object sender, RoutedEventArgs e)
        {
            SqlConnectionStringBuilder database = new SqlConnectionStringBuilder(myConn);

            int count2 = database.Connection().ExecuteScalarSql<int>("SELECT COUNT(*) FROM Person");

            MessageBox.Show(count2.ToString());
        }

        private void btn_SQLCE_Click(object sender, RoutedEventArgs e)
        {
            /* get the Path */
            var directoryName = System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var fileName = System.IO.Path.Combine(directoryName, "Foo2Database.sdf");

            /* check if exists */
            if (File.Exists(fileName))
                File.Delete(fileName);

            string connStr = @"Data Source = " + fileName;

            /* create Database */
            SqlCeEngine engine = new SqlCeEngine(connStr);
            engine.CreateDatabase();

            /* create table and columns */
            using (SqlCeConnection conn = new SqlCeConnection(connStr))
            {
                using (SqlCeCommand cmd = new SqlCeCommand(@"CREATE TABLE FooTable (Foo_ID int, FooData NVARCHAR(200))", conn))
                {
                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }
        }
    }
}
