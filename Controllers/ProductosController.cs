using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using huancaina.Models;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;


public class ProductosController : Controller
{
    private readonly ILogger<UsuariosController> _logger;
    private readonly IConfiguration _configuration;
    private readonly DatabaseHelper _dbHelper;
    public ProductosController(ILogger<UsuariosController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
        _dbHelper = new DatabaseHelper(configuration);
    }
    public IActionResult VerProductos()
    {
        return View();
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    
    public IActionResult LeerProductos()
    {
        var usuarioSesion = HttpContext.Session.GetString("Usuario");
        if (string.IsNullOrEmpty(usuarioSesion))
        {
            return RedirectToAction("Login", "Home");
        }
        try
        {
            string query = "SELECT * FROM productos";
            DataTable resultados = _dbHelper.VerDatos(query);

            ViewBag.DataTable = resultados;
            ViewBag.Mensaje = "Consulta ejecutada correctamente.";
        }
        catch (Exception ex)
        {
            ViewBag.Mensaje = $"Error: {ex.Message}";
        }
        return View("VerProductos");
    }
}

