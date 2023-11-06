using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;


namespace A2DeepinderKaurWarya
{
    public partial class AddCityWindow : Window
    {
        private int countryId;

        public AddCityWindow(int countryId)
        {
            InitializeComponent();
            this.countryId = countryId;
            LoadCountries();
        }

        private void LoadCountries()
        {
            string query = "SELECT CountryId, CountryName FROM Country";

            using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                CmbCountries.ItemsSource = dataTable.DefaultView;
            }
        }

        private void BtnAddCity_Click(object sender, RoutedEventArgs e)
        {
            string cityName = TxtCityName.Text;
            bool isCapital = ChkIsCapital.IsChecked ?? false;
            string population = TxtPopulation.Text;

            // Performing validation (Checking if country is not empty)
            if (CmbCountries.SelectedItem == null)
            {
                MessageBox.Show("Please select a country.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int countryId = (int)CmbCountries.SelectedValue;
            // Performing validation (Checking if cityName is not empty)
            if (string.IsNullOrWhiteSpace(cityName))
            {
                MessageBox.Show("Please enter a valid city name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Table "City" with columns "CityName", "IsCapital", "Population", and "CountryId"
            string query = "INSERT INTO City (CityName, IsCapital, Population, CountryId) VALUES (@CityName, @IsCapital, @Population, @CountryId)";

            using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CityName", cityName);
                command.Parameters.AddWithValue("@IsCapital", isCapital);
                command.Parameters.AddWithValue("@Population", population);
                command.Parameters.AddWithValue("@CountryId", countryId);
                command.ExecuteNonQuery();
            }
            MessageBox.Show("New City added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            // Close the window after adding the city
            this.Close();
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            // Close the AddCityWindow
            this.Close();
        }
    }
}