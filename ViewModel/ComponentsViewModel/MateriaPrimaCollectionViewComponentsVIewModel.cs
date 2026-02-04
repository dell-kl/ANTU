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

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task CargarDatosMateriaPrimaProducto()
    {
        if (this.IsLazyLoading)
            return;

        this.IsLazyLoading = true;

        RequestResultDto<IEnumerable<MateriaPrimaProducto>> listado =
            await ManagementService.materiaPrimaService.GetMateriaPrimaAync(this.DatosMateriaPrimaProductos.Count());
        
        if (listado.Success)
            foreach (var item in listado.Value)
                this.DatosMateriaPrimaProductos.Add(item);
        else
        {
            string mensajeError = "";
            foreach (var mensaje in listado.Errors)
                mensajeError += $"- {mensaje.Message}\n";
            
            if (listado.HttpStatusCode is HttpStatusCode.RequestTimeout)
                await Mensaje.MostrarAlertaSinConexion(mensajeError);
            else if ( listado.HttpStatusCode is HttpStatusCode.InternalServerError)
                await Mensaje.MostrarAlertaServidor(mensajeError);
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