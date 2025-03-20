using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using huancaina.Models;
using huancaina.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;


public class UsuariosController : Controller
{
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
}

