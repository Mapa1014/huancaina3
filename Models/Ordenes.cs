﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace huancaina.Models
{
    public class Ordenes
    {
        [Key]
        public required int IdOrden { get; set; }
        public required int NMesa { get; set; }
        public required DateTime FechaOrden { get; set; }
        public required string Estado { get; set; }
        public string? Observaciones { get; set; }
        public required int UsuariosIdUsuario { get; set; }
    }
}