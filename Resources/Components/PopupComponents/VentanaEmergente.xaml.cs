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
        set { _showButton = value; cambios("ShowButton"); }
        get => _showButton;
    }

    public string Source
    {
        set
        { _source = value; cambios("Source"); }
        get => _source;
    }

    public bool ShowImage
    {
        set
        { _showImage = value; cambios("ShowImage"); }
        get => _showImage;
    }
    
    public bool Loading
    {
        set
        { _loading = value; cambios("Loading"); }
        get => _loading;
    }
    
    public string Contenido
    {
        set
        { _contenido = value; cambios("Contenido"); }
        get => _contenido;
    }
    
    public string Titulo
    {
        set
        { _titulo = value; cambios("Titulo"); }
        get => _titulo;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public void cambios(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
    
	public VentanaEmergente()
	{
		InitializeComponent();
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