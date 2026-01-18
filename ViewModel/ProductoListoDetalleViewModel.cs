using System.Collections.ObjectModel;
using Modelos;
using Modelos.Dto;
using Modelos.RequestDto;
using ANTU.Resources.Components.FormularioComponentes;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
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
    
    public ProductoListoDetalleViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService) : base(restManagement, popupService, managementService) {
        
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
        
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task RefrescarTablaProductosListos()
    {
        await base.MostrarSpinner();
        await agregarMasDatosVenta(true);
        await base.DesmontarSpinner();
    }
    
   public async Task agregarMasDatosVenta(bool permitir = false)
    {
        if (this.ListadoProductosListos.Count == 0 || this.ListadoProductosListos.Count >= 10 || permitir)
        {
            var resultado = await _restManagement.ProduccionLista.ObtenerInformacionProductosBodega(this.Produccion.Identificador, this.ListadoProductosListos.Count());

            if (resultado.Any())
                this.ListadoProductosListos = this.ListadoProductosListos.Union(resultado).ToObservableCollection();
        }
    }
}