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

        public IActionResult VerInventarios()
        {
            return View("LeerInventario");
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

        public IActionResult ActualizarInventario(int id_inventario, string categoria, int cantidad_disponible, DateTime fecha_movimiento)
        {
            try
            {
                string query = "UPDATE inventarios SET categoria = @Categoria, cantidad_disponible = @CantidadDisponible, " +
                               "fecha_movimiento = @FechaMovimiento WHERE id_inventario = @IdInventario";

                var parametros = new[]
                {
                    new MySqlParameter("@IdInventario", id_inventario),
                    new MySqlParameter("@Categoria", categoria),
                    new MySqlParameter("@CantidadDisponible", cantidad_disponible),
                    new MySqlParameter("@FechaMovimiento", fecha_movimiento)
                };

                _dbHelper.ActualizarDatos(query, parametros);

                TempData["Mensaje"] = "Inventario actualizado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al actualizar el inventario: {ex.Message}";
            }

            return RedirectToAction("CrearInventario");
        }
        public IActionResult EliminarInventario(int id_inventario)
        {
            try
            {
                string query = "DELETE FROM inventarios WHERE id_inventario = @IdInventario";

                var parametros = new[]
                {
                    new MySqlParameter("@IdInventario", id_inventario)
                };

                _dbHelper.EliminarDatos(query, parametros);

                TempData["Mensaje"] = "Inventario eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al eliminar el inventario: {ex.Message}";
            }

            return RedirectToAction("LeerInventario");
        }

        /*

        public IActionResult GuardarInventario(int id_inventario, string categoria, int cantidad_disponible, DateTime fecha_creacion, DateTime fecha_movimiento, int id_usuario, string accion)
        {
            try
            {
                string query;

                if (accion == "Crear") // Lógica para crear un inventario
                {
                    query = "INSERT INTO inventarios (id_inventario, categoria, cantidad_disponible, fecha_creacion, fecha_movimiento, usuarios_id_usuario) " +
                            "VALUES (@IdInventario, @Categoria, @CantidadDisponible, @FechaCreacion, @FechaMovimiento, @IdUsuario)";
                }
                else if (accion == "Actualizar") // Lógica para actualizar un inventario
                {
                    query = "UPDATE inventarios SET categoria = @Categoria, cantidad_disponible = @CantidadDisponible, fecha_movimiento = @FechaMovimiento " +
                            "WHERE id_inventario = @IdInventario";
                }
                else
                {
                    throw new Exception("Acción no válida. Debe ser 'Crear' o 'Actualizar'.");
                }

                // Definir los parámetros de la consulta
                var parametros = new[]
                {
                    new MySqlParameter("@IdInventario", id_inventario),
                    new MySqlParameter("@Categoria", categoria),
                    new MySqlParameter("@CantidadDisponible", cantidad_disponible),
                    new MySqlParameter("@FechaCreacion", fecha_creacion), // Ignorado si la acción es "Actualizar"
                    new MySqlParameter("@FechaMovimiento", fecha_movimiento),
                    new MySqlParameter("@IdUsuario", id_usuario)
                };

                // Ejecutar la consulta
                if (accion == "Crear")
                {
                    _dbHelper.InsertarDatos(query, parametros);
                    TempData["Mensaje"] = "Inventario creado exitosamente.";
                }
                else if (accion == "Actualizar")
                {
                    _dbHelper.ActualizarDatos(query, parametros);
                    TempData["Mensaje"] = "Inventario actualizado exitosamente.";
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al guardar el inventario: {ex.Message}";
            }

            return RedirectToAction("LeerInventario");
        }
        public IActionResult FormularioCrearInventario()
        {
            ViewBag.TituloFormulario = "Crear Inventario";
            ViewBag.AccionFormulario = "CrearInventario";
            ViewBag.TextoBoton = "Registrar";

            ViewBag.IdInventario = ""; // Valores vacíos para crear
            ViewBag.IdUsuario = "";
            ViewBag.Categoria = "";
            ViewBag.CantidadDisponible = "";
            ViewBag.FechaCreacion = "";
            ViewBag.FechaMovimiento = "";

            return View("CrearInventario");
        }

        [HttpGet]
        public IActionResult FormularioActualizarInventario(int id_inventario)
        {
            try
            {
                string query = "SELECT * FROM inventarios WHERE id_inventario = @IdInventario";
                var parametros = new[]
                {
            new MySqlParameter("@IdInventario", id_inventario)
        };

                DataTable result = _dbHelper.VerDatos(query);

                if (result.Rows.Count > 0)
                {
                    var fila = result.Rows[0];
                    ViewBag.TituloFormulario = "Actualizar Inventario";
                    ViewBag.AccionFormulario = "ActualizarInventario";
                    ViewBag.TextoBoton = "Actualizar";

                    // Cargar datos en ViewBag para prellenar el formulario
                    ViewBag.IdInventario = fila["id_inventario"];
                    ViewBag.IdUsuario = fila["usuarios_id_usuario"];
                    ViewBag.Categoria = fila["categoria"];
                    ViewBag.CantidadDisponible = fila["cantidad_disponible"];
                    ViewBag.FechaCreacion = fila["fecha_creacion"];
                    ViewBag.FechaMovimiento = fila["fecha_movimiento"];
                }
                else
                {
                    TempData["Mensaje"] = "El inventario no fue encontrado.";
                    return RedirectToAction("LeerInventario");
                }
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al cargar el formulario: {ex.Message}";
                return RedirectToAction("LeerInventario");
            }

            return View("FormularioInventario"); // Reutiliza el formulario dinámico
        }

        */
    }
}

