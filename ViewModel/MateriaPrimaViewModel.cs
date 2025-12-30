using ANTU.Models;
using ANTU.Resources.Components.CollectionViewComponents;
using ANTU.Resources.Components.ControlersComponents;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ANTU.ViewModel
{
    public partial class MateriaPrimaViewModel : ParentViewModel
    {
        [ObservableProperty]
        private ObservableCollection<object> datosPresentacion = new ObservableCollection<object>();

        [ObservableProperty]
        private bool buttonUpdateMateriaPrima = true;

        [ObservableProperty]
        private bool isLazyLoading = false;

        // Componente CollectionView de Materia Prima
        [ObservableProperty]
        private MateriaPrimaCollectionViewComponents? materiaPrimaView = new MateriaPrimaCollectionViewComponents();

        // Componente CollectionView de Catalogo Producto
        [ObservableProperty]
        private CatalogoProductoCollectionViewComponents? catalogoProductoView = new CatalogoProductoCollectionViewComponents();

        // Compoenete CollectionView de Fabricacion
        [ObservableProperty]
        private FabricacionCollectionViewComponents? fabricacionView = new FabricacionCollectionViewComponents();

        [ObservableProperty]
        private ProductosListosCollectionViewComponents productosListosView = new ProductosListosCollectionViewComponents();
        
        // Componente Panel de controles e informacion.
        [ObservableProperty]
        private PanelComponents panelComponents = new PanelComponents();

        public MateriaPrimaViewModel(IRestManagement restManagement, IPopupService popupService) : base(restManagement, popupService) {
        }

        public async Task cargaProductos()
        {
            if ( this.DatosPresentacion.Count() == 0 || this.DatosPresentacion.Count() >= 10)
            {
                 if (this.DataQuery.Equals("MateriaPrima"))
                 {
                     IEnumerable<MateriaPrimaProducto> listadoMateriaPrima = await _restManagement.MateriaPrima.Get(this.DatosPresentacion.Count().ToString());

                     if (listadoMateriaPrima.Any()) 
                        DatosPresentacion = DatosPresentacion.Union(listadoMateriaPrima).ToObservableCollection();
                 }
                 else if (this.DataQuery.Equals("Catalogo"))
                 {
                    IEnumerable<CatalogoProducto> listadoCatalogo = await _restManagement.CatalogoProduct.Get(this.DatosPresentacion.Count().ToString());
                    if (listadoCatalogo.Any())
                        DatosPresentacion = DatosPresentacion.Union(listadoCatalogo).ToObservableCollection();
                 }
                 else if (this.DataQuery.Equals("Fabricacion"))
                 {
                     IEnumerable<Produccion> listadoProduccion = await _restManagement.Produccion.Get(this.DatosPresentacion.Count().ToString());
                     
                    if (listadoProduccion.Any())
                        DatosPresentacion = DatosPresentacion.Union(listadoProduccion).ToObservableCollection();
                 }
                 else if (this.DataQuery.Equals("ProductosListos"))
                 {
                     IEnumerable<Produccion> listadoProduccion = await _restManagement.Fabricado.Get(this.DatosPresentacion.Count().ToString());
                     
                     if (listadoProduccion.Any())
                         DatosPresentacion = DatosPresentacion.Union(listadoProduccion).ToObservableCollection();
                 }
            }
        }   

        [RelayCommand(AllowConcurrentExecutions = false)]
        public override async Task NavegarFormulario(string objeto)
        {
            if (this.DataQuery.Equals("MateriaPrima"))
                await base.NavegarFormulario("FormularioMateriaPrima");
            else if (this.DataQuery.Equals("Catalogo"))
                await base.NavegarFormulario("CatalogoProductoFormulario");
            else if (this.DataQuery.Equals("Fabricacion"))
                await base.NavegarFormulario("FabricacionFormulario");
        }


        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task NavegarPaginaProductoGestionar(string guid)
        {
            string paginaShell;
            object registro;

            if ( DataQuery.Equals("Catalogo"))
            {
                paginaShell = "CatalogoProductoDetalle";
                registro = this.DatosPresentacion.Where(item => (item as CatalogoProducto)!.Identificador.Equals(guid)).First();
            }
            else if (DataQuery.Equals("ProductosListos"))
            {
                paginaShell = "ProductoListoDetalle";
                registro = this.DatosPresentacion
                    .Where(item => item is Produccion produccion && produccion.Identificador.Equals(guid)).First();
            }
            else
            {
                paginaShell = "MateriaPrimaDetalle";
                registro = this.DatosPresentacion.Where(item => (item as MateriaPrimaProducto)!.guid.Equals(guid)).First();
            }

            var datosNavegacion = new ShellNavigationQueryParameters {
                {
                    "DataQuery", registro
                }
            };

            await base.NavegarFormulario(paginaShell, datosNavegacion);
        }


        [RelayCommand]
        public async Task LoadMoreElements()
        {
            this.IsLazyLoading = true;
            TimeSpan.FromMilliseconds(10);
            await cargaProductos();
            this.IsLazyLoading = false;
        }
        
    }
}
