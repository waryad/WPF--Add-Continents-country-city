using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;

namespace A2DeepinderKaurWarya
{
    public partial class AddCountryWindow : Window
    {
        private int continentId;
        public AddCountryWindow(int continentId)
        {
            InitializeComponent();
            LoadContinents();
            this.continentId = continentId;
        }
        private void CmbContinents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbContinents.SelectedItem != null)
            {
                DataRowView selectedContinent = (DataRowView)CmbContinents.SelectedItem;
                continentId = (int)selectedContinent["ContinentId"];
            }
        }
        private void LoadContinents()
        {
            string query = "SELECT ContinentId, ContinentName FROM Continent";

            using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                CmbContinents.ItemsSource = dataTable.DefaultView;
            }
        }

        private void BtnAddCountry_Click(object sender, RoutedEventArgs e)
        {
            string continentName = CmbContinents.Text.Trim();
            string countryName = TxtCountryName.Text;
            string language = TxtLanguage.Text;
            string currency = TxtCurrency.Text;

            // Performing validation (checking if the countryName are not empty)
            if (string.IsNullOrEmpty(countryName))
            {
                LblCountryRequired.Visibility = Visibility.Visible;
                MessageBox.Show("Please enter the Country Name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                LblCountryRequired.Visibility = Visibility.Collapsed;
            }

            if (string.IsNullOrEmpty(continentName))
            {
                LblContinentRequired.Visibility = Visibility.Visible;
                MessageBox.Show("Please select a Continent.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                LblContinentRequired.Visibility = Visibility.Collapsed;
            }

            // table "Country" with columns "CountryName" and "ContinentId"
            string query = "INSERT INTO Country (CountryName, Language, Currency, ContinentId) VALUES (@CountryName, @Language, @Currency, @ContinentId)";

            using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CountryName", countryName);
                command.Parameters.AddWithValue("@Language", language);
                command.Parameters.AddWithValue("@Currency", currency);
                command.Parameters.AddWithValue("@ContinentId", continentId);
                command.ExecuteNonQuery();
            }

            // Following code will close the window after adding the country and pass the country name back to the main window
           
            this.DialogResult = true;
            MessageBox.Show("New country added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            // Close the AddCountryWindow
            this.Close();
        }
    }
}
