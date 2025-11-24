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
        CatalogoProductoFormularioComponentes formularioComponente = new CatalogoProductoFormularioComponentes();
        formularioComponente.BindingContext = this.catalogoProductoViewModel;
        StackLayoutCatalogoProductoFormulario.Add(formularioComponente);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await this.catalogoProductoViewModel.DesmontarSpinner();
    }
}
