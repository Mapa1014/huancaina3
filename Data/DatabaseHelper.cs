using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace huancaina.Data
{
    public class DatabaseHelper
    {
        private readonly IConfiguration _configuration; //* Se inyecta la configuración de la aplicación
        public DatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }        
        public DataTable VerDatos(string query, MySqlParameter[] parametros = null)
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
                        if (parametros != null)
                        {
                            command.Parameters.AddRange(parametros);
                        }
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
        public void ActualizarDatos(string query, MySqlParameter[] parametros)
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
        public void EliminarDatos(string query, MySqlParameter[] parametros)
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
        private readonly string connectionString = "tu_cadena_de_conexión";

        public object ObtenerDato(string query, MySqlParameter[] parametros = null)
        {
            using (var connection = new MySqlConnection(connectionString))
            using (var command = new MySqlCommand(query, connection))
            {
                if (parametros != null)
                {
                    command.Parameters.AddRange(parametros);
                }

                connection.Open();
                return command.ExecuteScalar();
            }
        }
        public int[] ObtenerNMesa()
        {
            // Array para almacenar los resultados
            List<int> mesas = new List<int>();

            string? connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Crear el comando SQL
                using (var comando = new MySqlCommand("SELECT n_mesa FROM ordenes;", connection))
                {
                    // Ejecutar el query
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {                            
                            mesas.Add(reader.GetInt32(0)); 
                        }
                    }
                }
            }           
            return mesas.ToArray();
        }
    }
}