namespace ANTU.Models
{
    public class KgSeguimiento
    {
        
        public DateTime fechaCompra { set; get; } = DateTime.Now;

        public decimal precioTotal { set; get; } = 0;

        public double KgTotal { set; get; } = 0;

        public decimal precioUnitario { set; get; } = 0;

        public double KgEstandar { set; get; } = 0;
    }
}
