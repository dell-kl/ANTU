using CommunityToolkit.Mvvm.ComponentModel;

namespace Modelos.Dto
{
    public class InicioSesionDto : ObservableObject
    {
        private string colorText = "#C3C3C3";
        private string backgroundButton = "#8B7A6B";

        private string cedula;
        private string password;

        private string mensajeCedula = "Campo cedula obligatorio";
        private string estadoCedula = "True";

        private string mensajePassword = "Campo clave obligatorio";
        private string estadoPassword = "True";

        private string habilitarBoton = "False";

        public string Cedula { set {

            if (value is "" || value.Length < 10)
            {
                MensajeCedula = "Ingresar una cedula es obligatorio";
                EstadoCedula = "True";
            }
            else
               EstadoCedula = "False";


            this.setearParametrosButton();
            SetProperty(ref cedula, value); 

        } get { return cedula; } }
        public string Password { set {

            if (value is "" || value.Length < 5)
            {
                MensajePassword = "Ingresa tu clave de acceso";
                EstadoPassword = "True";
            }
            else
                EstadoPassword = "False";

            this.setearParametrosButton();
            SetProperty(ref password, value); 
            
        } get { return password; } }

        public string ColorText { set { SetProperty(ref colorText, value); } get => colorText; }
        public string BackgroundButon { set { SetProperty(ref backgroundButton, value); } get => backgroundButton; }
        public string HabilitarBoton { set { SetProperty(ref habilitarBoton, value); } get => habilitarBoton; }
        public string MensajeCedula { set { SetProperty(ref mensajeCedula, value); } get => mensajeCedula; }
        public string EstadoCedula { set { SetProperty(ref estadoCedula, value); } get => estadoCedula; }
        public string MensajePassword { set { SetProperty(ref mensajePassword, value); } get => mensajePassword; }
        public string EstadoPassword { set { SetProperty(ref estadoPassword, value); } get => estadoPassword; }
    
        private void setearParametrosButton()
        {
            if (!bool.Parse(EstadoCedula.ToLower()) && !bool.Parse(EstadoPassword.ToString()))
            {
                ColorText = "#FFFF";
                BackgroundButon = "#BF8924";
                HabilitarBoton = "True";
            }
            else
            {
                ColorText = "#C3C3C3";
                BackgroundButon = "#8B7A6B";
                HabilitarBoton = "False";
            }
        }
    }
}
