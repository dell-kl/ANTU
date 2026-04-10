using System.Runtime.Versioning;
using ANTU.ViewModel.PopupServicesViewModel;

namespace ANTU.Resources.Components.PopupComponents;

[SupportedOSPlatform("Android")]
public partial class VentanaPopupService
{
	private VentanaPopupServiceViewModel _ventanaPopupServiceViewModel;

	// Componentes Vista
    public VentanaPopupService(VentanaPopupServiceViewModel viewModel)
	{
		InitializeComponent();
		this._ventanaPopupServiceViewModel = viewModel;
		BindingContext = this._ventanaPopupServiceViewModel;
        
        this.Opened += VentanaPopupService_Opened;
	}

    private void VentanaPopupService_Opened(object? sender, EventArgs e)
    {
		CargarTipoFormulario();
    }
	
    public void CargarTipoFormulario() {
		if (this._ventanaPopupServiceViewModel.TipoFormulario == "CatalogoProductoFormulario") 
			TipoFormulario.Add(this._ventanaPopupServiceViewModel.FormularioCatalogoProducto);
		else if (this._ventanaPopupServiceViewModel.TipoFormulario == "MateriaPrimaFormulario")
			TipoFormulario.Add(this._ventanaPopupServiceViewModel.MateriaPrimaFormulario);
    }

}