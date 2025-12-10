using ANTU.ViewModel;

namespace ANTU.Views.Formularios;

public partial class FabricacionFormulario : ContentPage
{
    private FabricacionFormularioViewModel _fabricacionFormularioViewModel;

    public FabricacionFormulario(FabricacionFormularioViewModel fabricacionFormularioViewModel)
	{
		InitializeComponent();
        _fabricacionFormularioViewModel = fabricacionFormularioViewModel;
        this.BindingContext = _fabricacionFormularioViewModel;

        EntradaFormularioProduccion.Add(this._fabricacionFormularioViewModel.FabricacionFormularioComponentes);
    }

    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await this._fabricacionFormularioViewModel.DesmontarSpinner();
    }

    protected override bool OnBackButtonPressed()
    {
        return this._fabricacionFormularioViewModel.ControlarNavegacion();
    }

}