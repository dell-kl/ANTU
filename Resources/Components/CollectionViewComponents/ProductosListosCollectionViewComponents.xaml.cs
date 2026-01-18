using ANTU.ViewModel.ComponentsViewModel;

namespace ANTU.Resources.Components.CollectionViewComponents;

public partial class ProductosListosCollectionViewComponents : ContentView
{
    private ProductosListosCollectionViewComponentsViewModel itemViewModel;
    
    public ProductosListosCollectionViewComponents(ProductosListosCollectionViewComponentsViewModel itemViewModel)
    {
        InitializeComponent();
        this.itemViewModel = itemViewModel;
        BindingContext = this.itemViewModel;
        // Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        await this.itemViewModel.CargarDatosProductosListos();
    }

}