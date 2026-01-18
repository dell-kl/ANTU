using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Business.Services.IServices;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Rest.RestInterfaces;
using Modelos;

namespace ANTU.ViewModel.ComponentsViewModel;

public partial class MateriaPrimaCollectionViewComponentsVIewModel : ParentViewModel
{
    [ObservableProperty]
    private ObservableCollection<MateriaPrimaProducto> _datosMateriaPrimaProductos = new ObservableCollection<MateriaPrimaProducto>();

    [ObservableProperty]
    private bool _isLazyLoading;
    
    public MateriaPrimaCollectionViewComponentsVIewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService) : base(restManagement, popupService, managementService)
    {
        
    }

    public async Task CargarDatosMateriaPrimaProducto()
    {
        await _managementService.materiaPrimaService.GetMateriaPrimaAync(
            this.DatosMateriaPrimaProductos.Count(),
            this.DatosMateriaPrimaProductos);
    }
    
    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task LoadMoreElements()
    {
        this.IsLazyLoading = true;
        TimeSpan.FromMilliseconds(10);
        await CargarDatosMateriaPrimaProducto();
        this.IsLazyLoading = false;
    }
    
    [RelayCommand(AllowConcurrentExecutions = false)]
    [SupportedOSPlatform("Android")]
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