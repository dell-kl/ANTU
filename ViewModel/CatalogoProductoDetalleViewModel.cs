using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using Data.Rest.RestInterfaces;
using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class CatalogoProductoDetalleViewModel : ParentViewModel
    {

        [ObservableProperty]
        private bool _permitirCargaDatos = true;

        //datos que compartimos con nuestro producto que seleccionamos en la pagina o pantalla anterior.
        [ObservableProperty] private CatalogoProducto _catalogoProducto;

        //El listado de todas las ventas que se van a realizar.
        [ObservableProperty] private ObservableCollection<DataCatalogProducto> _listadoDataCatalogoProductos;

        //Este es el formulario que se inyectara en un PopupService, para poder editar datos del producto o seguir llenando mas datos de venta.
        [ObservableProperty]
        private CatalogoProductoFormulario? _catalogoProductoFormulario;

        [ObservableProperty] private CatalogoProductoDetalle _catalogoProductoDetalle;

        public CatalogoProductoDetalleViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) : base(restManagement, popupService, managementService, mensaje)
        {
            this.CatalogoProducto = new CatalogoProducto();
            this.ListadoDataCatalogoProductos = new ObservableCollection<DataCatalogProducto>();
            this.CatalogoProductoDetalle = new CatalogoProductoDetalle();
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);
            this.CatalogoProducto = (DataQuery as CatalogoProducto)!;
        }

        public async Task cargarDatos()
        {
            if ( !this.ListadoDataCatalogoProductos.Any() || this.ListadoDataCatalogoProductos.Count() >= 10)
            {
                IEnumerable<DataCatalogProducto> datos = await RestManagement.CatalogoProduct.GetDataCatalogProducto(this.ListadoDataCatalogoProductos.Count(), CatalogoProducto.Identificador);

                if (datos.Any())
                    ListadoDataCatalogoProductos = ListadoDataCatalogoProductos.Union(datos).ToObservableCollection();
            }
        }


        public async Task cargarDatosDetalle()
        {
            this.CatalogoProductoDetalle = await RestManagement.CatalogoProduct.GetDataCatalogProductoDetalle(CatalogoProducto.Identificador);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task NavegarDetallesImagenes()
        {
            var datosNavegacion = new ShellNavigationQueryParameters {
                {
                    "DataQuery", new List<object>()
                    {
                        this.CatalogoProductoDetalle!,
                        this.CatalogoProducto.Identificador
                    }
                }
            };


            await base.NavegarFormulario("MostrarImagenesDetalle", datosNavegacion);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task FormularioEditarDatos()
        {
            var datosNavegacion = new ShellNavigationQueryParameters
            {
                {
                    "DataQuery", new List<object>() {"CatalogoProductoFormulario", "editar_catalogProducto", this.CatalogoProducto}
                }
            };

            IPopupResult<Object> resultado = await PopupService.ShowPopupAsync<VentanaPopupServiceViewModel, Object>(Shell.Current, shellParameters : datosNavegacion, options: PopupOptions.Empty);
        
            if ( resultado.Result is List<object> datos )
            {
                await base.MostrarSpinner();

                CatalogoProductoFormulario formulario = (CatalogoProductoFormulario)datos[0];

                bool respuesta = await RestManagement.CatalogoProduct.Update(new CatalogoProductoRequestDto() { 
                    identificador = (string) datos[1],
                    nombreProducto = formulario.NombreProducto
                }, () => base.DesmontarSpinner());

                if ( respuesta )
                    this.CatalogoProducto.NombreProducto = formulario.NombreProducto!;
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task FormularioAgregarDatosVenta() {

            var datosNavegacion = new ShellNavigationQueryParameters
            {
                {
                    "DataQuery", new List<object>() {"CatalogoProductoFormulario", "agregar_datosVenta", this.CatalogoProducto}
                }
            };

            IPopupResult<Object> resultado = await PopupService.ShowPopupAsync<VentanaPopupServiceViewModel, Object>(Shell.Current, shellParameters : datosNavegacion, options: PopupOptions.Empty);

            if( resultado.Result is List<object> datos )
            {
                await base.MostrarSpinner();

                CatalogoProductoFormulario formulario = (CatalogoProductoFormulario) datos[0];
                
                bool respuesta = await RestManagement.CatalogoProduct.AddDatosVentaDataCatalogProduct(new Modelos.RequestDto.CatalogoProductoRequestDto()
                {
                    identificador = (string) datos[1],
                    nombreProducto = this.CatalogoProducto.NombreProducto,
                    dataCatalogProducts = new List<DataProduct>() {
                        new DataProduct() {
                            precio = (decimal) formulario.DatosVentas.Precio,
                            pesoKg = formulario.DatosVentas.Kg,
                            cantidadTotal = formulario.DatosVentas.Cantidad
                        }
                    }
                }, () => base.DesmontarSpinner());
            }
        }
    }
}
