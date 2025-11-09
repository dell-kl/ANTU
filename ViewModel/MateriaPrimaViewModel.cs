using ANTU.Models;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ANTU.ViewModel
{
    public partial class MateriaPrimaViewModel : ParentViewModel
    {
        private ObservableCollection<MateriaPrimaProducto> modeloMateriaPrima = new ObservableCollection<MateriaPrimaProducto>();
        public ObservableCollection<MateriaPrimaProducto> materiaPrimaProductos { set { SetProperty(ref modeloMateriaPrima, value); } get => modeloMateriaPrima; }

        private ObservableCollection<object> datosPresentacion = new ObservableCollection<object>();
        public ObservableCollection<object> DatosPresentacion { set => SetProperty(ref datosPresentacion, value); get => datosPresentacion; }

        
        private bool buttonUpdateMateriaPrima = true;
        public bool ButtonUpdateMateriaPrima { set => SetProperty(ref buttonUpdateMateriaPrima, value); get => buttonUpdateMateriaPrima; }

        private bool isLazyLoading = false;
        public bool IsLazyLoading { set => SetProperty(ref isLazyLoading, value); get => isLazyLoading; }

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
            }
        }   

        [RelayCommand(AllowConcurrentExecutions = false)]
        public override async Task NavegarFormulario(string objeto)
        {
            if (this.DataQuery.Equals("MateriaPrima"))
                await base.NavegarFormulario("FormularioMateriaPrima");
            else if (this.DataQuery.Equals("Catalogo"))
                await base.NavegarFormulario("CatalogoProductoFormulario");
        }


        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task NavegarPaginaProductoGestionar(string guid)
        {
            string paginaShell;
            object registro;

            if ( DataQuery.Equals("Catalogo"))
            {
                paginaShell = "CatalogoProductoDetalle";
                registro = this.DatosPresentacion.Where(item => (item as CatalogoProducto)!.guid.Equals(guid)).First();
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
