namespace huancaina.Models
{
    public class Productos
    {
        public required int IdProducto { get; set; }
        public required string NombreProducto { get; set; }
        public required string UnidadMedida { get; set; }
        public required string Categoria { get; set; }
        public required decimal CantidadProducto { get; set; }
        public required decimal PrecioUnitario { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public string Estado { get; set; } = "DISPONIBLE";
        public string? Descripcion { get; set; }
        public required int InventariosIdInventario { get; set; }
        public required int InventariosUsuariosIdUsuario { get; set; }
    }
}
