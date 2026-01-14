using ANTU.ViewModel;

namespace ANTU.Resources.Components.CollectionViewComponents;

public partial class CatalogoProductoCollectionViewComponents : ContentView
{
	public CatalogoProductoCollectionViewComponents()
	{
		InitializeComponent();
	}

    protected override async void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is MateriaPrimaViewModel materiaPrimaViewModel)
        {
            await materiaPrimaViewModel.cargaProductos();
            await materiaPrimaViewModel.DesmontarSpinner();
        }
    }
}