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
        string mensaje = "";
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
            var usuarioSesion = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioSesion))
            {
                return RedirectToAction("Login", "Home");
            }
            try
            {
                string query = "SELECT * FROM ordenes";
                DataTable resultados = _dbHelper.VerDatos(query);

                ViewBag.DataTable = resultados;
                mensaje = "Consulta ejecutada correctamente.";                
            }
            catch (Exception ex)
            {
                mensaje = $"Error: {ex.Message}";
            }
            TempData["MensajeOrdenes"] = mensaje;
            return View("VerOrdenes");
        }
        public async Task<IActionResult> FormularioOrdenes(string accion, int? id_orden = null)
        {
            var mesasOcupadas = _dbHelper.ObtenerNMesa();
            int[] mesas = Enumerable.Range(1, 16).ToArray();            
            int[] mesasDisponibles = mesas.Except(mesasOcupadas).ToArray();
            ViewBag.Mesas = mesasDisponibles.Select(i => new SelectListItem
            {
                Value = i.ToString(), 
                Text = i.ToString()
            }).ToList();

            ViewBag.Accion = accion; // "Crear" o "Actualizar"
            ViewBag.Orden = null;
            var usuarioSesion = HttpContext.Session.GetString("Usuario");
            if (string.IsNullOrEmpty(usuarioSesion))
            {
                return RedirectToAction("Login", "Home");
            }
            var estados = new List<SelectListItem>
            {
                new SelectListItem { Value = "PENDIENTE", Text = "Pendiente", Disabled = true},
                new SelectListItem { Value = "EN PREPARACION", Text = "En preparación", Selected = true},
                new SelectListItem { Value = "LISTO PARA SERVIR", Text = "Listo para servir", Disabled = true },
                new SelectListItem { Value = "ENTREGADO", Text = "Entregado", Disabled = true },
                new SelectListItem { Value = "CANCELADO", Text = "Cancelado", Disabled = true  }
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
            mensaje = "Formulario cargado correctamente.";
            TempData.Remove("MensajeOrdenes");
            TempData["MensajeOrdenes"] = mensaje;
            TempData.Keep("MensajeOrdenes");
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

                    // FALTA INSERTAR el próximo ID de orden
                    string query1 = "SELECT COALESCE(MAX(id_orden), 0) + 1 AS ProximoId FROM ordenes";
                    int proximoId = Convert.ToInt32(_dbHelper.ObtenerDato(query1));
                    Console.WriteLine("proximoId: " + proximoId);
                    ViewBag.ProximoIdOrden = proximoId;                    

                    mensaje = "Orden creada exitosamente.";
                    TempData.Remove("MensajeOrdenes");
                    TempData["MensajeOrdenes"] = mensaje;
                    return RedirectToAction("LeerOrdenes");
                }
                else if (accion == "Actualizar")
                {
                    query = "UPDATE ordenes SET n_mesa = @NMesa, fecha_orden = @FechaOrden, estado = @Estado, " +
                                   "observaciones = @Observaciones, usuarios_id_usuario = @IdUsuario WHERE id_orden = @IdOrden";

                    _dbHelper.ActualizarDatos(query, parametros);                    
                    return RedirectToAction("LeerOrdenes");
                }
            }
            catch (Exception ex)
            {
                mensaje = $"Error al guardar la orden: {ex.Message}";
            }
            TempData.Remove("MensajeOrdenes");
            TempData["MensajeOrdenes"] = mensaje;
            TempData.Keep("MensajeOrdenes");
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
                mensaje = "Orden eliminada exitosamente.";
            }
            catch (Exception ex)
            {
                mensaje = $"Error al eliminar la orden: {ex.Message}";
            }
            TempData.Remove("MensajeOrdenes");
            TempData["MensajeOrdenes"] = mensaje;
            TempData.Keep("MensajeOrdenes");
            return RedirectToAction("LeerOrdenes");
        }    
    }    
}