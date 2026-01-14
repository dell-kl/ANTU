using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANTU.ViewModel.ComponentsViewModel;
using Syncfusion.Maui.ListView;
using Syncfusion.Maui.Toolbar;

namespace ANTU.Resources.Components.CollectionViewComponents;

public partial class ProductosListosCollectionViewComponents : ContentView
{
    public ProductosListosCollectionViewComponents()
    {
        InitializeComponent();
    }
    
    protected async override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (this.BindingContext is ProductosListosCollectionViewComponentsViewModel productosListosViewmodel)
        {
            await productosListosViewmodel.cargaProductos();
            await productosListosViewmodel.DesmontarSpinner();
        }
    }
    
}