using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Business.Services.IServices;
using Modelos;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.ViewModel.ComponentsViewModel;

public partial class ProductosListosCollectionViewComponentsViewModel : ParentViewModel
{
    [ObservableProperty]
    private bool _checkBoxEstado;

    [ObservableProperty]
    private bool _panelVisible;
    
    [ObservableProperty]
    private ObservableCollection<Produccion> _datosProductosListos = new ObservableCollection<Produccion>();

    [ObservableProperty]
    private bool _isLazyLoading;
    
    public ProductosListosCollectionViewComponentsViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService)
    : base(restManagement, popupService, managementService)
    {
        
    }

    public Task CargarDatosProductosListos()
    {
        // cargar datos del service de productos listos.
        return Task.CompletedTask;
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task LoadMoreElements()
    {
        this.IsLazyLoading = true;
        TimeSpan.FromMilliseconds(10);
        await CargarDatosProductosListos();
        this.IsLazyLoading = false;
    }
    
    public void Acciones(string accion, object parametros)
    {
        switch (accion)
        {
            case "ELIMINAR_ITEM":
                
                string identificadorProductoListo = ( parametros is string ) ? parametros.ToString()! : string.Empty;

                Produccion? registro = this.DatosProductosListos.Where(item =>
                    item.Identificador.Equals(identificadorProductoListo))
                    .ToList()
                    .FirstOrDefault();
                
                if(registro != null)
                    this.DatosProductosListos.Remove(registro);
                
                break;
        }
    }
    
    [RelayCommand(AllowConcurrentExecutions = false)]
    [SupportedOSPlatform("Android")]
    public async Task NavegarPaginaDetalle(string guid)
    {
        Produccion? registro = this.DatosProductosListos.Where(item => item.Identificador.Equals(guid)).ToList()
            .FirstOrDefault();
        
        var datosNavegacion = new ShellNavigationQueryParameters {
            {
                "DataQuery", registro is null ? "" : registro
            }
        };
        
        await base.NavegarFormulario("ProductoListoDetalle",datosNavegacion);
    }
}