using System.ComponentModel.DataAnnotations;

namespace L01_2021RR650.Models
{
    public class comentarios
    {
        [Key]
        public int comentatioId { get; set; }
        public int publicacionId { get; set; }
        public int usuarioId { get; set; }
        public string comentario { get; set; }
    }
}
