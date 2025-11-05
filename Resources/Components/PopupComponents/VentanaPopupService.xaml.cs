using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui.Views;

namespace ANTU.Resources.Components.PopupComponents;

public partial class VentanaPopupService : Popup
{
	public VentanaPopupService(VentanaPopupServiceViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

}