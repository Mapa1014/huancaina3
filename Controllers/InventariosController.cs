using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using huancaina.Models;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult LeerInventario()
        {
            var usuarioSesion = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioSesion))
            {
                return RedirectToAction("Login", "Home");
            }
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
        public IActionResult FormularioInventarios(string accion, int? id_inventario = null)
        {
            ViewBag.Accion = accion; // "Crear" o "Actualizar"
            ViewBag.Inventario = null;

            var categorias = new List<SelectListItem>
            {
                new SelectListItem { Value = "Carnes", Text = "Carnes" },
                new SelectListItem { Value = "Lácteos", Text = "Lácteos" },
                new SelectListItem { Value = "Pescados Y Mariscos", Text = "Pescados Y Mariscos" },
                new SelectListItem { Value = "Bebidas", Text = "Bebidas" },
                new SelectListItem { Value = "Verduras", Text = "Verduras" }
            };

            if (accion == "Actualizar" && id_inventario.HasValue)
            {
                string query = "SELECT * FROM inventarios WHERE id_inventario = @IdInventario";
                var parametros = new[] { new MySqlParameter("@IdInventario", id_inventario.Value) };

                DataTable resultado = _dbHelper.VerDatos(query, parametros);

                if (resultado.Rows.Count > 0)
                {
                    ViewBag.Inventario = resultado.Rows[0];

                    var categoriaSeleccionada = resultado.Rows[0]["categoria"].ToString();
                    categorias.ForEach(c => c.Selected = c.Value == categoriaSeleccionada);
                }
            }

            ViewBag.Categorias = categorias;
            return View("FormularioInventarios");
        }


        public IActionResult GuardarInventario(int id_inventario, string categoria, int cantidad_disponible, DateTime fecha_creacion, DateTime fecha_movimiento, int id_usuario, string accion)
        {
            try
            {
                string query;
                var parametros = new[]
                {
                    new MySqlParameter("@IdInventario", id_inventario),
                    new MySqlParameter("@Categoria", categoria),
                    new MySqlParameter("@CantidadDisponible", cantidad_disponible),
                    new MySqlParameter("@FechaCreacion", fecha_creacion),
                    new MySqlParameter("@FechaMovimiento", fecha_movimiento),
                    new MySqlParameter("@IdUsuario", id_usuario)
                };

                if (accion == "Crear")
                {
                    query = "INSERT INTO inventarios (id_inventario, categoria, cantidad_disponible, fecha_creacion, fecha_movimiento, usuarios_id_usuario) " +
                            "VALUES (@IdInventario, @Categoria, @CantidadDisponible, @FechaCreacion, @FechaMovimiento, @IdUsuario)";

                    _dbHelper.InsertarDatos(query, parametros);
                    TempData["MensajeInventarios"] = "Inventario creado exitosamente.";

                }
                else if (accion == "Actualizar")
                {
                    query = "UPDATE inventarios SET categoria = @Categoria, cantidad_disponible = @CantidadDisponible, fecha_movimiento = @FechaMovimiento, usuarios_id_usuario = @IdUsuario " +
                            "WHERE id_inventario = @IdInventario";

                    _dbHelper.ActualizarDatos(query, parametros);
                    TempData["MensajeInventarios"] = "Inventario actualizado exitosamente.";
                    return RedirectToAction("LeerInventario");

                }
            }
            catch (Exception ex)
            {
                TempData["MensajeInventarios"] = $"Error al guardar el inventario: {ex.Message}";

            }
            return RedirectToAction("LeerInventario");
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

                TempData["MensajeInventarios"] = "Inventario eliminado exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["MensajeInventarios"] = $"Error al eliminar el inventario: {ex.Message}";
            }

            return RedirectToAction("LeerInventario");
        }
    }
}