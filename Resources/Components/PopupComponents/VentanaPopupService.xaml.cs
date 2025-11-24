using ANTU.Models.Dto;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui.Views;


namespace ANTU.Resources.Components.PopupComponents;

public partial class VentanaPopupService : Popup<Object>
{
	private VentanaPopupServiceViewModel ventanaPopupServiceViewModel;

	// Componentes Vista
	private CatalogoProductoFormularioComponentes formularioCatalogoProducto;

    public VentanaPopupService(VentanaPopupServiceViewModel viewModel)
	{
		InitializeComponent();
		this.ventanaPopupServiceViewModel = viewModel;
		BindingContext = this.ventanaPopupServiceViewModel;
        
        this.Opened += VentanaPopupService_Opened;
        //this.Loaded += VentanaPopupService_Loaded;
	}

    private void VentanaPopupService_Opened(object? sender, EventArgs e)
    {
		cargarTipoFormulario();
    }

    private void VentanaPopupService_Loaded(object? sender, EventArgs e)
    {
        cargarTipoFormulario();

	}

    public void cargarTipoFormulario() {
		if (this.ventanaPopupServiceViewModel.TipoFormulario == "CatalogoProductoFormulario") {

			formularioCatalogoProducto = new CatalogoProductoFormularioComponentes();
			formularioCatalogoProducto.BindingContext = this.ventanaPopupServiceViewModel;
			TipoFormulario.Add(formularioCatalogoProducto);
		}
	}

}