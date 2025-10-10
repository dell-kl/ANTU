using ANTU.ViewModel;

namespace ANTU.Views;

public partial class Catalogo : ContentPage
{
	public Catalogo(CatalogoViewModel catalogoViewModel)
	{
		InitializeComponent();

		BindingContext = catalogoViewModel;
	}
}