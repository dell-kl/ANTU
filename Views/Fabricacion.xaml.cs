using ANTU.ViewModel;

namespace ANTU.Views;

public partial class Fabricacion : ContentPage
{
	public Fabricacion(FabricacionViewModel fabricacionViewModel)
	{
		InitializeComponent();

		BindingContext = fabricacionViewModel;
	}
}