using ANTU.ViewModel;

namespace ANTU.Views.Detalles;

public partial class MateriaPrimaDetalle : ContentPage
{
    private MateriaPrimaDetalleViewModel materiaPrimaDetalleViewModel;

	public MateriaPrimaDetalle(MateriaPrimaDetalleViewModel materiaPrimaDetalleViewModel)
	{
		InitializeComponent();
        this.materiaPrimaDetalleViewModel = materiaPrimaDetalleViewModel;
		BindingContext = this.materiaPrimaDetalleViewModel;
        MateriaPrimaSeguimiento.SearchController.AllowFiltering = true;
    }


    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (this.materiaPrimaDetalleViewModel.MateriaPrimaDetalle == null )
        {
            await this.materiaPrimaDetalleViewModel.cargarDatosMateriaPrimaDetalle();
            await this.materiaPrimaDetalleViewModel.cargarDatosKgSeguimiento();
            await this.materiaPrimaDetalleViewModel.DesmontarSpinner();
            ShimmerKgTotal.IsActive = false;
            ShimmerPrecioPromedio.IsActive = false;
            ShimmerTotalCompra.IsActive = false;
            ShimmerUltimaCompra.IsActive = false;
        }

        if(!this.materiaPrimaDetalleViewModel.MateriaPrimaDetalle.imagenes.Any())
            this.materiaPrimaDetalleViewModel.MateriaPrimaProducto.rutaImagen = "default_icon.png";
        else if (
            (
            this.materiaPrimaDetalleViewModel.MateriaPrimaProducto!.rutaImagen is "default_icon.png" ||
            !this.materiaPrimaDetalleViewModel.MateriaPrimaDetalle!.imagenes
                .Where(item => item.Url == this.materiaPrimaDetalleViewModel.MateriaPrimaProducto!.rutaImagen).Any()
            )
            &&
            this.materiaPrimaDetalleViewModel.MateriaPrimaDetalle!.imagenes.Any()
            )
            this.materiaPrimaDetalleViewModel.MateriaPrimaProducto!.rutaImagen = this.materiaPrimaDetalleViewModel.MateriaPrimaDetalle!.imagenes.First().Url;
    }

    private void SearchMateriaPrimaSeguimiento_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar? searchBar = sender as SearchBar;

        if (searchBar.Text.Any())
            MateriaPrimaSeguimiento.SearchController.Search(searchBar.Text);
        else
            MateriaPrimaSeguimiento.SearchController.ClearSearch();

    }


    private async void PaginationKgSeguimiento_PageChanging(object sender, Syncfusion.Maui.DataGrid.DataPager.PageChangingEventArgs e)
    {
        if ( e.NewPageIndex is 1 )
        {
            await this.materiaPrimaDetalleViewModel.cargarDatosKgSeguimiento();

            PaginationKgSeguimiento.MoveToNextPage();

        }
    }


    protected override bool OnBackButtonPressed()
    {
        return this.materiaPrimaDetalleViewModel.ControlarNavegacion();
    }
}