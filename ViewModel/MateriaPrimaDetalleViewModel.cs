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
        public async Task ObtenerDatosMateriaPrimaDetalle()
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

                bool solicitud = await RestManagement.MateriaPrima.EditarDatosMateriaPrima(new Modelos.RequestDto.MateriaPrimaRequestDto()
                {
                    id_dto = datos[1].ToString()!,
                    nombre_dto = formulario.MateriaPrima
                }, async () => { await base.DesmontarSpinner(); });

                if (solicitud)
                    this.MateriaPrimaProducto.nombreProducto = formulario.MateriaPrima;
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

                bool solicitud = await RestManagement.MateriaPrima.AgregarStockMateriaPrima(new Modelos.RequestDto.StockMateriaPrimaRequestDto()
                {
                    Identificador = datos[1].ToString()!,
                    Amount = formulario.Cantidad,
                    KgStandard = formulario.KgStandard,
                    PriceUnit = formulario.Precio
                }, async () => { await base.DesmontarSpinner(); });

                if (solicitud)
                {
                    this.MateriaPrimaDetalle!.TotalCompras += 1;
                    this.MateriaPrimaDetalle!.UltimaCompra = (decimal)(formulario.Cantidad * formulario.Precio);
                    this.MateriaPrimaDetalle!.KgTotal += (formulario.Cantidad * formulario.KgStandard);
                }
            }

        }
    }
}
