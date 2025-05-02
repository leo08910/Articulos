using System;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Articulos
{
    public partial class Form1 : Form
    {
        private decimal total = 0;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
            // Suscribirse al evento CellValueChanged
            dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;

            // Asegurarse de que los cambios en las celdas se confirmen al salir de la celda
            dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ConnectToMySqlDatabase();
        }

        private void ConnectToMySqlDatabase()
        {
            string connectionString = "Server=localhost;Database=articulos;User ID=root;Password=pz%8_f8pZ.64;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    MessageBox.Show("Conexión exitosa a la base de datos MySQL.");

                    string query = "SELECT * FROM articulos";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    dataGridView1.DataSource = dataTable;

                    foreach (DataGridViewColumn column in dataGridView1.Columns)
                    {
                        if (column.Name != "Cantidad")
                        {
                            column.ReadOnly = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al conectar a la base de datos MySQL: {ex.Message}");
                }
            }
        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Verificar que la columna modificada sea "Cantidad"
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "Cantidad")
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Obtener el valor de "Cantidad" y "Precio"
                if (decimal.TryParse(row.Cells["Cantidad"].Value?.ToString(), out decimal cantidad) &&
                    decimal.TryParse(row.Cells["Precio_unitario"].Value?.ToString(), out decimal precio))
                {
                    // Calcular el importe
                    decimal importe = cantidad * precio;

                    // Actualizar la columna "Importe"
                    row.Cells["Importe"].Value = importe;
                }
                else
                {
                    MessageBox.Show("Por favor, ingresa valores válidos en las columnas 'Cantidad' y 'Precio'.");
                }
            }
        }

        private void DataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            // Confirmar los cambios en la celda actual para que se dispare el evento CellValueChanged
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void btn_calcular_Click(object sender, EventArgs e)
        {
            total = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Importe"].Value != null &&
                    decimal.TryParse(row.Cells["Importe"].Value.ToString(), out decimal importe))
                {
                    total += importe;
                }
            }

            // Mostrar el total en el Label
            lblTotal.Text = $"Total: {total:C}";
        }
    }
}
