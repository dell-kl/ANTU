using Mopups.Services;

namespace ANTU.Resources.Components.PopupComponents;

public partial class VentanaConfirmacionEmergente
{
	public VentanaConfirmacionEmergente()	
	{
		InitializeComponent();
	}

    private async void BotonRegresar_Clicked(object sender, EventArgs e)
    {
		bool respuesta = MopupService.Instance.PopupStack.Where(item => item is VentanaConfirmacionEmergente).Any();

		if (respuesta) {
            //MopupService.Instance.PopAsync();
            await MopupService.Instance.PopAsync();
        }
    }
}