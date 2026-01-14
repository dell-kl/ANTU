using CommunityToolkit.Mvvm.ComponentModel;

namespace ANTU.Models
{
    public partial class DataImage : ObservableObject
    {
        [ObservableProperty]
        private string _identificador;
        [ObservableProperty]
        private string _url;
        [ObservableProperty]
        private bool _estado;
        [ObservableProperty]
        private string _tipo;
    }
}
