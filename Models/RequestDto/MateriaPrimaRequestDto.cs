
namespace ANTU.Models.RequestDto
{
    public class MateriaPrimaRequestDto
    {
        public string? id_dto { get; set; } = Guid.NewGuid().ToString();

        public string nombre_dto { set; get; } = null!;

        public IEnumerable<KgSeguimientoRequestDto> KgMonitoringDtos { set; get; } = new List<KgSeguimientoRequestDto>();
    }
}
