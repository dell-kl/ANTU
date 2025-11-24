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

}