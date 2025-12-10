using ANTU.Resources.Components.FormularioComponentes;
using ANTU.ViewModel;

namespace ANTU.Views.Formularios;

public partial class CatalogoProductoFormulario : ContentPage
{
    private CatalogoProductoFormularioViewModel catalogoProductoViewModel;
	public CatalogoProductoFormulario(CatalogoProductoFormularioViewModel catalogoProductoViewModel)
	{
		InitializeComponent();
        this.catalogoProductoViewModel = catalogoProductoViewModel;
        BindingContext = this.catalogoProductoViewModel;

        //vamos a inyectar nuestro componente de formulario.    
        StackLayoutCatalogoProductoFormulario.Add(this.catalogoProductoViewModel.CatalogoProductoFormularioComponenets);
        EntradaFormularioImagenes.Add(this.catalogoProductoViewModel.ImagenesGuardarFormularioComponentes);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await this.catalogoProductoViewModel.DesmontarSpinner();
    }

    protected override bool OnBackButtonPressed()
    {
        return this.catalogoProductoViewModel.ControlarNavegacion();
    }
}
