using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Business.Services.IServices;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Rest.RestInterfaces;
using Modelos;

namespace ANTU.ViewModel.ComponentsViewModel;

[SupportedOSPlatform("Android")]
public partial class MateriaPrimaCollectionViewComponentsVIewModel : ParentViewModel
{
    [ObservableProperty]
    private ObservableCollection<MateriaPrimaProducto> _datosMateriaPrimaProductos = new ObservableCollection<MateriaPrimaProducto>();

    [ObservableProperty]
    private bool _isLazyLoading;


    public MateriaPrimaCollectionViewComponentsVIewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService) : base(restManagement, popupService, managementService)
    {
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task CargarDatosMateriaPrimaProducto()
    {
        if (this.IsLazyLoading)
            return; 
        
        this.IsLazyLoading = true;

        CancellationTokenSource tokenSource = new CancellationTokenSource();
        
        IEnumerable<MateriaPrimaProducto> listado = await ManagementService.materiaPrimaService.GetMateriaPrimaAync(
            this.DatosMateriaPrimaProductos.Count(),
            tokenSource.Token);

        foreach (MateriaPrimaProducto item in listado)
        {
            this.DatosMateriaPrimaProductos.Add(item);
        }

        this.IsLazyLoading = false;
    }
    
    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task NavegarPaginaDetalle(string guid)
    {
        MateriaPrimaProducto? registro = this.DatosMateriaPrimaProductos.Where(item => item.guid.Equals(guid)).ToList()
            .FirstOrDefault();
        
        var datosNavegacion = new ShellNavigationQueryParameters {
            {
                "DataQuery", registro is null ? "" : registro
            }
        };
        
        await base.NavegarFormulario("MateriaPrimaDetalle",datosNavegacion);
    }
}