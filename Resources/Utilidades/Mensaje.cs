using ANTU.Resources.Components.PopupComponents;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using Mopups.Services;

namespace ANTU.Resources.Utilidades
{
    public static class Mensaje
    {

        public static async Task MensajeCorrecto(string mensaje, string cuerpo) {
            await MopupService.Instance.PushAsync(new VentanaEmergente(
          mensaje,
          cuerpo,
          false,
          true,
          "successful.png",
          true
    ));
        }

        public static async Task MensajeError(string mensaje, string cuerpo) {
            await MopupService.Instance.PushAsync(new VentanaEmergente(
        mensaje,
        cuerpo,
        false,
        true,
        "alert.png",
    true));
        }
    }
}
