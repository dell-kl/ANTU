using ANTU.ViewModel;


namespace ANTU.Views.Formularios;

public partial class MateriaPrimaFormulario : ContentPage
{
    private FormularioMateriaPrimaViewModel formularioMateriaPrimaViewModel;

	public MateriaPrimaFormulario(FormularioMateriaPrimaViewModel formularioMateriaPrimaViewModel)
	{
		InitializeComponent();

        this.formularioMateriaPrimaViewModel = formularioMateriaPrimaViewModel;
		BindingContext = this.formularioMateriaPrimaViewModel;

        //vamos a realizar las inyecciones.
        EntradaFormularioImagenes.Add(this.formularioMateriaPrimaViewModel.ImagenesGuardarFormularioComponentes);
        EntradaFormularioMateriaPrima.Add(this.formularioMateriaPrimaViewModel.MateriaPrimaFormularioComponentes);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await (BindingContext as FormularioMateriaPrimaViewModel)!.DesmontarSpinner();
    }

    protected override bool OnBackButtonPressed()
    {
        return this.formularioMateriaPrimaViewModel.ControlarNavegacion();
    }
}