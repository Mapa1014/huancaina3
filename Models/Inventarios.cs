using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace huancaina.Models
{
    public class Inventarios
    {
        [Key]
        public required int IdInventario { get; set; }
        public required string Categoria { get; set; }
        public required decimal CantidadDisponible { get; set; }
        public required DateTime FechaCreacion { get; set; }
        public required DateTime FechaMovimiento { get; set; }
        public required int UsuariosIdUsuario { get; set; }
    }
}