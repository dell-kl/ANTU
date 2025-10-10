using ANTU.Models.Dto;
using ANTU.Resources.Messenger;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;


namespace ANTU.ViewModel
{
    public partial class InicioSesionViewModel : ParentViewModel
    {
        public InicioSesionViewModel(IRestManagement IRestManagement)
        : base(IRestManagement)
        {

        }

        public InicioSesionDto inicioSesion { set; get; } = new InicioSesionDto();


        [RelayCommand]
        public async Task EnviarForm()
        {
            await Shell.Current.GoToAsync("SnipperComponent");

            string username = "1754090106";
            string password = "admin123";

            if (inicioSesion.Cedula.Equals(username) && inicioSesion.Password.Equals(password))
            {
                Dictionary<string, bool> keyValuePairs = new Dictionary<string, bool>();
                keyValuePairs.Add("CRepresentation", false);
                keyValuePairs.Add("TabBar", true);
                //ocultar nuestro ShellContent principal y habilitar la barra de opciones.
                WeakReferenceMessenger.Default.Send(new AppShellMessenger(keyValuePairs));
            }
            else
            {
                this.inicioSesion.Cedula = "";
                this.inicioSesion.Password = "";
                await Shell.Current.GoToAsync("..");
            }

        }

    }
}
