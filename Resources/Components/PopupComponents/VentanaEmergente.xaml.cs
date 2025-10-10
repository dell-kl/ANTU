using Mopups.Services;

namespace ANTU.Resources.Components.PopupComponents;

public partial class VentanaEmergente 
{
	public VentanaEmergente(string titulo, string contenido, bool loading, bool showImage = false, string source = "", bool showButton = false)
	{
		InitializeComponent();

        Titulo.Text = titulo;
        Contenido.Text = contenido;
        LoadingElement.IsVisible = loading;
        ImagenIcon.Source = source;
        ImagenIcon.IsVisible = showImage;
        Button.IsVisible = showButton;
        Button.IsEnabled = showButton;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }


    private void Button_Clicked(object sender, EventArgs e)
    {
        MopupService.Instance.PopAsync();
    }
}