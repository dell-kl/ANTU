using ANTU.ViewModel;

namespace ANTU.Views;

public partial class ProductosListos : ContentPage
{
	public ProductosListos(ProductosListosViewModel productosListosViewModel)
	{
		InitializeComponent();

		BindingContext = productosListosViewModel;
	}
}