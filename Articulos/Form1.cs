using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Articulos
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Llamar al método de conexión al cargar el formulario
            ConnectToMySqlDatabase();
        }

        private void ConnectToMySqlDatabase()
        {
            // Cadena de conexión para MySQL
            string connectionString = "Server=localhost;Database=articulos;User ID=root;Password=pz%8_f8pZ.64;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Conexión exitosa a la base de datos MySQL.");

                    // Aquí puedes ejecutar consultas o comandos
                    string query = "SELECT * FROM articulos";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar a la base de datos MySQL: {ex.Message}");
                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
