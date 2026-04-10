using Modelos;
using Modelos.Dto;
using ANTU.Resources.Components.PopupComponents;
using Data.Rest.RestInterfaces;
using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net;
using System.Runtime.Versioning;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;
using Modelos.RequestDto;
using Modelos.ResultDto;    

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class MateriaPrimaDetalleViewModel : ParentViewModel
    {
        [ObservableProperty] private MateriaPrimaProducto _materiaPrimaProducto;
        //el formato para mostrar datos.
        [ObservableProperty] private MateriaPrimaDetalle? _materiaPrimaDetalle = null;
        //formulario para agregar mas stock
        [ObservableProperty] private MateriaPrimaDetalleFormulario _materiaPrimaDetalleFormulario;
        //donde establecemos los formularios para poder mostrarlos.
        [ObservableProperty] private FormularioEmergente _formulario;
        //formulario para editar el nombre de la materia prima
        [ObservableProperty] private MateriaPrimaEditarDataFormulario _materiaPrimaEditarDataFormulario;
        [ObservableProperty] private ObservableCollection<KgSeguimiento> _kgSeguimientoList;

        /// <summary> 
        ///Esta propiedad de aqui esta relacionado con la parte de SfDataGrid, es para poder obtener el item seleccionado
        ///y realiazar alguna accion especifica.
        /// Revisar: EditarCompraRegistradaStock (Command) y EliminarCompraRegistradaStock (Command)
        /// XAML relacionado: Views/Detalles/MateriaPrimaDetalle.xaml:503-519 (acciones swipe que disparan los comandos)
        /// 
        /// </summary>
        [ObservableProperty] private KgSeguimiento _kgSeguimientoSeleccionado;
        
        public MateriaPrimaDetalleViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) : base(restManagement, popupService, managementService, mensaje)
        {
            this.MateriaPrimaProducto = new MateriaPrimaProducto(0);
            this.MateriaPrimaDetalleFormulario = new MateriaPrimaDetalleFormulario();
            this.Formulario = new FormularioEmergente();
            this.MateriaPrimaEditarDataFormulario = new MateriaPrimaEditarDataFormulario();
            this.KgSeguimientoList = new ObservableCollection<KgSeguimiento>();
        }


        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);

            if ( base.DataQuery is not null )
                this.MateriaPrimaProducto = (base.DataQuery as MateriaPrimaProducto)!;
        }

        /// <summary>
        ///  Este es el primer metodo que se ejecutara cuando entremos a los
        /// detalles de la materia prima que seleccionamos.
        ///
        ///
        /// HttpStatusCode.RequestTimeout : Cuando la conexion se perdio mandamos un mensaje de que se ha perdido la conexion.
        /// HttpStatusCode.InternalServerError: Un error en el procesamiento de los datos que se logro obtener.
        /// HttpStatusCode.OK: Todo se proceso exitosamente.
        /// </summary>
        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task ObtenerDatosMateriaPrimaDetalle(object? estado=null)
        {
            RequestResultDto<(MateriaPrimaDetalle, IEnumerable<KgSeguimiento>)> resultado = await ManagementService.materiaPrimaService
                .ObtenerDatosMateriaPrimaDetalle(this.MateriaPrimaProducto.guid);

            if (!resultado.Success)
            {
                string mensajeSinConexion = "";
                foreach (var mensaje in resultado.Errors)
                    mensajeSinConexion += $"{mensaje.Message}\n";
                
                if (resultado.HttpStatusCode == HttpStatusCode.RequestTimeout)
                    await Mensaje.MostrarAlertaSinConexion(mensajeSinConexion);
                else if (resultado.HttpStatusCode == HttpStatusCode.InternalServerError)
                    await Mensaje.MostrarAlertaServidor(mensajeSinConexion);
            }
            else
            {
                MateriaPrimaDetalle = resultado.Value.Item1;
                foreach(var item in resultado.Value.Item2)
                    KgSeguimientoList.Add(item);
            }
            
            await this.EliminarSpinnerDirectamente();
        }
        
        /// <summary>
        /// Este metodo nos permite obtener todos los datos de compra que realiza el usuario sobre la materia prima
        /// que selecciono. Esto es un historial de compras. Los datos se van trayendo de 10 en 10, para no
        /// colapsar el aplicativo movil. 
        /// </summary>
        public async Task cargarDatosKgSeguimiento()
        {
            this.MateriaPrimaDetalle!.NValoresListadoKgSeguimiento = KgSeguimientoList.Count;
            RequestResultDto<IEnumerable<KgSeguimiento>> resultado = await ManagementService.materiaPrimaService.GetKgSeguimientoMateriaPrimaDetalle(MateriaPrimaDetalle!);

            if (resultado.Success)
                foreach (var item in resultado.Value)
                    KgSeguimientoList.Add(item);
            else
                await Mensaje.MensajeError("Error", "No se pudieron traer los registros");
        }


        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task MostrarDetallesImagenes()
        {
            var datosNavegacion = new ShellNavigationQueryParameters {
                {
                    "DataQuery", new List<object>()
                    {
                        this.MateriaPrimaDetalle!,
                        this.MateriaPrimaProducto.guid
                    }
                }
            };
            await base.NavegarFormulario("MostrarImagenesDetalle", datosNavegacion);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task VentanaFormularioEditarDatosMateriaPrima()
        {

            var datosNavegacion = new ShellNavigationQueryParameters
            {
                {
                    "DataQuery", new List<object>() { "MateriaPrimaFormulario", "editar_materiaPrima", this.MateriaPrimaProducto}
                }
            };

            IPopupResult<Object> resultado = await PopupService.ShowPopupAsync<VentanaPopupServiceViewModel, Object>(Shell.Current, shellParameters: datosNavegacion, options: PopupOptions.Empty);

            if (resultado.Result is List<object> datos)
            {
                await base.MostrarSpinner();

                MateriaPrimaFormulario formulario = (MateriaPrimaFormulario)datos[0];
                RequestResultDto<bool> respuesta = await ManagementService.materiaPrimaService.EditarDatosMateriaPrima(new MateriaPrimaRequestDto()
                {
                    id_dto = datos[1].ToString()!,
                    nombre_dto = (datos[0] as MateriaPrimaFormulario)!.MateriaPrima
                });

                if (respuesta.Success)
                {
                    await Mensaje.MensajeCorrecto("Editar Materia Prima", "Se han editado los datos de la materia prima");
                    this.MateriaPrimaProducto.nombreProducto = formulario.MateriaPrima;
                }
                else
                {
                    string mensajeError = "";
                    foreach (var item in respuesta.Errors)
                        mensajeError += $"{item.Message}\n";   
                    
                    if(respuesta.HttpStatusCode is HttpStatusCode.RequestTimeout)
                        await Mensaje.MensajeError("Fuera de red/sin conexion", mensajeError);
                    else if(respuesta.HttpStatusCode is HttpStatusCode.InternalServerError)
                        await Mensaje.MensajeError("Error procesar datos", mensajeError);
                }
           
                await base.EliminarSpinnerDirectamente();
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task VentanaFormularioMateriaPrimaDetalleStock(object data)
        {
            var datosNavegacion = new ShellNavigationQueryParameters
            {
                {
                    "DataQuery", new List<object>() { "MateriaPrimaFormulario", "agregar_compra", this.MateriaPrimaProducto}
                }
            };

            IPopupResult<Object> resultado = await PopupService.ShowPopupAsync<VentanaPopupServiceViewModel, Object>(Shell.Current, shellParameters: datosNavegacion, options: PopupOptions.Empty);

            if (resultado.Result is List<object> datos)
            {
                await base.MostrarSpinner();
                MateriaPrimaFormulario formulario = (MateriaPrimaFormulario)datos[0];
                RequestResultDto<KgSeguimiento> resultadoServidor = await ManagementService.materiaPrimaService.AgregarStockMateriaPrima(formulario, datos[1].ToString()!);

                if (resultadoServidor.Success)
                {
                    this.MateriaPrimaDetalle!.TotalCompras += 1;
                    this.MateriaPrimaDetalle!.UltimaCompra = (decimal)(formulario.Cantidad * formulario.Precio);
                    this.MateriaPrimaDetalle!.KgTotal += (formulario.Cantidad * formulario.KgStandard);
                    await Mensaje.MensajeCorrecto("Agregar a Bodega", "Se he agregado mas material a la bodega");
                }
                else if (!resultadoServidor.Success)
                {
                    string mensajeError = "";
                    foreach (var item in resultadoServidor.Errors)
                        mensajeError += $"{item.Message}\n";    
                    
                    if(resultadoServidor.HttpStatusCode is HttpStatusCode.RequestTimeout)
                        await Mensaje.MensajeError("Fuera de red/sin conexion", mensajeError);
                    else if(resultadoServidor.HttpStatusCode is HttpStatusCode.InternalServerError)
                        await Mensaje.MensajeError("Error procesar datos", mensajeError);
                }
                
                await EliminarSpinnerDirectamente();
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public Task EditarCompraRegistradaStock(object datos)
        {
            //vamos a realizar la respectiva edicion de la compra registrada.
            
            return Task.CompletedTask;
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task EliminarCompraRegistradaStock(string identificador)
        {
            //vamos a realizar la respectiva eliminacion de la compra registrada.
            await MostrarVentanaConfirmacion(
                "Proceso Eliminar",
                "Desear eliminar este registro de compra, se descontara el stock agregado por esta compra",
                "Eliminando, espere...",
                "Cancelar",
                "Continuar", async () =>
                {
                    await Task.Delay(4000);

                    await Mensaje.MensajeCorrecto("Eliminacion registro", "Se elimino el registro de compra exitosamente");

                    await EliminarSpinnerDirectamente();
                });
        }
    }
}
