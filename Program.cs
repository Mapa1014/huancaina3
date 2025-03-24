using huancaina.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<DatabaseHelper>();
// Configure the database context to use MySQL
var connectionString = builder.Configuration.GetConnectionString("MySqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

// configure the app to use the database context
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.MapControllerRoute(
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
