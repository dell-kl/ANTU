using System.ComponentModel;
using Mopups.Services;

namespace ANTU.Resources.Components.PopupComponents;

public partial class VentanaEmergente : INotifyPropertyChanged
{
    private string _titulo;
    private string _contenido;
    private bool _loading;
    private bool _showImage;
    private string _source;
    private bool _showButton;

    public bool ShowButton
    {
        set { _showButton = value; cambios("showButton"); }
        get => _showButton;
    }

    public string Source
    {
        set
        { _source = value; cambios("source"); }
        get => _source;
    }

    public bool ShowImage
    {
        set
        { _showImage = value; cambios("showImage"); }
        get => _showImage;
    }
    
    public bool Loading
    {
        set
        { _loading = value; cambios("loading"); }
        get => _loading;
    }
    
    public string Contenido
    {
        set
        { _contenido = value; cambios("contenido"); }
        get => _contenido;
    }
    
    public string Titulo
    {
        set
        { _titulo = value; cambios("titulo"); }
        get => _titulo;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void cambios(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
	public VentanaEmergente(
        // string titulo, string contenido, bool loading, bool showImage = false, string source = "", bool showButton = false
        )
	{
		InitializeComponent();

        // Titulo.Text = titulo;
        // Contenido.Text = contenido;
        // LoadingElement.IsVisible = loading;
        // ImagenIcon.Source = source;
        // ImagenIcon.IsVisible = showImage;
        // Button.IsVisible = showButton;
        // Button.IsEnabled = showButton;

        BindingContext = this;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }


    private async void Button_Clicked(object sender, EventArgs e)
    {
        await MopupService.Instance.PopAllAsync();
    }
}