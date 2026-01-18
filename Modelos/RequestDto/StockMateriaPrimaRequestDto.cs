namespace Modelos.RequestDto
{
    public class StockMateriaPrimaRequestDto
    {
        public string Identificador { set; get; } = null!;

        public int Amount { set; get; } 

        public double KgStandard { set; get; }

        public double PriceUnit { set; get; }
    }
}
