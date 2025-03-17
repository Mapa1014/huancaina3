using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using huancaina.Models;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace huancaina.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context; // Añadir esta línea

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ApplicationDbContext context) // Modificar el constructor
        {
            _logger = logger;
            _configuration = configuration;
            _context = context; // Inicializar el contexto
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult getLogin()
        {
            return View("Login");
        }

        public IActionResult Login(string username, string password)
        {
            Console.WriteLine($"Usuarios: {username}, Contraseña: {password}");
            // Simulación de autenticación

            var u = _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario.ToString() == username && u.Contraseña == password);
            
            if (u != null)
            {
                return RedirectToAction("Index", "Home"); // Redirige si el login es exitoso
            }
            if (username == "IdUsuario" && password == "Contraseña")
            {
                return RedirectToAction("Index", "Home"); // Redirige si el login es exitoso
            }
            ViewBag.Error = "Usuario o contraseña incorrectos"; 
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LeerDatos()
        {
            DataTable dataTable = new DataTable();

            try
            {
                // Obtener la cadena de conexión desde la configuración
                string? connectionString = _configuration.GetConnectionString("MySqlConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("La cadena de conexión no puede ser nula o vacía.");
                }
                Console.WriteLine($"Cadena de conexión: {connectionString}");

                // Conexión a la base de datos
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Conexión a la base de datos exitosa."); // Mensaje en consola
                    string query = "SELECT * FROM usuarios"; // Ajusta la consulta según tus necesidades
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

            return View("EjecutarScript");
        }
    }
}

