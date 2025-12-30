using ANTU.ViewModel;

namespace ANTU.Views.Formularios;

public partial class FabricacionFormulario : ContentPage
{
    private FabricacionFormularioViewModel fabricacionFormularioViewModel;

    public FabricacionFormulario(FabricacionFormularioViewModel fabricacionFormularioViewModel)
	{
		InitializeComponent();
        this.fabricacionFormularioViewModel = fabricacionFormularioViewModel;
        this.BindingContext = this.fabricacionFormularioViewModel;
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await this.fabricacionFormularioViewModel.ObtenerDatosCatalogoProducto();
        await this.fabricacionFormularioViewModel.ObtenerDatosMateriaPrimaProducto();
        this.fabricacionFormularioViewModel.FabricacionFormularioComponentes.BindingContext = this.fabricacionFormularioViewModel;
        FormularioProduccionVista.Add(this.fabricacionFormularioViewModel.FabricacionFormularioComponentes);
        await this.fabricacionFormularioViewModel.DesmontarSpinner();
    }

    protected override bool OnBackButtonPressed()
    {
        return this.fabricacionFormularioViewModel.ControlarNavegacion();
    }

}