using ANTU.Resources.Components.PopupComponents;
using ANTU.ViewModel;
using Mopups.Services;
using Syncfusion.Maui.DataForm;

namespace ANTU.Views.Detalles;

public partial class MateriaPrimaDetalle : ContentPage
{
	public MateriaPrimaDetalle(MateriaPrimaDetalleViewModel materiaPrimaDetalleViewModel)
	{
		InitializeComponent();

		BindingContext = materiaPrimaDetalleViewModel;
        MateriaPrimaSeguimiento.SearchController.AllowFiltering = true;
    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        TituloMateriaPrima.Text = (BindingContext as MateriaPrimaDetalleViewModel)!.MateriaPrimaProducto.nombreProducto;
        await (BindingContext as MateriaPrimaDetalleViewModel)!.cargarDatosMateriaPrimaDetalle();
        await (BindingContext as MateriaPrimaDetalleViewModel)!.DesmontarSpinner();
        await Task.Delay(4000);
        ShimmerKgTotal.IsActive = false;
        ShimmerPrecioPromedio.IsActive = false;
        ShimmerTotalCompra.IsActive = false;
        ShimmerUltimaCompra.IsActive = false;
    }

    private void SearchMateriaPrimaSeguimiento_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar? searchBar = sender as SearchBar;

        if (searchBar.Text.Any())
            MateriaPrimaSeguimiento.SearchController.Search(searchBar.Text);
        else
            MateriaPrimaSeguimiento.SearchController.ClearSearch();

    }

   

}