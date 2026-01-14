using ANTU.Models.Dto;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui.Views;


namespace ANTU.Resources.Components.PopupComponents;

public partial class VentanaPopupService : Popup<Object>
{
	private VentanaPopupServiceViewModel ventanaPopupServiceViewModel;

	// Componentes Vista


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
		if (this.ventanaPopupServiceViewModel.TipoFormulario == "CatalogoProductoFormulario") 
			TipoFormulario.Add(this.ventanaPopupServiceViewModel.FormularioCatalogoProducto);
		else if (this.ventanaPopupServiceViewModel.TipoFormulario == "MateriaPrimaFormulario")
			TipoFormulario.Add(this.ventanaPopupServiceViewModel.MateriaPrimaFormulario);
    }

}