using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace huancaina.Models
{

    [Table("usuarios")]
    public class Usuarios
    {
        [Key]
        [Required]
        [Column("id_usuario")]  // Nombre real de la columna en la BD
        public required int IdUsuario { get; set; }

        [Column("rol_usuario")]

        [Required]
        public required string RolUsuario { get; set; }

        [Column("nombre_usuario")]

        [Required]
        public required string NombreUsuario { get; set; }

        [Column("nombre_completo")]

        [Required]
        public required string NombreCompleto { get; set; }

        [Column("email")]

        [Required]
        public required string Email { get; set; }

        [Column("contrasena")]

        [Required]
        public required string Contraseña { get; set; }

        [Column("direccion")]

        [Required]
        public required string Direccion { get; set; }

        [Column("telefono")]

        [Required]
        public required string Telefono { get; set; }

        [Column("telefono_emergencia")]

        [Required]
        public required string TelefonoEmergencia { get; set; }

        [Column("fecha_registro")]
        public DateTime FechaRegistro { get; set; }
    }
}
