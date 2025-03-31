using huancaina.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar servicios al contenedor de servicios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DatabaseHelper>();
// Configurar la base de datos
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configuración de servicios
builder.Services.AddControllersWithViews().AddCookieTempDataProvider();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
// Agregar servicios para sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true; 
    options.Cookie.IsEssential = true; 
});

builder.Services.AddControllersWithViews();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // Manejar errores en producción
    app.UseExceptionHandler("/Home/Error500");
    app.UseStatusCodePagesWithReExecute("/Home/Error404");
}
else
{
    // Mostrar errores detallados en desarrollo
    app.UseDeveloperExceptionPage();
}

app.UseSession();
app.Use(async (context, next) =>
{
    var path = context.Request.Path.ToString().ToLower();
    if (!path.Contains("/login") && string.IsNullOrEmpty(context.Session.GetString("Usuario")))
    {
        context.Response.Redirect("/Home/Login");
        return;
    }
    await next();
});

// Configurar middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

// configurar la aplicacion
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute( // Ruta de inicio
    name: "home",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "ordenes",
    pattern: "{controller=Ordenes}/{action=FormularioOrdenes}/{id_orden?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Ordenes}/{action=EliminarOrden}/{id_orden?}");

app.MapControllerRoute(
    name: "inventarios",
    pattern: "{controller=Inventarios}/{action=FormularioInventarios}/{id_orden?}");


app.Run();
