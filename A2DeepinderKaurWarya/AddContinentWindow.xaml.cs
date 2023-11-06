using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Controls;


namespace A2DeepinderKaurWarya
{
    public partial class AddContinentWindow : Window
    {
        public AddContinentWindow()
        {
            InitializeComponent();
        }

        private void BtnAddContinent_Click(object sender, RoutedEventArgs e)
        {
            string continentName = TxtContinentName.Text;

            // Performing validation ( checking if the continentName is not empty)
            if (string.IsNullOrWhiteSpace(continentName))
            {
                MessageBox.Show("Please enter a valid continent name.", "Error", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string query = "INSERT INTO Continent (ContinentName) VALUES (@ContinentName)";

            using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ContinentName", continentName);
                command.ExecuteNonQuery();
            }
            MessageBox.Show("New continent added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
           // Close the window after adding the continent
           this.Close();
        }

        private void BtnCloseWindow_Click(object sender, RoutedEventArgs e)
        {
            // Close the AddContinentWindow
            this.Close();
        }
    }
}



