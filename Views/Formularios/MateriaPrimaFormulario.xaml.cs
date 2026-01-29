using System.Runtime.Versioning;
using ANTU.ViewModel;

namespace ANTU.Views.Formularios;

[SupportedOSPlatform("Android")]
public partial class MateriaPrimaFormulario
{
    private FormularioMateriaPrimaViewModel _formularioMateriaPrimaViewModel;

	public MateriaPrimaFormulario(FormularioMateriaPrimaViewModel formularioMateriaPrimaViewModel)
	{
		InitializeComponent();

        this._formularioMateriaPrimaViewModel = formularioMateriaPrimaViewModel;
		BindingContext = this._formularioMateriaPrimaViewModel;

        //vamos a realizar las inyecciones.
        EntradaFormularioImagenes.Add(this._formularioMateriaPrimaViewModel.ImagenesGuardarFormularioComponentes);
        EntradaFormularioMateriaPrima.Add(this._formularioMateriaPrimaViewModel.MateriaPrimaFormularioComponentes);
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await (BindingContext as FormularioMateriaPrimaViewModel)!.DesmontarSpinner();
    }

    protected override bool OnBackButtonPressed()
    {
        return this._formularioMateriaPrimaViewModel.ControlarNavegacion();
    }
}