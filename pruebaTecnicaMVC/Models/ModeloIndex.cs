using pruebaTecnicaMVC.Controllers;

namespace pruebaTecnicaMVC.Models
{
    public class ModeloIndex
    {
        public int? pagina { get; set; }
        public int? tamaño { get; set; }
        public Foto[] Fotos { get; set; }
        public int TotalCount { get; set; }
    }
}

