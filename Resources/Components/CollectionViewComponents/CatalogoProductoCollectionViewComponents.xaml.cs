using System.Runtime.Versioning;
using ANTU.ViewModel.ComponentsViewModel;

namespace ANTU.Resources.Components.CollectionViewComponents;

[SupportedOSPlatform("Android")]
public partial class CatalogoProductoCollectionViewComponents
{
	public CatalogoProductoCollectionViewComponents(CatalogoProductoCollectionViewComponentsViewModel itemViewModel)
	{
		InitializeComponent();
        BindingContext = itemViewModel;
        
    }
}