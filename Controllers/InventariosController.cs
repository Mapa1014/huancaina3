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
        private readonly DatabaseHelper _dbHelper;

        public InventariosController(ILogger<InventariosController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _dbHelper = new DatabaseHelper(configuration);
        }

        public IActionResult VerInventario()
        {
            return View();
        }

        public IActionResult CrearInventario()
        {
            return View("CrearInventario");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult LeerInventario()
        {
            try
            {
                string query = "SELECT * FROM inventarios";                 
                DataTable resultados = _dbHelper.VerDatos(query);

                ViewBag.DataTable = resultados;
                ViewBag.Mensaje = "Consulta ejecutada correctamente.";
            }
            catch (Exception ex)
            {
                ViewBag.Mensaje = $"Error: {ex.Message}";
            }

            return View("VerInventario");
        }
        [HttpPost]
        public IActionResult CrearInventario(int id_inventario, string categoria, int cantidad_disponible, DateTime fecha_creacion, DateTime fecha_movimiento, int id_usuario)
        {
            try
            {                
                string query = "INSERT INTO inventarios (id_inventario, categoria, cantidad_disponible, fecha_creacion, fecha_movimiento, usuarios_id_usuario) " +
                               "VALUES (@IdInventario, @Categoria, @CantidadDisponible, @FechaCreacion, @FechaMovimiento, @IdUsuario)";

                // Parametrizar la consulta para evitar inyecciones SQL
                var parametros = new[]
                {
                new MySqlParameter("@IdInventario", id_inventario),
                new MySqlParameter("@Categoria", categoria),
                new MySqlParameter("@CantidadDisponible", cantidad_disponible),
                new MySqlParameter("@FechaCreacion", fecha_creacion),
                new MySqlParameter("@FechaMovimiento", fecha_movimiento),
                new MySqlParameter("@IdUsuario", id_usuario)
            };
                
                _dbHelper.InsertarDatos(query, parametros);
                                
                TempData["Mensaje"] = "Inventario creado exitosamente.";
            }
            catch (Exception ex)
            {                
                TempData["Mensaje"] = $"Error al crear el inventario: {ex.Message}";
            }
                        
            return RedirectToAction("LeerInventario");
        }
    }
}

