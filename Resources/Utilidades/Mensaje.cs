using System.Runtime.Versioning;
using ANTU.Resources.Components.PopupComponents;
using CommunityToolkit.Mvvm.ComponentModel;
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
            // this._ventanaEmergente.Loading = false;
            this._ventanaEmergente.ShowImage = true;
            this._ventanaEmergente.Source = "successful.png";
            this._ventanaEmergente.ShowButton = true;
            
            await MopupService.Instance.PushAsync(this._ventanaEmergente);
        }

        public async Task MensajeError(string mensaje, string cuerpo) 
        {
            this._ventanaEmergente.Titulo = mensaje;
            this._ventanaEmergente.Contenido = cuerpo;
            // this._ventanaEmergente.Loading = false;
            this._ventanaEmergente.ShowImage = true;
            this._ventanaEmergente.Source = "alert.png";
            this._ventanaEmergente.ShowButton = true;
            
            await MopupService.Instance.PushAsync(this._ventanaEmergente);
        }

        public async Task MostrarAlertaSinConexion(string mensaje)
        {
            this._ventaEmergenteSinConexion.Descripcion = mensaje;
            await MopupService.Instance.PushAsync(this._ventaEmergenteSinConexion);
        }
    }
}
