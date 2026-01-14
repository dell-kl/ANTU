using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Components.PopupComponents;
using ANTU.Resources.Rest.RestInterfaces;
using ANTU.Resources.ValueConverter;
using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using Syncfusion.Maui.DataForm;
using System.Collections.ObjectModel;



namespace ANTU.ViewModel
{
    public partial class MateriaPrimaDetalleViewModel : ParentViewModel
    {
        [ObservableProperty]
        private MateriaPrimaProducto materiaPrimaProducto = new MateriaPrimaProducto(0);


        //el formato para mostrar datos.
        [ObservableProperty]
        private MateriaPrimaDetalle? materiaPrimaDetalle = null;

        //formulario para agregar mas stock
        [ObservableProperty]
        private MateriaPrimaDetalleFormulario materiaPrimaDetalleFormulario = new MateriaPrimaDetalleFormulario();

        //donde establecemos los formularios para poder mostrarlos.
        [ObservableProperty]
        private FormularioEmergente formulario = new FormularioEmergente();

        //formulario para editar el nombre de la materia prima
        [ObservableProperty]
        private MateriaPrimaEditarDataFormulario materiaPrimaEditarDataFormulario = new MateriaPrimaEditarDataFormulario();
        
        [ObservableProperty]
        private ObservableCollection<KgSeguimiento> _kgSeguimientoList = new ObservableCollection<KgSeguimiento>();

        public MateriaPrimaDetalleViewModel(IRestManagement restManagement, IPopupService popupService) : base(restManagement, popupService)
        {
        }


        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);

            if ( base.DataQuery is not null )
                this.MateriaPrimaProducto = (base.DataQuery as MateriaPrimaProducto)!;
        }

        public async Task cargarDatosMateriaPrimaDetalle()
        {
            this.MateriaPrimaDetalle = await _restManagement.MateriaPrima.MateriaPrimaDetalles(this.MateriaPrimaProducto.guid);
        }

        public async Task cargarDatosKgSeguimiento()
        {
            if ( !this.KgSeguimientoList.Any() || this.KgSeguimientoList.Count() >= 10 )
            {
                IEnumerable<KgSeguimiento> listadokgSeguimientos = await _restManagement.MateriaPrima.GetKgSeguimientos(this.KgSeguimientoList.Count(), this.MateriaPrimaProducto.guid);

                if (listadokgSeguimientos.Any()) {
                    this.KgSeguimientoList = this.KgSeguimientoList.Union(listadokgSeguimientos).ToObservableCollection();
                }
            }

        }


        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task MostrarDetallesImagenes()
        {
            
            var datosNavegacion = new ShellNavigationQueryParameters {
                {
                    "DataQuery", new List<object>()
                    {
                        this.materiaPrimaDetalle!,
                        this.materiaPrimaProducto.guid
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

            IPopupResult<Object> resultado = await _popupService.ShowPopupAsync<VentanaPopupServiceViewModel, Object>(Shell.Current, shellParameters: datosNavegacion, options: PopupOptions.Empty);

            if (resultado.Result is List<object> datos)
            {
                await base.MostrarSpinner();

                MateriaPrimaFormulario formulario = (MateriaPrimaFormulario)datos[0];

                bool solicitud = await _restManagement.MateriaPrima.EditarDatosMateriaPrima(new Models.RequestDto.MateriaPrimaRequestDto()
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

            IPopupResult<Object> resultado = await _popupService.ShowPopupAsync<VentanaPopupServiceViewModel, Object>(Shell.Current, shellParameters: datosNavegacion, options: PopupOptions.Empty);

            if (resultado.Result is List<object> datos)
            {
                await base.MostrarSpinner();

                MateriaPrimaFormulario formulario = (MateriaPrimaFormulario)datos[0];

                bool solicitud = await _restManagement.MateriaPrima.AgregarStockMateriaPrima(new Models.RequestDto.StockMateriaPrimaRequestDto()
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
