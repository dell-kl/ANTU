using ANTU.ViewModel;
using Microsoft.Maui.Controls.Shapes;

namespace ANTU.Views.Login;

public partial class InicioSesion : ContentPage
{
	public InicioSesion(InicioSesionViewModel inicioSesionViewModel)
	{
		InitializeComponent();

		BindingContext = inicioSesionViewModel;

	}

}