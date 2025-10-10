using ANTU.Models;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ANTU.ViewModel
{
    public partial class MateriaPrimaViewModel : ParentViewModel
    {

        private ObservableCollection<MateriaPrimaProducto> modeloMateriaPrima = new ObservableCollection<MateriaPrimaProducto>();
        public ObservableCollection<MateriaPrimaProducto> materiaPrimaProductos { set { SetProperty(ref modeloMateriaPrima, value); } get => modeloMateriaPrima; }

        private bool buttonAddMateriaPrima = true;
        public bool ButtonAddMateriaPrima { set => SetProperty(ref buttonAddMateriaPrima, value); get => buttonAddMateriaPrima; }

        
        private bool buttonUpdateMateriaPrima = true;
        public bool ButtonUpdateMateriaPrima { set => SetProperty(ref buttonUpdateMateriaPrima, value); get => buttonUpdateMateriaPrima; }

        private bool isLazyLoading = false;
        public bool IsLazyLoading { set => SetProperty(ref isLazyLoading, value); get => isLazyLoading; }

        public MateriaPrimaViewModel(IRestManagement restManagement)
            : base(restManagement)
        {
        }

        public async Task cargaProductos()
        {
            if ( this.materiaPrimaProductos.Count() == 0 || this.materiaPrimaProductos.Count() >= 10)
            {
                 IEnumerable<MateriaPrimaProducto> listadoMateriaPrima = await _restManagement.MateriaPrima.Get(this.materiaPrimaProductos.Count().ToString());

                if (listadoMateriaPrima.Any())
                {
                    materiaPrimaProductos = materiaPrimaProductos.Union(listadoMateriaPrima).ToObservableCollection();
                }
            }
        }   

        [RelayCommand(AllowConcurrentExecutions = false)]
        public override async Task NavegarFormulario(string objeto)
        {
            await base.NavegarFormulario(objeto);
        }


        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task NavegarPaginaProductoGestionar(string guid)
        {
            MateriaPrimaProducto materiaPrimaProducto = materiaPrimaProductos.Where(item => item.guid.Equals(guid)).First();

            var datosNavegacion = new ShellNavigationQueryParameters {
                {
                    "DataQuery", materiaPrimaProducto
                }
            };

            await base.NavegarFormulario($"MateriaPrimaDetalle", datosNavegacion);
        }

        [RelayCommand]
        public void EliminarProducto(string guid)
        {
           MateriaPrimaProducto? materiaProducto =  this.materiaPrimaProductos.Where(item => item.guid.Equals(guid)).FirstOrDefault();
        
            if ( materiaPrimaProductos is not null )
                this.materiaPrimaProductos.Remove(materiaProducto!);
            
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
