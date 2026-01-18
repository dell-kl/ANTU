using Business.Services.IServices;
using Data.Rest.RestInterfaces;

namespace Business.Services;

public class ManagementService : IManagementService
{
    public IMateriaPrimaService materiaPrimaService { get; private set; }

    public ManagementService(IRestManagement restManagement)
    {
        this.materiaPrimaService = new MateriaPrimaService(restManagement);
    }
}