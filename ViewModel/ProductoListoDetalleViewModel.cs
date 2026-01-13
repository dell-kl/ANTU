using System.Collections.ObjectModel;
using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Data;

namespace ANTU.ViewModel;

public partial class ProductoListoDetalleViewModel : ParentViewModel
{
    [ObservableProperty] 
    private Produccion produccion;

    [ObservableProperty]
    private ProductosListosFormulario productosListosFormulario = new ProductosListosFormulario();
    
    [ObservableProperty]
    private ObservableCollection<ProductosListos> listadoProductosListos = new ObservableCollection<ProductosListos>();

    [ObservableProperty]
    private ObservableCollection<DataCatalogoProduccion> listadoDataCatalogoProduccions = new ObservableCollection<DataCatalogoProduccion>();
    
    [ObservableProperty]
    private ProductosListosFormularioComponentes formularioProductosListosView;
    
    public ProductoListoDetalleViewModel(IRestManagement restManagement, IPopupService popupService) : base(restManagement, popupService) {
        
    }
    
    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);
        
        if ( base.DataQuery is not null )
            Produccion = (base.DataQuery as Produccion)!;
    }

    public async Task TraerDatosDeVenta()
    {
        if (this.ListadoDataCatalogoProduccions.Count() == 0 || this.ListadoDataCatalogoProduccions.Count() >= 10)
        {
            IEnumerable<DataCatalogoProduccion> respuesta = await _restManagement.ProduccionLista.ObtenerDatosCatalogoProduccion(this.Produccion.IdentificadorCatalogoProduccion, this.ListadoDataCatalogoProduccions.Count());

            if (respuesta.Any())
                this.ListadoDataCatalogoProduccions = this.ListadoDataCatalogoProduccions.Union( respuesta ).ToObservableCollection();
        }
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task FinalizarEmpaquetadoFabricado()
    {
        await base.MostrarVentanaConfirmacion(
            "Finalizar Proceso",
            "Si terminaste de empaquetar tus productos, dale en continuar.",
            "Registrando, espere...",
            "Cancelar",
            "Continuar",
            async () =>
            {
                bool resultado = await _restManagement.ProduccionLista
                    .ActualizarEstadoAFinalizado(this.Produccion.Identificador, () => base.DesmontarSpinner());

                if (true)
                {   
                    var datosNavegacion = new ShellNavigationQueryParameters {
                        {
                            "DataFormSource", new Dictionary<string, object>()
                            {
                                { "Accion", "ELIMINAR_ITEM"},
                                { "Parametros", this.Produccion.Identificador }
                            }
                        }
                    };
                    
                    await RegresarFormulario(datosNavegacion);
                }
            }
        );

        
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task RegistrarProductosABodega()
    {
        await base.MostrarSpinner();
        
        bool resultado = await _restManagement.ProduccionLista.Add(
            new FabricadoRequestDto()
            {
                identificadorDataCatalogProduction = this.ProductosListosFormulario.DatosVenta,
                identificadorProduccion = this.Produccion.Identificador,
                cantidadCostales = this.ProductosListosFormulario.NumeroCostales
            },
            () => base.DesmontarSpinner(),
            true
        );

        if (resultado)
            this.ListadoProductosListos.Add(new ProductosListos()
            {
                NumeroCostales = this.ProductosListosFormulario.NumeroCostales,
                Descripcion = this.ProductosListosFormulario.DatosVenta
            });
    }
    
   public async Task agregarMasDatosVenta()
    {
        if (this.ListadoProductosListos.Count == 0 || this.ListadoProductosListos.Count >= 10)
        {
            var resultado = await _restManagement.ProduccionLista.ObtenerInformacionProductosBodega(this.Produccion.Identificador, this.ListadoProductosListos.Count());

            if (resultado.Any())
                this.ListadoProductosListos = this.ListadoProductosListos.Union(resultado).ToObservableCollection();
        }
    }
}