using ANTU.Resources.Components.PopupComponents;
using Mopups.Services;

namespace ANTU.Resources.Utilidades
{
    public class Mensaje
    {
        private VentaEmergenteSinConexion _ventaEmergenteSinConexion;
        private VentanaEmergente _ventanaEmergente;
        
        public Mensaje(VentaEmergenteSinConexion ventaEmergenteSinConexion, VentanaEmergente ventanaEmergente)
        {
            this._ventaEmergenteSinConexion = ventaEmergenteSinConexion;
            this._ventanaEmergente = ventanaEmergente;
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

        public async Task MensajeError(string mensaje, string cuerpo) {
            
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
