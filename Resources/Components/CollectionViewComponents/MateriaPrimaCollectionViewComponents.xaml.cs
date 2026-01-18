using System.Runtime.Versioning;
using ANTU.ViewModel.ComponentsViewModel;

namespace ANTU.Resources.Components.CollectionViewComponents;

[SupportedOSPlatform("Android")]
public partial class MateriaPrimaCollectionViewComponents
{
	public MateriaPrimaCollectionViewComponents(MateriaPrimaCollectionViewComponentsVIewModel itemViewModel)
	{
		InitializeComponent();
		BindingContext = itemViewModel;
	}

}