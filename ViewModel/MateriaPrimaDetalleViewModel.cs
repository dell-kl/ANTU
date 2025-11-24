using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Resources.Components.PopupComponents;
using ANTU.Resources.Rest.RestInterfaces;
using ANTU.Resources.ValueConverter;
using CommunityToolkit.Maui;
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
        private MateriaPrimaProducto materiaPrimaProducto = new MateriaPrimaProducto(0);
        public MateriaPrimaProducto MateriaPrimaProducto { set => SetProperty(ref materiaPrimaProducto, value); get => materiaPrimaProducto; }

        //el formato para mostrar datos.
        private MateriaPrimaDetalle? materiaPrimaDetalle = null;
        public MateriaPrimaDetalle? MateriaPrimaDetalle { set => SetProperty(ref materiaPrimaDetalle, value); get => materiaPrimaDetalle; }

        //formulario para agregar mas stock
        private MateriaPrimaDetalleFormulario materiaPrimaDetalleFormulario = new MateriaPrimaDetalleFormulario();
        public MateriaPrimaDetalleFormulario MateriaPrimaDetalleFormulario { set => SetProperty(ref materiaPrimaDetalleFormulario, value); get => materiaPrimaDetalleFormulario; }

        //donde establecemos los formularios para poder mostrarlos.
        private FormularioEmergente formulario = new FormularioEmergente();
        public FormularioEmergente Formulario { set => SetProperty(ref formulario, value); get => formulario; }

        //formulario para editar el nombre de la materia prima
        private MateriaPrimaEditarDataFormulario materiaPrimaEditarDataFormulario = new MateriaPrimaEditarDataFormulario();


        [ObservableProperty]
        private ObservableCollection<KgSeguimiento> _kgSeguimientoList = new ObservableCollection<KgSeguimiento>();

        public MateriaPrimaEditarDataFormulario MateriaPrimaEditarDataFormulario { set => SetProperty(ref materiaPrimaEditarDataFormulario, value); get => materiaPrimaEditarDataFormulario; }


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

            await base.EjecutarFormularioEmergente(
                "Editar Datos",
                "Ingresa un nuevo nombre que quieras actualizar en tu materia prima",
                this.MateriaPrimaEditarDataFormulario,
                (item) => {
                    this.MateriaPrimaEditarDataFormulario.NombreMateriaPrima = item["NombreMateriaPrima"].ToString()!;
                },
                async () =>
                {
                    bool resultado = await _restManagement.MateriaPrima.EditarDatosMateriaPrima(new Models.RequestDto.MateriaPrimaRequestDto()
                    {
                        id_dto = this.MateriaPrimaProducto.guid,
                        nombre_dto = this.MateriaPrimaEditarDataFormulario.NombreMateriaPrima
                    }, async () => { await base.DesmontarSpinner();  });

                    if (resultado) {
                        this.MateriaPrimaProducto.nombreProducto = this.MateriaPrimaEditarDataFormulario.NombreMateriaPrima;
                    }

                    return resultado;
                }
            ); 

        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task VentanaFormularioMateriaPrimaDetalleStock(object data)
        {
            await base.EjecutarFormularioEmergente(
                "Agregar Stock",
                "Rellena el formulario para agregar mas stock a tu materia prima",
                this.MateriaPrimaDetalleFormulario,
                (item) => {
                    this.MateriaPrimaDetalleFormulario.Cantidad = item["Cantidad"] is null ? 0 : int.Parse(item["Cantidad"].ToString()!);
                    this.MateriaPrimaDetalleFormulario.KGStandard = item["KGStandard"] is null ? 0.0d : double.Parse(item["KGStandard"].ToString()!);
                    this.MateriaPrimaDetalleFormulario.PrecioUnitario = item["PrecioUnitario"] is null ? 0.0d : double.Parse(item["PrecioUnitario"].ToString()!);
                },
                async () =>
                {
                    bool resultado = await _restManagement.MateriaPrima.AgregarStockMateriaPrima(new Models.RequestDto.StockMateriaPrimaRequestDto()
                    {
                        Identificador = this.MateriaPrimaProducto.guid,
                        Amount = this.materiaPrimaDetalleFormulario.Cantidad,
                        KgStandard = this.materiaPrimaDetalleFormulario.KGStandard,
                        PriceUnit = this.materiaPrimaDetalleFormulario.PrecioUnitario
                    }, async () => { await base.DesmontarSpinner(); });

                    if (resultado)
                    {
                        this.MateriaPrimaDetalle!.TotalCompras += 1;
                        this.MateriaPrimaDetalle!.UltimaCompra = (decimal)( this.MateriaPrimaDetalleFormulario.Cantidad * this.MateriaPrimaDetalleFormulario.PrecioUnitario );
                        this.MateriaPrimaDetalle!.KgTotal += (this.MateriaPrimaDetalleFormulario.Cantidad * this.MateriaPrimaDetalleFormulario.KGStandard);
                    }

                    return resultado;
                }
            );

        }
    }
}
