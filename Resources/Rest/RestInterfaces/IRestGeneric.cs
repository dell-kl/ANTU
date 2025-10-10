namespace ANTU.Resources.Rest.RestInterfaces
{
    public interface IRestGeneric<TRequestPost, TRequestGet> where TRequestPost: class
                                                            where TRequestGet : class
    {
        public Task<bool> Add(TRequestPost data);
        public Task<IEnumerable<TRequestGet>> Get(object data);
        public void Update();
        public void Delete();
    }
}
