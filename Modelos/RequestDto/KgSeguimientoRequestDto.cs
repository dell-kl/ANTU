namespace Modelos.RequestDto
{
    public class KgSeguimientoRequestDto
    {
        public string? id_dto { set; get; } = Guid.NewGuid().ToString();
        public int cantidad_dto { set; get; } = 0;
        public double kg_standard { set; get; } = 0.0d;
        public decimal price_dto { set; get; } = 0.0m;
    }
}
