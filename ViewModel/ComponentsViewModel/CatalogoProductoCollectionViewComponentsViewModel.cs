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
public partial class CatalogoProductoCollectionViewComponentsViewModel : ParentViewModel
{
    [ObservableProperty]
    private ObservableCollection<CatalogoProducto> _datosCatalogoProductos;
    
    [ObservableProperty]
    private bool _isLazyLoading;
    
    public CatalogoProductoCollectionViewComponentsViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService) : base(restManagement, popupService, managementService)
    {
        this.DatosCatalogoProductos = new ObservableCollection<CatalogoProducto>();
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task CargarDatosCatalogoProducto()
    {
        if (this.IsLazyLoading)
            return;
        
        this.IsLazyLoading = true;

        var listado = await ManagementService.CatalogoProductoService.GetCatalogoProductosAync(this.DatosCatalogoProductos.Count());

        foreach (var item in listado)
        {
            this.DatosCatalogoProductos.Add(item);
        }
        
        this.IsLazyLoading = false;
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
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