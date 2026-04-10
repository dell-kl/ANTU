using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Versioning;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Rest.RestInterfaces;
using Modelos;
using Modelos.ResultDto;
using Syncfusion.Maui.Data;

namespace ANTU.ViewModel.ComponentsViewModel;

[SupportedOSPlatform("Android")]
public partial class CatalogoProductoCollectionViewComponentsViewModel : ParentViewModel
{
    [ObservableProperty]
    private ObservableCollection<CatalogoProducto> _datosCatalogoProductos;
    
    [ObservableProperty]
    private bool _isLazyLoading;
    
    public CatalogoProductoCollectionViewComponentsViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) : base(restManagement, popupService, managementService, mensaje)
    {
        this.DatosCatalogoProductos = new ObservableCollection<CatalogoProducto>();
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task CargarDatosCatalogoProducto(object? reintento = null)
    {
        if (IsLazyLoading)
            return;

        IsLazyLoading = true;

        //Mostramos la pantalla de "Cargando..." cuando el usuario presione el boton de reintentar.
        if (reintento is bool && Boolean.TryParse(reintento?.ToString(), out var reintentarPeticion) && reintentarPeticion)
            await MostrarSpinner();
        
        RequestResultDto<IEnumerable<CatalogoProducto>> resultado = await ManagementService.CatalogoProductoService.GetCatalogoProductosAync(DatosCatalogoProductos.Count);
        
        if (resultado.Success)
        {
            if (!DatosCatalogoProductos.Any())
                DatosCatalogoProductos = resultado.Value.ToObservableCollection();
            else
                foreach (var item in resultado.Value)
                    DatosCatalogoProductos.Add(item);
            
            await Mensaje.EliminarVentaSinConexion();
        }
        else
        {
            string mensajeError = "";
            foreach (var mensaje in resultado.Errors)
                mensajeError += $"- {mensaje.Message}\n";
            
            //Pasamos como argumento este mismo metodo, porque el Command esta vinculado a un boton de la venta
            // de "sin conexion". Cuando se presione el boton de Reintentar, volvar a llamar a esta funcion.
            if (resultado.HttpStatusCode is HttpStatusCode.RequestTimeout)
                await Mensaje.MostrarAlertaSinConexion(mensajeError, command: new AsyncRelayCommand(async () => await CargarDatosCatalogoProducto(true)) );
            else if ( resultado.HttpStatusCode is HttpStatusCode.InternalServerError)
                await Mensaje.MostrarAlertaServidor(mensajeError, command: new AsyncRelayCommand(async () => await CargarDatosCatalogoProducto(true)));
        }
        
        await EliminarSpinnerDirectamente();
        IsLazyLoading = false;
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