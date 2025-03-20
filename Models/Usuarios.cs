using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace huancaina.Models
{
    public class Usuarios
    {
        [Key]
        public required int IdUsuario { get; set; }
        public required string RolUsuario { get; set; }
        public required string NombreUsuario { get; set; }
        public required string NombreCompleto { get; set; }
        public required string Email { get; set; }
        public required string Contraseña { get; set; }
        public required string Direccion { get; set; }
        public required string Telefono { get; set; }
        public required string TelefonoEmergencia { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
