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

            var u = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == username && u.Contrase�a == password);

            if (u != null)
            {
                TempData["Mensaje"] = $"Ingres� exitosamente. {username}";
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

    }
}

