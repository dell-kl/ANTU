using ANTU.ViewModel;
using System.Threading.Tasks;

namespace ANTU.Views.Formularios;

public partial class MateriaPrimaFormulario : ContentPage
{
	public MateriaPrimaFormulario(FormularioMateriaPrimaViewModel formularioMPVM)
	{
		InitializeComponent();

		BindingContext = formularioMPVM;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await (BindingContext as FormularioMateriaPrimaViewModel)!.DesmontarSpinner();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
		Button button = (Button)sender;
		(BindingContext as FormularioMateriaPrimaViewModel)!.EliminarArchivo(button.ClassId);
    }

}