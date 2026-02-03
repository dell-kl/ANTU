using Modelos;
using Modelos.Dto;
using ANTU.Resources.Components.PopupComponents;
using Data.Rest.RestInterfaces;
using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
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
    public partial class MateriaPrimaDetalleViewModel : ParentViewModel
    {
        [ObservableProperty] private MateriaPrimaProducto _materiaPrimaProducto;
        //el formato para mostrar datos.
        [ObservableProperty] private MateriaPrimaDetalle? _materiaPrimaDetalle;
        //formulario para agregar mas stock
        [ObservableProperty] private MateriaPrimaDetalleFormulario _materiaPrimaDetalleFormulario;
        //donde establecemos los formularios para poder mostrarlos.
        [ObservableProperty] private FormularioEmergente _formulario;
        //formulario para editar el nombre de la materia prima
        [ObservableProperty] private MateriaPrimaEditarDataFormulario _materiaPrimaEditarDataFormulario;
        [ObservableProperty] private ObservableCollection<KgSeguimiento> _kgSeguimientoList;

        public MateriaPrimaDetalleViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) : base(restManagement, popupService, managementService, mensaje)
        {
            this.MateriaPrimaDetalle = new MateriaPrimaDetalle();
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

        //Este metodo de aqui se encarga de traer datos informativos sobre la materia prima
        //Incluido con los 10 primeros registros de la tabla que muestra las compras realizadas.
        public async Task ObtenerDatosMateriaPrimaDetalle()
        {
            RequestResultDto<(MateriaPrimaDetalle, IEnumerable<KgSeguimiento>)> resultado = await ManagementService.materiaPrimaService
                .ObtenerDatosMateriaPrimaDetalle(this.MateriaPrimaProducto.guid);

            if (!resultado.Success && resultado.HttpStatusCode == HttpStatusCode.RequestTimeout)
            {
                string mensajeSinConexion = "";
                foreach (var mensaje in resultado.Errors)
                    mensajeSinConexion += $"{mensaje.Message}\n";
                await Mensaje.MostrarAlertaSinConexion(mensajeSinConexion);
            }
            else if (resultado.Success)
            {
                MateriaPrimaDetalle = resultado.Value.Item1;
                foreach(var item in resultado.Value.Item2)
                    KgSeguimientoList.Add(item);
            }
            
            await this.EliminarSpinnerDirectamente();
        }
        
        public async Task cargarDatosMateriaPrimaDetalle()
        {
            // this.MateriaPrimaDetalle = await RestManagement.MateriaPrima.MateriaPrimaDetalles(this.MateriaPrimaProducto.guid);
            throw new NotImplementedException();
        }

        public async Task cargarDatosKgSeguimiento()
        {
            // if ( !this.KgSeguimientoList.Any() || this.KgSeguimientoList.Count() >= 10 )
            // {
            //     IEnumerable<KgSeguimiento> listadokgSeguimientos = await RestManagement.MateriaPrima.GetKgSeguimientos(this.KgSeguimientoList.Count(), this.MateriaPrimaProducto.guid);
            //
            //     if (listadokgSeguimientos.Any()) {
            //         this.KgSeguimientoList = this.KgSeguimientoList.Union(listadokgSeguimientos).ToObservableCollection();
            //     }
            // }



            throw new NotImplementedException();
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
