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
        private readonly ApplicationDbContext _context;
        private readonly DatabaseHelper _dbHelper;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, ApplicationDbContext context) // Modificar el constructor
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
            _dbHelper = new DatabaseHelper(configuration);
        }

        public IActionResult Index()
        {
            ViewBag.UserName = User.Identity.Name;
            return View();
        }

        public IActionResult getLogin()
        {
            return View("Login");
        }

        public async Task<IActionResult> Login(string username, string password)
        {
            Console.WriteLine($"Usuarios: {username}, Contrase�a: {password}");
            // Simulaci�n de autenticaci�n

            var u = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == username && u.Contrasena == password);


            if (u != null)
            {
                TempData["Mensaje"] = $"Ingres� exitosamente. {username}";
                Console.WriteLine($"Ingres� exitosamente. {username}");
                return RedirectToAction("Index", "Home"); // Redirige si el login es exitoso
            }

            ViewBag.Error = "Usuario o contrase�a incorrectos";
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Contactanos()
        {
            return View();
        }

        // Acci�n para procesar el env�o del formulario
        [HttpPost]
        public IActionResult EnviarMensaje(string nombre, string telefono, string email, string mensaje)
        {
            // Enviar un mensaje de confirmaci�n a la vista
            ViewBag.MensajeExito = "Tu mensaje ha sido enviado correctamente. Nos pondremos en contacto contigo pronto.";
            Console.WriteLine($"Nombre: {nombre}, Tel�fono: {telefono}, Email: {email}, Mensaje: {mensaje}");
            return View("Contactanos"); // Vuelve a la misma vista de Contactanos con el mensaje
        }
    }
}