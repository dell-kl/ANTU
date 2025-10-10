using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace ANTU.Resources.Utilidades
{
    public static class Mensaje
    {

        public static async Task mostrarMensajeError(string mensaje)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            var snackbarOptions = new SnackbarOptions
            {
                BackgroundColor = Colors.Red,
                TextColor = Colors.Green,
                ActionButtonTextColor = Colors.Yellow,
                CornerRadius = new CornerRadius(10),
                CharacterSpacing = 0.5
            };

            string actionButtonText = "Entendido";
            TimeSpan duration = TimeSpan.FromSeconds(5);
            var snackbar = Snackbar.Make(mensaje, null, actionButtonText, duration, snackbarOptions);

            await snackbar.Show(cancellationTokenSource.Token);
        }
    }
}
