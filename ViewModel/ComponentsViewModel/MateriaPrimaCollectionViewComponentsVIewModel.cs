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
public partial class MateriaPrimaCollectionViewComponentsVIewModel : ParentViewModel
{
    [ObservableProperty] 
    private ObservableCollection<MateriaPrimaProducto> _datosMateriaPrimaProductos;

    [ObservableProperty]
    private bool _isLazyLoading;
    
    public MateriaPrimaCollectionViewComponentsVIewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) : base(restManagement, popupService, managementService, mensaje)
    {
        this.DatosMateriaPrimaProductos = new ObservableCollection<MateriaPrimaProducto>();
    }

    public event Action<string> EventoReintentar; 
    
    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task CargarDatosMateriaPrimaProducto(object? reintento = null)
    {
        if (this.IsLazyLoading)
            return;

        this.IsLazyLoading = true;
        
        //Mostramos la pantalla de "Cargando..." cuando el usuario presione el boton de reintentar.
        if (reintento is bool && Boolean.TryParse(reintento?.ToString(), out var reintentarPeticion) && reintentarPeticion)
            await MostrarSpinner();
        
        RequestResultDto<IEnumerable<MateriaPrimaProducto>> listado =
            await ManagementService.materiaPrimaService.GetMateriaPrimaAync(this.DatosMateriaPrimaProductos.Count());

        if (listado.Success)
        {
            if (!DatosMateriaPrimaProductos.Any())
                DatosMateriaPrimaProductos = listado.Value.ToObservableCollection();
            else
                foreach (var item in listado.Value)
                    this.DatosMateriaPrimaProductos.Add(item);

            await Mensaje.EliminarVentaSinConexion();
        }
        else
        {
            string mensajeError = "";
            foreach (var mensaje in listado.Errors)
                mensajeError += $"- {mensaje.Message}\n";
            
            //Pasamos como argumento este mismo metodo, porque el Command esta vinculado a un boton de la venta
            // de "sin conexion". Cuando se presione el boton de Reintentar, volvar a llamar a esta funcion.
            if (listado.HttpStatusCode is HttpStatusCode.RequestTimeout)
                await Mensaje.MostrarAlertaSinConexion(mensajeError, command: new AsyncRelayCommand(async () => await CargarDatosMateriaPrimaProducto(true)) );
            else if ( listado.HttpStatusCode is HttpStatusCode.InternalServerError)
                await Mensaje.MostrarAlertaServidor(mensajeError, command: new AsyncRelayCommand(async () => await CargarDatosMateriaPrimaProducto(true)));
        }
        
        await EliminarSpinnerDirectamente();
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