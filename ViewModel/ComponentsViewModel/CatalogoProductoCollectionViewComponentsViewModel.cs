using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Business.Services.IServices;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Rest.RestInterfaces;
using Modelos;

namespace ANTU.ViewModel.ComponentsViewModel;

public partial class CatalogoProductoCollectionViewComponentsViewModel : ParentViewModel
{
    [ObservableProperty]
    private ObservableCollection<CatalogoProducto> _datosCatalogoProductos = new ObservableCollection<CatalogoProducto>();
    
    [ObservableProperty]
    private bool _isLazyLoading;
    
    public CatalogoProductoCollectionViewComponentsViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService) : base(restManagement, popupService, managementService)
    {
        
    }

    public Task CargarDatosCatalogoProducto()
    {
        // llamamos al service para trar los datos de Catalogo Productos.
        return Task.CompletedTask;
    }
    
    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task LoadMoreElements()
    {
        this.IsLazyLoading = true;
        TimeSpan.FromMilliseconds(10);
        await CargarDatosCatalogoProducto();
        this.IsLazyLoading = false;
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    [SupportedOSPlatform("Android")]
    public async Task NavegarPaginaDetalle(string guid)
    {
        CatalogoProducto? registro = this.DatosCatalogoProductos.Where(item => item.Identificador.Equals(guid)).ToList()
            .FirstOrDefault();
        
        var datosNavegacion = new ShellNavigationQueryParameters {
            {
                "DataQuery", registro is null ? "" : registro
            }
        };
    
        
        await base.NavegarFormulario("CatalogoProductoDetalle",datosNavegacion);
    }
}