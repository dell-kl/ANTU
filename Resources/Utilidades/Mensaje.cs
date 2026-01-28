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
            this._ventanaEmergente.FindByName<Label>("Titulo").Text = mensaje;
            this._ventanaEmergente.FindByName<Label>("Contenido").Text = mensaje;
            this._ventanaEmergente.FindByName<ActivityIndicator>("LoadingElement").IsVisible = false;
            this._ventanaEmergente.FindByName<Image>("ImagenIcon").Source = "successful.png";
            this._ventanaEmergente.FindByName<Image>("ImagenIcon").IsVisible = true;
            this._ventanaEmergente.FindByName<Button>("Button").IsVisible = true;
            this._ventanaEmergente.FindByName<Button>("Button").IsEnabled = true;
            
            await MopupService.Instance.PushAsync(this._ventanaEmergente);
        }

        public async Task MensajeError(string mensaje, string cuerpo) 
        {
            this._ventanaEmergente.FindByName<Label>("Titulo").Text = mensaje;
            this._ventanaEmergente.FindByName<Label>("Contenido").Text = mensaje;
            this._ventanaEmergente.FindByName<ActivityIndicator>("LoadingElement").IsVisible = false;
            this._ventanaEmergente.FindByName<Image>("ImagenIcon").Source = "alert.png";
            this._ventanaEmergente.FindByName<Image>("ImagenIcon").IsVisible = true;
            this._ventanaEmergente.FindByName<Button>("Button").IsVisible = true;
            this._ventanaEmergente.FindByName<Button>("Button").IsEnabled = true;
            
            await MopupService.Instance.PushAsync(this._ventanaEmergente);
        }

        public async Task MostrarAlertaSinConexion(string mensaje)
        {
            this._ventaEmergenteSinConexion.Descripcion = mensaje;
            await MopupService.Instance.PushAsync(this._ventaEmergenteSinConexion);
        }
    }
}
