namespace ANTU;

using ANTU.Resources.Messenger;
using CommunityToolkit.Mvvm.Messaging;


public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        //registrar un respectivo messenger para interactuar con nuestro Shell
        WeakReferenceMessenger.Default.Register<AppShellMessenger>(this, (r, m) =>
        {
            var datos = m.Value;

            ContentRepresentation.IsVisible = datos["CRepresentation"];
            BarraOpciones.IsVisible = datos["TabBar"];
        });
    }
}