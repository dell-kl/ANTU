using ANTU.ViewModel;

namespace ANTU.Views;

public partial class Catalogo : ContentPage
{
	public Catalogo(CatalogoViewModel catalogoViewModel)
	{
		InitializeComponent();

		BindingContext = catalogoViewModel;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await(BindingContext as CatalogoViewModel)!.DesmontarSpinner();
    }
}