using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ProyectoBase.Filters // Cambia 'ProyectoBase' por el nombre de tu proyecto
{
    public class SesionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session.GetString("Usuario");

            if (string.IsNullOrEmpty(session)) // Si no hay sesión activa
            {
                context.Result = new RedirectToActionResult("Login", "Home", null); // Redirigir a login
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
