using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Rest;
using ANTU.Resources.Rest.RestInterfaces;
using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ANTU.ViewModel
{
    public partial class CatalogoProductoDetalleViewModel : ParentViewModel
    {

        [ObservableProperty]
        private bool permitirCargaDatos = true;

        //datos que compartimos con nuestro producto que seleccionamos en la pagina o pantalla anterior.
        [ObservableProperty]
        private CatalogoProducto catalogoProducto = new CatalogoProducto();

        //El listado de todas las ventas que se van a realizar.
        [ObservableProperty]
        private ObservableCollection<DataCatalogProducto> _listadoDataCatalogoProductos = new ObservableCollection<DataCatalogProducto>();

        //Este es el formulario que se inyectara en un PopupService, para poder editar datos del producto o seguir llenando mas datos de venta.
        [ObservableProperty]
        private CatalogoProductoFormulario _catalogoProductoFormulario;

        [ObservableProperty]
        private CatalogoProductoDetalle _catalogoProductoDetalle = new CatalogoProductoDetalle();

        public CatalogoProductoDetalleViewModel(IRestManagement restManagement, IPopupService popupService) : base(restManagement, popupService)
        {
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
                IEnumerable<DataCatalogProducto> datos = await _restManagement.CatalogoProduct.GetDataCatalogProducto(this.ListadoDataCatalogoProductos.Count(), CatalogoProducto.Identificador);

                if (datos.Any())
                    ListadoDataCatalogoProductos = ListadoDataCatalogoProductos.Union(datos).ToObservableCollection();
            }
        }

        public async Task cargarDatosDetalle()
        {
            this.CatalogoProductoDetalle = await _restManagement.CatalogoProduct.GetDataCatalogProductoDetalle(CatalogoProducto.Identificador);
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

            IPopupResult<Object> resultado = await _popupService.ShowPopupAsync<VentanaPopupServiceViewModel, Object>(Shell.Current, shellParameters : datosNavegacion, options: PopupOptions.Empty);
        
            if ( resultado.Result is List<object> datos )
            {
                await base.MostrarSpinner();

                CatalogoProductoFormulario formulario = (CatalogoProductoFormulario)datos[0];

                bool respuesta = await _restManagement.CatalogoProduct.Update(new CatalogoProductoRequestDto() { 
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

            IPopupResult<Object> resultado = await _popupService.ShowPopupAsync<VentanaPopupServiceViewModel, Object>(Shell.Current, shellParameters : datosNavegacion, options: PopupOptions.Empty);

            if( resultado.Result is List<object> datos )
            {
                await base.MostrarSpinner();

                CatalogoProductoFormulario formulario = (CatalogoProductoFormulario) datos[0];
                
                bool respuesta = await _restManagement.CatalogoProduct.AddDatosVentaDataCatalogProduct(new Models.RequestDto.CatalogoProductoRequestDto()
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
