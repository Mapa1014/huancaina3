using Microsoft.AspNetCore.Mvc;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using huancaina.Models;

namespace huancaina.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly ILogger<OrdenesController> _logger;
        private readonly DatabaseHelper _dbHelper;
        private readonly ApplicationDbContext _context;

        public OrdenesController(ILogger<OrdenesController> logger, DatabaseHelper dbHelper, ApplicationDbContext context)
        {
            _dbHelper = dbHelper;
            _logger = logger;
            _context = context;            
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
        public async Task<IActionResult> FormularioOrdenes(string accion, int? id_orden = null)
        {
            ViewBag.Accion = accion; // "Crear" o "Actualizar"
            ViewBag.Orden = null;
            
            var estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "PENDIENTE", Text = "Pendiente" },
                new SelectListItem { Value = "EN PREPARACION", Text = "En preparación" },
                new SelectListItem { Value = "LISTO PARA SERVIR", Text = "Listo para servir" },
                new SelectListItem { Value = "ENTREGADO", Text = "Entregado" },
                new SelectListItem { Value = "CANCELADO", Text = "Cancelado" }
            }; 
            var productos = await _context.Productos.ToListAsync() ?? new List<Productos>();
            ViewBag.Productos = productos ?? new List<Productos>(); 

            if (accion == "Actualizar" && id_orden.HasValue)
            {
                string query = "SELECT * FROM ordenes WHERE id_orden = @IdOrden";
                var parametros = new[] { new MySqlParameter("@IdOrden", id_orden.Value) };

                DataTable resultado = _dbHelper.VerDatos(query, parametros);

                if (resultado.Rows.Count > 0)
                {
                    ViewBag.Orden = resultado.Rows[0];
                    var estadoSeleccionado = resultado.Rows[0]["estado"].ToString();
                    estados.ForEach(e => e.Selected = e.Value == estadoSeleccionado);
                }
            }
            ViewBag.Estados = estados; 
            return View("FormularioOrdenes");
        }

        [HttpPost]
        public IActionResult GuardarOrden(string accion, int id_orden, int n_mesa, DateTime fecha_orden, string estado, string observaciones, int id_usuario)
        {
            try
            {
                string query;
                var parametros = new[]
                {
                    new MySqlParameter("@IdOrden", id_orden),
                    new MySqlParameter("@NMesa", n_mesa),
                    new MySqlParameter("@FechaOrden", fecha_orden),
                    new MySqlParameter("@Estado", estado),
                    new MySqlParameter("@Observaciones", observaciones),
                    new MySqlParameter("@IdUsuario", id_usuario)
                };
                if (accion == "Crear")
                {
                    query = "INSERT INTO ordenes (id_orden, n_mesa, fecha_orden, estado, observaciones, usuarios_id_usuario) " +
                                   "VALUES (@IdOrden, @NMesa, @FechaOrden, @Estado, @Observaciones, @IdUsuario)";
                    
                    _dbHelper.InsertarDatos(query, parametros);
                    TempData["MensajeOrdenes"] = "Orden creada exitosamente.";


                    // FALTA INSERTAR el próximo ID de orden
                    string query1 = "SELECT COALESCE(MAX(id_orden), 0) + 1 AS ProximoId FROM ordenes";
                    int proximoId = Convert.ToInt32(_dbHelper.ObtenerDato(query1));
                    ViewBag.ProximoIdOrden = proximoId;

                    return RedirectToAction("LeerOrdenes");
                }
                else if (accion == "Actualizar")
                {
                    query = "UPDATE ordenes SET n_mesa = @NMesa, fecha_orden = @FechaOrden, estado = @Estado, " +
                                   "observaciones = @Observaciones, usuarios_id_usuario = @IdUsuario WHERE id_orden = @IdOrden";
                    
                    _dbHelper.ActualizarDatos(query, parametros);
                    TempData["MensajeOrdenes"] = "Orden actualizada exitosamente.";
                    return RedirectToAction("LeerOrdenes");
                }
            }
            catch (Exception ex)
            {
                TempData["MensajeOrdenes"] = $"Error al guardar la orden: {ex.Message}";
            }
            return RedirectToAction("LeerOrdenes");
        }
        public IActionResult EliminarOrden(int id_orden)
        {
            try
            {
                string query = "DELETE FROM ordenes WHERE id_orden = @IdOrden";
                var parametros = new[]
                {
                     new MySqlParameter("@IdOrden", id_orden)
                };

                _dbHelper.EliminarDatos(query, parametros);
                TempData["MensajeOrdenes"] = "Orden eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                TempData["Mensaje"] = $"Error al eliminar la orden: {ex.Message}";
            }
            return RedirectToAction("LeerOrdenes");
        }
    }
}
