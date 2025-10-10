using ANTU.Models;
using ANTU.ViewModel;
using System.Threading.Tasks;

namespace ANTU.Views;

public partial class MateriaPrima : ContentPage
{
	public MateriaPrima(MateriaPrimaViewModel materiaPrimaViewModel)
	{
		InitializeComponent();
        BindingContext = materiaPrimaViewModel;
    

    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!(BindingContext as MateriaPrimaViewModel)!.materiaPrimaProductos.Any())
        {
            await(BindingContext as MateriaPrimaViewModel)!.cargaProductos();
            await(BindingContext as MateriaPrimaViewModel)!.DesmontarSpinner();
            await Task.Delay(4000);
            shimmerListView.IsActive = false;
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchBar = (sender as SearchBar);

        if ( ListadoMateriaPrima.DataSource is not null)
		{
			ListadoMateriaPrima.DataSource!.Filter = (object obj) =>
			{
                if (searchBar == null || searchBar.Text == null)
                    return true;

                var taskInfo = obj as MateriaPrimaProducto;
                if (taskInfo!.nombreProducto.ToLower().Contains(searchBar.Text.ToLower()))
                    return true;
                else
                    return false;
            };
			ListadoMateriaPrima.DataSource!.RefreshFilter();
		}

	}

    private void BotonEditar_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {

        Border elemento = (sender as Border)!;

        var enlace = (BindingContext as MateriaPrimaViewModel)!;
        bool estado = enlace.NavegarPaginaProductoGestionarCommand.CanExecute(elemento.ClassId);
        if (estado)
        {   
            enlace.NavegarPaginaProductoGestionarCommand.Execute(elemento.ClassId);
        }

    }

}