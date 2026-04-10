using Modelos;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Versioning;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;
using Modelos.ResultDto;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class MostrarImagenesDetalleViewModel : ParentViewModel
    {

        private string identificador = "";

        private int alturaDinamicaListaImagenes = 700;
        public int AlturaDinamicaListaImagenes { set => SetProperty(ref alturaDinamicaListaImagenes, value); get => alturaDinamicaListaImagenes; }

        [ObservableProperty]
        private bool _activarPanelNuevasImagenes = false;

        [ObservableProperty]
        private bool _activarCheckBoxEliminar = false;

        [ObservableProperty]
        private bool _activarPanelAcciones = false;

        [ObservableProperty]
        private ObservableCollection<DataImage> data = new ObservableCollection<DataImage>();
        
        [ObservableProperty]
        private object _dataModel;

        public MostrarImagenesDetalleViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) : base(restManagement, popupService, managementService, mensaje) {
            
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);
                
            if ( base.DataQuery is not null )
            {
                List<object> listobject = (base.DataQuery! as List<object>)!;
                this.identificador = listobject[1].ToString()!;
                this.DataModel = listobject[0]!;

                if (DataModel is MateriaPrimaDetalle materiaPrimaDetalle)
                {
                    this.Data = materiaPrimaDetalle.imagenes.Where(item => item.Url.Equals("default_icon.png")).Any() ? [] : materiaPrimaDetalle.imagenes;
                }
                else if (DataModel is CatalogoProductoDetalle catalogoProductoDetalle)
                {
                    this.Data = catalogoProductoDetalle.Imagenes.Where(item => item.Url.Equals("default_icon.png")).Any() ? [] : catalogoProductoDetalle.Imagenes;
                }
            }
        }


        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task SeleccionarOpcionBarraHerramientas()
        {
            await base.MostrarVentanaConfirmacion(
                "Proceso Eliminar",
                "Estas completamente seguro en querer eliminar las imagenes",
                "Eliminando, espere...",
                "Cancelar",
                "Continuar",
                (Func<Task>)(async () =>
                {

                    ICollection<DataImage> ListDataImages = this.Data.Where((Func<DataImage, bool>)(item => (bool)item.Estado)).ToList();
                    RequestResultDto<bool> respuestaServidor = new();

                    if (DataModel is MateriaPrimaDetalle materiaPrimaDetalle)
                        respuestaServidor = await ManagementService.materiaPrimaService.EliminarImagenesMateriaPrima(ListDataImages);

                    //ESTE ES PARA CATALOGO PRODUCTO... REALIZARLO MAS ADELANTE!!!!
                    // else if(DataModel is CatalogoProductoDetalle catalogoProductoDetalle)
                    //     resultado = await RestManagement.CatalogoProduct.DeleteImages(ListDataImages, async() => { await base.DesmontarSpinner(); });
                    //
                    
                    if (respuestaServidor.Success)
                    {
                        Data = this.Data.Where((Func<DataImage, bool>)(item => (bool)!item.Estado)).ToObservableCollection();
                        
                        if (DataModel is MateriaPrimaDetalle modelo1)
                            modelo1.imagenes = Data;
                        else if (DataModel is CatalogoProductoDetalle modelo2)
                            modelo2.Imagenes = Data;
                        
                        await Mensaje.MensajeCorrecto("Eliminar imagenes", "Las imagenes se eliminaron exitosamente");
                    }
                    else
                    {
                        string mensajeError = "";
                        foreach (var item in respuestaServidor.Errors)
                            mensajeError += $"{item.Message}\n";   
                    
                        if(respuestaServidor.HttpStatusCode is HttpStatusCode.RequestTimeout)
                            await Mensaje.MensajeError("Fuera de red/sin conexion", mensajeError);
                        else if (respuestaServidor.HttpStatusCode is HttpStatusCode.InternalServerError)
                            await Mensaje.MensajeError("Error procesar datos", mensajeError);
                        else
                            await Mensaje.MensajeError("Error procesar eliminado", mensajeError);
                    }
                    
                    ActivarPanelNuevasImagenes = false;
                    AlturaDinamicaListaImagenes = 700;
                    
                    await EliminarSpinnerDirectamente();
                })
            );
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task SeleccionarArchivos()
        {
            await base.SeleccionarArchivoMostrar();

            if (FileManyResults.Any())
            {
                ActivarPanelNuevasImagenes = true;
                AlturaDinamicaListaImagenes = 500;
            }
        }

        public override void EliminarArchivo(string codigo)
        {
            base.EliminarArchivo(codigo);

            if (!FileManyResults.Any())
            {
                ActivarPanelNuevasImagenes = false;
                AlturaDinamicaListaImagenes = 700;
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task GuardarNuevasImagenes()
        {
            if ( FileManyResults.Any() )
            {
                await base.MostrarVentanaConfirmacion(
                   "Registrar Imagenes",
                   "Deseas continuar con el registro de las nuevas imagenes?",
                   "Registrar, espere...",
                   "Cancelar",
                   "Continuar",
                   async () =>
                   {
                       RequestResultDto<object> respuestaServidor = new();
                       
                       if(DataModel is MateriaPrimaDetalle materiaPrimaDetalle)
                           respuestaServidor = await ManagementService.materiaPrimaService.RegistarImagenesMateriaPrima(FileManyResults, this.identificador);

                       // NECESITAMOS ESTE METODO PARA PROCESAR IMAGENES PARA LA PARTE DE CATALOOG PRODUCTO.!!!!
                       // else if(DataModel is CatalogoProductoDetalle catalogoProductoDetalle)
                       //      resultado = await RestManagement.CatalogoProduct.SaveImages(FileManyResults, this.identificador, true, async() => { await base.DesmontarSpinner(); });
                       //
                       
                       FileManyResults.Clear();
                       ActivarPanelNuevasImagenes = false;
                       AlturaDinamicaListaImagenes = 700;
                       
                       if (respuestaServidor.Success)
                       {
                           RequestDataImage imagenesModelo = (respuestaServidor.Value as RequestDataImage)!;
                           
                           foreach(var imagen in imagenesModelo.imagenes)
                               Data.Add(imagen);

                           if (DataModel is MateriaPrimaDetalle modelo)
                               modelo.imagenes = Data;
                           else if (DataModel is CatalogoProductoDetalle modelo2)
                               modelo2.Imagenes = Data;
                           
                           await Mensaje.MensajeCorrecto("Registrar Imagenes", "Las imagenes se registraron exitosamente");
                       }
                       else
                       {
                           string mensajeError = "";
                           foreach (var item in respuestaServidor.Errors)
                               mensajeError += $"{item.Message}\n";   
                    
                           if(respuestaServidor.HttpStatusCode is HttpStatusCode.RequestTimeout)
                               await Mensaje.MensajeError("Fuera de red/sin conexion", mensajeError);
                           else if (respuestaServidor.HttpStatusCode is HttpStatusCode.InternalServerError)
                               await Mensaje.MensajeError("Error procesar datos", mensajeError);
                           else
                               await Mensaje.MensajeError("Error procesar imagenes", mensajeError);
                       }
                       
                       await EliminarSpinnerDirectamente();
                   }
                );

            }
        }

    }
}
