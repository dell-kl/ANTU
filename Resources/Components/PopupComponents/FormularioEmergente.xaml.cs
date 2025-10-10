using Mopups.Services;

namespace ANTU.Resources.Components.PopupComponents;

public partial class FormularioEmergente
{
	public FormularioEmergente()
	{
		InitializeComponent();
	}

    private void BontonCancelarFormulario_Clicked(object sender, EventArgs e)
    {
		MopupService.Instance.PopAsync();
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}