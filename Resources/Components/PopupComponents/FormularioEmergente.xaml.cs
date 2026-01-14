using Mopups.Services;

namespace ANTU.Resources.Components.PopupComponents;

public partial class FormularioEmergente
{
	public FormularioEmergente()
	{
		InitializeComponent();
	}


    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}