using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace huancaina.Data
{
    public class DatabaseHelper
    {
        private readonly IConfiguration _configuration;

        public DatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DataTable VerDatos(string query)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string? connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error de MySQL: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                throw;
            }

            return dataTable;
        }
        public void InsertarDatos(string query, MySqlParameter[] parametros)
        {
            try
            {
                string? connectionString = _configuration.GetConnectionString("MySqlConnection");

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddRange(parametros);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error de MySQL: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                throw;
            }
        }
    }
}