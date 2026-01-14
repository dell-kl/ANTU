using ANTU.Models;
using ANTU.ViewModel;

namespace ANTU.Views.Detalles;

public partial class MostrarImagenesDetalle : ContentPage
{
    private MostrarImagenesDetalleViewModel mostrarImagenesDetalleViewModel;

    public MostrarImagenesDetalle(MostrarImagenesDetalleViewModel mostrarImagenesDetalleViewModel)
	{
		InitializeComponent();

        this.mostrarImagenesDetalleViewModel = mostrarImagenesDetalleViewModel;
        BindingContext = this.mostrarImagenesDetalleViewModel;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await this.mostrarImagenesDetalleViewModel.DesmontarSpinner();
    }

    private void listView_SelectionChanging(object sender, Syncfusion.Maui.ListView.ItemSelectionChangingEventArgs e)
    {
        if (!this.mostrarImagenesDetalleViewModel.ActivarCheckBoxEliminar)
        {
            this.mostrarImagenesDetalleViewModel.ActivarCheckBoxEliminar = true;
            this.mostrarImagenesDetalleViewModel.ActivarPanelAcciones = true;
            ListadoImagenesView.SelectionGesture = Syncfusion.Maui.ListView.TouchGesture.Tap;
        }

        DataImage? dataImage = (e.AddedItems?.FirstOrDefault() as DataImage);

        if (dataImage is null)
            dataImage = (e.RemovedItems?.FirstOrDefault() as DataImage);

        this.mostrarImagenesDetalleViewModel.Data.Where(item => item.Identificador.Equals(dataImage.Identificador)).First()!.Estado = !dataImage.Estado;
    }

    private void barraHerramientas_Tapped(object sender, Syncfusion.Maui.Toolbar.ToolbarTappedEventArgs e)
    {
        MostrarImagenesDetalleViewModel mostrarIMGViewModel = (BindingContext as MostrarImagenesDetalleViewModel)!;
        string? type = e.NewToolbarItem?.Name;

        if ( type is "Cancelar" )
        {
            this.mostrarImagenesDetalleViewModel.ActivarCheckBoxEliminar = false;
            this.mostrarImagenesDetalleViewModel.ActivarPanelAcciones = false;                                                                                                                                                                              
            ListadoImagenesView.SelectionGesture = Syncfusion.Maui.ListView.TouchGesture.LongPress;
        }   

        if ( type is "Eliminar" )
        {
            //mostrar opcion u alerta, de si esta seguro que va a realizar la eliminacion.

            //ejecutar peticion.
            if (this.mostrarImagenesDetalleViewModel.SeleccionarOpcionBarraHerramientasCommand.CanExecute(""))
                this.mostrarImagenesDetalleViewModel.SeleccionarOpcionBarraHerramientasCommand.Execute("");

            return;
        }

        this.mostrarImagenesDetalleViewModel.Data.Where(item => (type is "Cancelar") ? item.Estado : !item.Estado).ToList().ForEach(item => {
            item.Estado = !item.Estado;
        });
    }

    private void ButtonEliminarImagen_Clicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        this.mostrarImagenesDetalleViewModel.EliminarArchivo(button.ClassId);
    }

    protected override bool OnBackButtonPressed()
    {
        return this.mostrarImagenesDetalleViewModel.ControlarNavegacion();
    }
}