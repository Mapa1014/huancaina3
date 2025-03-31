using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using huancaina.Models;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public IActionResult Error404()
        {
            return View();
        }

        public IActionResult Error500()
        {
            return View();
        }


        public async Task<IActionResult> Login(string nombreUsuario, string contrasena)
        {
            ViewBag.Error = null;
            TempData["MensajeUsername"] = "";
            var query = "SELECT * FROM usuarios WHERE nombre_usuario = @NombreUsuario AND contrasena = @Contrasena";
            var parametros = new[]
            {
                 new MySqlParameter("@NombreUsuario", nombreUsuario),
                 new MySqlParameter("@Contrasena", contrasena)
             };

            var resultado = _dbHelper.VerDatos(query, parametros);
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.NombreUsuario == nombreUsuario && u.Contrasena == contrasena);
            if (resultado.Rows.Count > 0 || usuario != null)
            {
                var nombre = usuario != null ? usuario.NombreUsuario : resultado.Rows[0]["nombre_usuario"].ToString();
                var rol = usuario != null ? usuario.RolUsuario : resultado.Rows[0]["rol_usuario"].ToString();

                // Guardar en sesión
                HttpContext.Session.SetString("Usuario", nombre);
                HttpContext.Session.SetString("RolUsuario", rol);
                
                TempData["MensajeUsername"] = $"!     {nombre}    !";
                ViewBag.Mensaje = TempData.Peek("MensajeUsername"); 
                return RedirectToAction("Index", "Home"); 
            }
            else
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
            }
                
            return View("Login");
        }

        public IActionResult CerrarSesion()
        {
            HttpContext.Session.Clear(); 
            TempData["MensajeUsername"] = "";
            ViewBag.Mensaje = TempData.Peek("MensajeUsername");
            return RedirectToAction("Login", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult RegistrarUsuario()
        {
            return View();
        }

        public IActionResult Contactanos()
        {
            return View();
        }
                
        [HttpPost]
        public IActionResult EnviarMensaje(string nombre, string telefono, string email, string mensaje)
        {            
            ViewBag.MensajeExito = "Tu mensaje ha sido enviado correctamente. Nos pondremos en contacto contigo pronto.";
            Console.WriteLine($"Nombre: {nombre}, Teléfono: {telefono}, Email: {email}, Mensaje: {mensaje}");
            return View("Contactanos"); 
        }
    }
}