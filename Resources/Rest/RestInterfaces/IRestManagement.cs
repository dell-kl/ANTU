namespace ANTU.Resources.Rest.RestInterfaces
{
    public interface IRestManagement
    {
        public IMateriaPrima MateriaPrima { get; }

        public HttpClient httpClient { set; get; }
    }
}
