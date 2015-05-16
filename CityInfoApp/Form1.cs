using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CityInfoApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string ConnectionString = @"SERVER = .\SQLEXPRESS; Database = CityInfoDB; Integrated Security = True ";



        City aCity = new City();

        private void saveButton_Click(object sender, EventArgs e)
        {
            aCity.cityName =nameTextBox.Text;
            aCity.cityAbout = aboutTextBox.Text;
            aCity.country = countryTextBox.Text;


            if (IsCityNameExists(aCity.cityName))
            {
                MessageBox.Show("City Name Already Exist!");

                //SqlConnection Connection = new SqlConnection(ConnectionString);

                //string query = "UPDATE CityTable SET About='" + aCity.cityAbout + "' WHERE CityName='" + aCity.cityName + "' ";
                //SqlCommand command = new SqlCommand(query, Connection);
                //Connection.Open();
                //int rowAffected=command.ExecuteNonQuery();
                //Connection.Close();

                //if (rowAffected > 0)
                //{
                //    MessageBox.Show("About is updated successfully!");
                //    GetTextBoxesClear();
                //}
                //else
                //{
                //    MessageBox.Show("Update Failed !");
                //}

            }


            else if ( nameTextBox.Text.Length < 4 )
            {
                MessageBox.Show("You Must Enter City Name at least FOUR Characters!");
            }


            else
            {
                SqlConnection Connection = new SqlConnection(ConnectionString);

                string query = "INSERT INTO CityTable VALUES('" + aCity.cityName + "','" + aCity.cityAbout + "','" +
                               aCity.country + "')";

                SqlCommand command = new SqlCommand(query, Connection);
                Connection.Open();
                int rowAffected = command.ExecuteNonQuery();
                Connection.Close();

                if (rowAffected > 0)
                {
                    MessageBox.Show("Information inserted successfully!");
                    GetTextBoxesClear();
                }
                else
                {
                    MessageBox.Show("Insertion Failed !");
                }
            }
            ShowAllInfo();

        }

        private void GetTextBoxesClear()
        {
            nameTextBox.Clear();
            aboutTextBox.Clear();
            countryTextBox.Clear();
        }

        public bool IsCityNameExists(string name)
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);

            string query = "SELECT CityName FROM CityTable WHERE CityName='" + name + "'";

            bool isCityNameExists = false;

            SqlCommand command = new SqlCommand(query, Connection);
            Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                isCityNameExists = true;
                break;
            }
            reader.Close();
            Connection.Close();

            return isCityNameExists;
        }


        public void LoadCityListView(List<City>city)

        {
            CitylistView.Items.Clear();
            int cnt = 1;

            foreach (var City in city)
            {
                ListViewItem item = new ListViewItem(cnt.ToString() );
                item.SubItems.Add(City.cityName);
                item.SubItems.Add(City.cityAbout);
                item.SubItems.Add(City.country);
                CitylistView.Items.Add(item);
                cnt++;

            }
        }

        public void ShowAllInfo()
        {
            SqlConnection Connection = new SqlConnection(ConnectionString);

            string query = " SELECT * FROM CityTable ";

            SqlCommand command = new SqlCommand(query,Connection);
            Connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<City> cityList = new List<City>();
            while (reader.Read())
            {
                City city = new City();
                city.cityName = reader[0].ToString();
                city.cityAbout = reader[1].ToString();
                city.country = reader[2].ToString();

                cityList.Add(city);
            }
            reader.Close();
            Connection.Close();

            LoadCityListView(cityList);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ShowAllInfo();
        }

        private void CitylistView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CitylistView_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void CitylistView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = CitylistView.SelectedItems[0];
        }

    }
}
