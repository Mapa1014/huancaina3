using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using huancaina.Models;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace huancaina.Controllers
{
    public class InventariosController : Controller
    {
        private readonly ILogger<InventariosController> _logger;
        private readonly IConfiguration _configuration;

        public InventariosController(ILogger<InventariosController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult VerInventario()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LeerInventario()
        {
            DataTable dataTable = new DataTable();

            try
            {
                // Obtener la cadena de conexión desde la configuración
                string? connectionString = _configuration.GetConnectionString("MySqlConnection");
                Console.WriteLine($"Cadena de conexión: {connectionString}");

                // Conexión a la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Conexión a la base de datos exitosa."); // Mensaje en consola
                    string query = "SELECT * FROM inventarios"; // Ajusta la consulta según tus necesidades
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                }

                ViewBag.Mensaje = "Consulta ejecutada exitosamente.";
                ViewBag.DataTable = dataTable;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Error de MySQL: {ex.Message}");
                ViewBag.Mensaje = $"Error de MySQL: {ex.Message}";
                ViewBag.DataTable = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error general: {ex.Message}");
                ViewBag.Mensaje = $"Error general: {ex.Message}";
                ViewBag.DataTable = null;
            }

            return View("VerInventario");
        }
    }
}

