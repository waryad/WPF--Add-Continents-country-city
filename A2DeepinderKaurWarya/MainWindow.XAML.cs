using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;



namespace A2DeepinderKaurWarya
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadContinents(); // This will load the continents when the main window is initialized
        }

        // This method to load continents into the ComboBox
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

        // This method to load countries based on the selected continent
        private void LoadCountries(int continentId)
        {
            string query = "SELECT CountryId, CountryName FROM Country WHERE ContinentId = @ContinentId";

            using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ContinentId", continentId);
                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                LstCountries.ItemsSource = dataTable.DefaultView;
            }
        }

        // This method to load cities based on the selected country
        private void LoadCities(int countryId)
        {
            string query = "SELECT C.CityName, C.IsCapital, C.Population, CO.CountryName " +
                           "FROM City C " + "INNER JOIN Country CO ON C.CountryId = CO.CountryId " +
                           "WHERE C.CountryId = @CountryId";

            using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CountryId", countryId);
                SqlDataReader reader = command.ExecuteReader();
                DataTable dataTable = new DataTable();
                dataTable.Load(reader);
                dgCities.ItemsSource = dataTable.DefaultView;
            }
        }

        // When a continent is selected in the ComboBox----Event handler 
        private void CmbContinents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbContinents.SelectedItem != null)
            {
                DataRowView selectedContinent = (DataRowView)CmbContinents.SelectedItem;
                int continentId = (int)selectedContinent["ContinentId"];
                LoadCountries(continentId); // Load countries based on selected continent
                LblLanguage.Content = "";
                LblCurrency.Content = ""; 
                dgCities.ItemsSource = null; //this will Clear the DataGrid for cities
            }
        }

        //  when a country is selected in ListBox --Event handler
        private void LstCountries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LstCountries.SelectedItem != null)
            {
                DataRowView selectedCountry = (DataRowView)LstCountries.SelectedItem;
                int countryId = (int)selectedCountry["CountryId"];
                LoadCities(countryId); // Load cities based on the selected country

                using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT DISTINCT Language FROM Country WHERE CountryId = @CountryId";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    object languageResult = command.ExecuteScalar();
                    LblLanguage.Content = languageResult != null ? languageResult.ToString() : "N/A";

                    query = "SELECT Currency FROM Country WHERE CountryId = @CountryId";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@CountryId", countryId);
                    object currencyResult = command.ExecuteScalar();
                    LblCurrency.Content = currencyResult != null ? currencyResult.ToString() : "N/A";
                }
            }
        }

        //  When the "Add Continent" button is clicked-- Event handler
        private void BtnAddContinent_Click(object sender, RoutedEventArgs e)
        {
            AddContinentWindow addContinentWindow = new AddContinentWindow();
            addContinentWindow.ShowDialog();
            LoadContinents(); // Refresh the list of continents after adding a new one
        }

        // When the "Add Country" button is clicked---Event handler 
        private void BtnAddCountry_Click(object sender, RoutedEventArgs e)
        {
            if (CmbContinents.SelectedItem != null)
            {
                DataRowView selectedContinent = (DataRowView)CmbContinents.SelectedItem;
                int continentId = (int)selectedContinent["ContinentId"];
                AddCountryWindow addCountryWindow = new AddCountryWindow(continentId);
                addCountryWindow.ShowDialog();
                LoadCountries(continentId); // Refresh the list of countries after adding a new one
            }
        }

        //  When the "Add City" button is clicked---Event handler
        private void BtnAddCity_Click(object sender, RoutedEventArgs e)
        {
            if (LstCountries.SelectedItem != null)
            {
                DataRowView selectedCountry = (DataRowView)LstCountries.SelectedItem;
                int countryId = (int)selectedCountry["CountryId"];
                AddCityWindow addCityWindow = new AddCityWindow(countryId);
                addCityWindow.ShowDialog();
                LoadCities(countryId); // Refresh the list of cities after adding new data
            }
        }
    }
}


