using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using huancaina.Models;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Renci.SshNet.Messages;


public class UsuariosController : Controller
{
    string mensaje = "";
    private readonly ILogger<UsuariosController> _logger;
    private readonly IConfiguration _configuration;
    private readonly DatabaseHelper _dbHelper;

    public UsuariosController(ILogger<UsuariosController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _dbHelper = new DatabaseHelper(configuration);
    }

    public IActionResult VerUsuarios()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult LeerUsuarios()
    {
        var usuarioSesion = HttpContext.Session.GetString("Usuario");
        if (string.IsNullOrEmpty(usuarioSesion))
        {
            return RedirectToAction("Login", "Home");
        }
        try
        {
            string query = "SELECT * FROM usuarios";
            DataTable resultados = _dbHelper.VerDatos(query);

            ViewBag.DataTable = resultados;
            ViewBag.Mensaje = "Consulta ejecutada correctamente.";
        }
        catch (Exception ex)
        {
            ViewBag.Mensaje = $"Error: {ex.Message}";
        }

        return View("VerUsuarios");
    }

    public async Task<IActionResult> FormularioUsuarios(string accion, int? id_usuario = null)
    {
        ViewBag.Accion = accion; // "Crear" o "Actualizar"
        ViewBag.Usuario = null;

        if (accion == "Actualizar" && id_usuario.HasValue)
        {
            string query = "SELECT * FROM usuarios WHERE id_usuario = @IdUsuario";
            var parametros = new[] { new MySqlParameter("@IdUsuario", id_usuario.Value) };

            DataTable resultado = _dbHelper.VerDatos(query, parametros);            
            if (resultado.Rows.Count > 0)
            {
                ViewBag.Usuario = resultado.Rows[0];
            }
        }

        return View("FormularioUsuarios");
    }

    [HttpPost]
    public IActionResult GuardarUsuario(string accion, int id_usuario, string contrasena, string rol_usuario, string nombre_usuario,
                                        string nombre_completo, string email, string direccion, string telefono, string telefono_emergencia, string fecha_registro)
    {
        try
        {
            string query;
            var parametros = new[]
            {
                new MySqlParameter("@IdUsuario", id_usuario),
                new MySqlParameter("@Contrasena", contrasena),
                new MySqlParameter("@RolUsuario", rol_usuario),
                new MySqlParameter("@NombreUsuario", nombre_usuario),
                new MySqlParameter("@NombreCompleto", nombre_completo),
                new MySqlParameter("@Email", email),
                new MySqlParameter("@Direccion", direccion),
                new MySqlParameter("@Telefono", telefono),
                new MySqlParameter("@TelefonoEmergencia", telefono_emergencia),
                new MySqlParameter("@FechaRegistro", fecha_registro)
            };
            if (accion == "Crear")
            {
                query = "INSERT INTO usuarios (id_usuario, contrasena, rol_usuario, nombre_usuario, nombre_completo, email, direccion, telefono, telefono_emergencia, fecha_registro) " +
                        "VALUES (@IdUsuario, @Contrasena, @RolUsuario, @NombreUsuario, @NombreCompleto, @Email, @Direccion, @Telefono, @TelefonoEmergencia, @FechaRegistro)";

                _dbHelper.InsertarDatos(query, parametros);
                mensaje = "Usuario creado exitosamente.";
            }
            else if (accion == "Actualizar")
            {
                query = "UPDATE usuarios SET contrasena = @Contrasena, rol_usuario = @RolUsuario, nombre_usuario = @NombreUsuario, " +
                        "nombre_completo = @NombreCompleto, email = @Email, direccion = @Direccion, telefono = @Telefono, " +
                        "telefono_emergencia = @TelefonoEmergencia, fecha_registro = @FechaRegistro WHERE id_usuario = @IdUsuario";

                _dbHelper.ActualizarDatos(query, parametros);
                mensaje = "Usuario actualizado exitosamente.";
            }
        }
        catch (Exception ex)
        {
            mensaje = $"Error al guardar el usuario: {ex.Message}";
        }
        TempData.Remove("MensajeUsuarios");
        TempData["MensajeUsuarios"] = mensaje;
        TempData.Keep("MensajeUsuarios");

        return RedirectToAction("LeerUsuarios");
    }

    public IActionResult EliminarUsuario(int id_usuario)
    {
        try
        {
            string query = "DELETE FROM usuarios WHERE id_usuario = @IdUsuario";
            var parametros = new[] { new MySqlParameter("@IdUsuario", id_usuario) };

            _dbHelper.EliminarDatos(query, parametros);
            mensaje = "Usuario eliminado exitosamente.";
        }
        catch (Exception ex)
        {
            mensaje = $"Error al eliminar el usuario: {ex.Message}";
        }
        TempData.Remove("MensajeUsuarios");
        TempData["MensajeUsuarios"] = mensaje;
        TempData.Keep("MensajeUsuarios");
        return RedirectToAction("LeerUsuarios");
    }
}

