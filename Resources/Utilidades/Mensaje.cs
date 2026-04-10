using System.Runtime.Versioning;
using ANTU.Resources.Components.PopupComponents;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace ANTU.Resources.Utilidades
{
    [SupportedOSPlatform("Android")]
    public partial class Mensaje : ObservableObject
    {
        private VentaEmergenteSinConexion _ventaEmergenteSinConexion;
        private VentanaEmergente _ventanaEmergente;
        
        public Mensaje()
        {
            this._ventaEmergenteSinConexion = new VentaEmergenteSinConexion();
            this._ventanaEmergente = new VentanaEmergente();
        }

        public async Task MensajeCorrecto(string mensaje, string cuerpo)
        {
            this._ventanaEmergente.Titulo = mensaje;
            this._ventanaEmergente.Contenido = cuerpo;
            this._ventanaEmergente.ShowImage = true;
            this._ventanaEmergente.Source = "successful.png";
            this._ventanaEmergente.ShowButton = true;
            
            await MopupService.Instance.PushAsync(this._ventanaEmergente);
        }

        public async Task MensajeError(string mensaje, string cuerpo) 
        {
            this._ventanaEmergente.Titulo = mensaje;
            this._ventanaEmergente.Contenido = cuerpo;
            this._ventanaEmergente.ShowImage = true;
            this._ventanaEmergente.Source = "alert.png";
            this._ventanaEmergente.ShowButton = true;
            
            await MopupService.Instance.PushAsync(this._ventanaEmergente);
        }
        
        public async Task MensajeAdvertencia(string mensaje, string cuerpo) 
        {
            this._ventanaEmergente.Titulo = mensaje;
            this._ventanaEmergente.Contenido = cuerpo;
            this._ventanaEmergente.ShowImage = true;
            this._ventanaEmergente.Source = "warning.png";
            this._ventanaEmergente.ShowButton = true;
            
            await MopupService.Instance.PushAsync(this._ventanaEmergente);
        }

        public async Task EliminarVentaSinConexion()
        {
            var pagina = MopupService.Instance.PopupStack.Where(item => item is VentaEmergenteSinConexion).ToList().FirstOrDefault();

            if(pagina is not null)
                await MopupService.Instance.RemovePageAsync(pagina);
        }
        
        public async Task MostrarAlertaSinConexion(string mensaje, AsyncRelayCommand? command = null)
        {
            this._ventaEmergenteSinConexion.Descripcion = mensaje;
            this._ventaEmergenteSinConexion.Icono = "conexion_perdida.jpg";
            if (command is not null)
                this._ventaEmergenteSinConexion.Command = command;
            
            //vamos a tener que verificar si ya existe o todavia no dicha ventana.
            var pagina = MopupService.Instance.PopupStack.Where(item => item is VentaEmergenteSinConexion).ToList().FirstOrDefault();

            if(pagina is null)
                await MopupService.Instance.PushAsync(this._ventaEmergenteSinConexion);
        }

        public async Task MostrarAlertaServidor(string mensaje, AsyncRelayCommand? command= null)
        {
            this._ventaEmergenteSinConexion.Descripcion = mensaje;
            this._ventaEmergenteSinConexion.Icono = "error_del_servidor.png";
            
            if(command is not null)
                this._ventaEmergenteSinConexion.Command = command;
            
            var pagina = MopupService.Instance.PopupStack.Where(item => item is VentaEmergenteSinConexion).ToList().FirstOrDefault();

            if(pagina is null)
                await MopupService.Instance.PushAsync(this._ventaEmergenteSinConexion);
        }
    }
}
