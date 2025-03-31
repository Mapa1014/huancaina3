
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace huancaina.Models
{    

    [Table("inventarios")]
    public class Inventarios
    {
        [Key]
        [Column("id_inventario")]
        public int IdInventario { get; set; }

        [Column("categoria")]

        [Required]
        public required string Categoria { get; set; }

        [Column("cantidad_disponible")]
        public decimal CantidadDisponible { get; set; }

        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        [Column("fecha_movimiento")]
        public DateTime FechaMovimiento { get; set; } = DateTime.Now;

        // Clave foránea hacia Usuario
        [ForeignKey("Usuario")]
        [Column("usuarios_id_usuario")]
        public int UsuarioId { get; set; }

        [Required]

        public required virtual Usuarios usuarios { get; set; } // Relación con Usuario
    }
}
