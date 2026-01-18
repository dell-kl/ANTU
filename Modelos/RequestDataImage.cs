
namespace Modelos
{
    public class RequestDataImage
    {
        public string mensaje { set; get; } = null!;
        public ICollection<DataImage> imagenes = new List<DataImage>();
    }
}
