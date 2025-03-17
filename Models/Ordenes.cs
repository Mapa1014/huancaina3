namespace huancaina.Models
{
    public class Ordenes
    {
        public required int IdOrden { get; set; }
        public required int NMesa { get; set; }
        public required DateTime FechaOrden { get; set; }
        public required string Estado { get; set; } // Usaremos string para ENUM.
        public string? Observaciones { get; set; } // Campo opcional.
        public required int UsuariosIdUsuario { get; set; }
    }
}
