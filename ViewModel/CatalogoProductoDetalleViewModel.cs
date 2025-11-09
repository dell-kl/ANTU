using ANTU.Models;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace ANTU.ViewModel
{
    public partial class CatalogoProductoDetalleViewModel : ParentViewModel
    {
        [ObservableProperty]
        private CatalogoProducto catalogoProducto = new CatalogoProducto();

        [ObservableProperty]
        private ObservableCollection<DataCatalogProducto> _listadoDataCatalogoProductos = new ObservableCollection<DataCatalogProducto>();

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
                IEnumerable<DataCatalogProducto> datos = await _restManagement.CatalogoProduct.GetDataCatalogProducto(0, CatalogoProducto.guid);

                if (datos.Any())
                    ListadoDataCatalogoProductos = ListadoDataCatalogoProductos.Union(datos).ToObservableCollection();
            }
        }
    }
}
