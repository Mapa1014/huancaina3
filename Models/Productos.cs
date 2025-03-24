using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace huancaina.Models
{
    [Table("Productos")]
    public class Productos

    {
        [Key]
        [Column("id_producto")]
        public required int IdProducto { get; set; }

        [Column("nombre_producto")]
        public required string NombreProducto { get; set; }
        [Column("unidad_medida")]
        public required string UnidadMedida { get; set; }
        [Column("categoria")]
        public required string Categoria { get; set; }
        [Column("cantidad_producto")]
        public required decimal CantidadProducto { get; set; }
        [Column("precio_unitario")]
        public required decimal PrecioUnitario { get; set; }
        [Column("fecha_actualizacion")]
        public DateTime FechaActualizacion { get; set; }
        [Column("estado")]
        public string Estado { get; set; } = "DISPONIBLE";
        [Column("descripcion")]
        public string? Descripcion { get; set; }
        [Column("inventarios_id_inventario")]
        public required int InventariosIdInventario { get; set; }
        [Column("inventarios_usuarios_id_usuario")]
        public required int InventariosUsuariosIdUsuario { get; set; }
    }
}