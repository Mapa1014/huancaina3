using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using huancaina.Models;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace huancaina.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly ILogger<OrdenesController> _logger;
        private readonly IConfiguration _configuration;
        private readonly DatabaseHelper _dbHelper;

        public OrdenesController(ILogger<OrdenesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _dbHelper = new DatabaseHelper(configuration);
        }

        public IActionResult VerOrdenes()
        {
            return View();
        }

        public IActionResult CrearOrdenes()
        {
            return View("CrearOrdenes");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LeerOrdenes()
        {
            try
            {
                string query = "SELECT * FROM ordenes"; 
                DataTable resultados = _dbHelper.VerDatos(query);

                ViewBag.DataTable = resultados;
                ViewBag.Mensaje = "Consulta ejecutada correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = $"Error: {ex.Message}";
            }

            return View("VerOrdenes");
        }

        [HttpPost]
        public IActionResult CrearOrdenes(int id_orden, int n_mesa, DateTime fecha_orden, string estado, string observaciones, int id_usuario)
        {
            try
            {                
                string query = "INSERT INTO ordenes (id_orden, n_mesa, fecha_orden, estado, observaciones, usuarios_id_usuario) " +
                               "VALUES (@IdOrden, @NMesa, @FechaOrden, @Estado, @Observaciones, @IdUsuario)";
                                
                var parametros = new[]
                {
                new MySqlParameter("@IdOrden", id_orden),
                new MySqlParameter("@NMesa", n_mesa),
                new MySqlParameter("@FechaOrden", fecha_orden),
                new MySqlParameter("@Estado", estado),
                new MySqlParameter("@Observaciones", observaciones),
                new MySqlParameter("@IdUsuario", id_usuario)
            };
                                
                _dbHelper.InsertarDatos(query, parametros);
                                
                TempData["Mensaje"] = "Orden creada exitosamente.";
            }
            catch (Exception ex)
            {
                // Manejo de errores
                TempData["Mensaje"] = $"Error al crear la orden: {ex.Message}";
            }

            
            return RedirectToAction("LeerOrdenes");
        }
    }
}


