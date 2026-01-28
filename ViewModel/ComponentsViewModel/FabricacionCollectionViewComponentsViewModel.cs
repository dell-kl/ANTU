using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;
using Modelos;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Data;

namespace ANTU.ViewModel.ComponentsViewModel;

[SupportedOSPlatform("Android")]
public partial class FabricacionCollectionViewComponentsViewModel : ParentViewModel
{
    [ObservableProperty]
    private bool _checkBoxEstado;

    [ObservableProperty]
    private bool _panelVisible;

    [ObservableProperty] private ObservableCollection<Produccion> _datosProducciones;
    
    [ObservableProperty]
    private bool _isLazyLoading;

    public FabricacionCollectionViewComponentsViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje)
    : base(restManagement, popupService, managementService, mensaje)
    {
        this.DatosProducciones = new ObservableCollection<Produccion>();
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task ObtenerDatosProduccion()
    {
        if (this.IsLazyLoading)
            return;
        
        this.IsLazyLoading = true;

        var listado = await ManagementService.FabricacionService.GetProduccionAync(this.DatosProducciones.Count());

        foreach (var item in listado)
        {
            this.DatosProducciones.Add(item);
        }
        
        this.IsLazyLoading = false;
    }

    //LLama al api para cambiar productos en produccion a ya fabricados.
    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task GenerarCambioEstadoProductosFabricacion()
    {
        var productosFabricados = this.DatosProducciones
            .Where(item => item.EstadoFabricado)
            .Select(item => new Produccion()
            {
                Identificador = item.Identificador,
                Estado = 2
            } )
            .ToList();

        if (productosFabricados.Any())
        {
            await MostrarSpinner();
            
            bool resultado =
                await RestManagement.Produccion.cambiarEstadoProduccionAFabricado(productosFabricados,
                    () => base.DesmontarSpinner());
            if (resultado)
                this.DatosProducciones = this.DatosProducciones.Where(item => !item.EstadoFabricado).ToObservableCollection();
        }
        
    }
    
    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task NavegarPaginaDetalle(string guid)
    {
        // MateriaPrimaProducto? registro = this.DatosProducciones.Where(item => item.guid.Equals(guid)).ToList()
        //     .FirstOrDefault();
        
        var datosNavegacion = new ShellNavigationQueryParameters {
            {
                "DataQuery", ""
            }
        };
        // !!!! TODAVIA NO EXISTE ESTA PAGINA, CREAR LA PAGINA ...
        await base.NavegarFormulario("FabricacionDetalle",datosNavegacion);
    }
}