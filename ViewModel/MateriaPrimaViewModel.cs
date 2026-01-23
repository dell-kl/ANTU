using System.Runtime.Versioning;
using ANTU.Resources.Components.CollectionViewComponents;
using ANTU.Resources.Components.ControlersComponents;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Business.Services.IServices;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class MateriaPrimaViewModel : ParentViewModel
    {
        [ObservableProperty]
        private bool _buttonUpdateMateriaPrima = true;
        
        // Componente CollectionView de Materia Prima
        [ObservableProperty] 
        private MateriaPrimaCollectionViewComponents? _materiaPrimaView;

        // Componente CollectionView de Catalogo Producto
        [ObservableProperty]
        private CatalogoProductoCollectionViewComponents? _catalogoProductoView;

        // Compoenete CollectionView de Fabricacion
        [ObservableProperty]
        private FabricacionCollectionViewComponents? _fabricacionView;

        // Componente CollectionView de Productos Listos
        [ObservableProperty]
        private ProductosListosCollectionViewComponents? _productosListosView;
        
        // Componente Panel de controles e informacion.
        [ObservableProperty]
        private PanelComponents _panelComponents = new PanelComponents();

        public MateriaPrimaViewModel(
            MateriaPrimaCollectionViewComponents materiaPrimaView, 
            CatalogoProductoCollectionViewComponents catalogoProductoView,
            FabricacionCollectionViewComponents fabricacionView,
            ProductosListosCollectionViewComponents productosListosView,
            IRestManagement restManagement, 
            IPopupService popupService, 
            IManagementService managementService) : base(restManagement, popupService, managementService)
        {
            this._materiaPrimaView = materiaPrimaView;
            this._catalogoProductoView = catalogoProductoView;
            this._fabricacionView = fabricacionView;
            this._productosListosView = productosListosView;
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
    }
}
