using System.Runtime.Versioning;
using ANTU.ViewModel;
using Modelos;
using Syncfusion.Maui.DataGrid;
using Syncfusion.Maui.DataGrid.DataPager;

namespace ANTU.Views.Detalles;

[SupportedOSPlatform("Android")]
public partial class MateriaPrimaDetalle : ContentPage
{
    private MateriaPrimaDetalleViewModel _materiaPrimaDetalleViewModel;

	public MateriaPrimaDetalle(MateriaPrimaDetalleViewModel materiaPrimaDetalleViewModel)
	{
		InitializeComponent();
        this._materiaPrimaDetalleViewModel = materiaPrimaDetalleViewModel;
		BindingContext = this._materiaPrimaDetalleViewModel;
        MateriaPrimaSeguimiento.SearchController.AllowFiltering = true;
    }
    
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (this._materiaPrimaDetalleViewModel.MateriaPrimaDetalle == null )
        {
            if(this._materiaPrimaDetalleViewModel.ObtenerDatosMateriaPrimaDetalleCommand.CanExecute(false))
                await this._materiaPrimaDetalleViewModel.ObtenerDatosMateriaPrimaDetalleCommand.ExecuteAsync(false);
            ShimmerKgTotal.IsActive = false;
            ShimmerPrecioPromedio.IsActive = false;
            ShimmerTotalCompra.IsActive = false;
            ShimmerUltimaCompra.IsActive = false;
        }

        if( 
            this._materiaPrimaDetalleViewModel.MateriaPrimaDetalle != null &&
            !this._materiaPrimaDetalleViewModel.MateriaPrimaDetalle.imagenes.Any())
            this._materiaPrimaDetalleViewModel.MateriaPrimaProducto.rutaImagen = "default_icon.png";
        else if (
            this._materiaPrimaDetalleViewModel.MateriaPrimaDetalle != null &&
            (
            this._materiaPrimaDetalleViewModel.MateriaPrimaProducto!.rutaImagen is "default_icon.png" ||
            !this._materiaPrimaDetalleViewModel.MateriaPrimaDetalle!.imagenes
                .Where(item => item.Url == this._materiaPrimaDetalleViewModel.MateriaPrimaProducto!.rutaImagen).Any()
            )
            &&
            this._materiaPrimaDetalleViewModel.MateriaPrimaDetalle!.imagenes.Any()
            )
            this._materiaPrimaDetalleViewModel.MateriaPrimaProducto!.rutaImagen = this._materiaPrimaDetalleViewModel.MateriaPrimaDetalle!.imagenes.First().Url;
    }

    private void SearchMateriaPrimaSeguimiento_TextChanged(object sender, TextChangedEventArgs e)
    {
        SearchBar? searchBar = sender as SearchBar;

        if (searchBar.Text.Any())
            MateriaPrimaSeguimiento.SearchController.Search(searchBar.Text);
        else
            MateriaPrimaSeguimiento.SearchController.ClearSearch();

    }

    //Este metodo se usaba para traer los datos cuando se presionaba el boton de ir hacia la siguiente pagina,
    //pero se cambio a traer los datos cuando se esta por cambiar de pagina, para evitar que se traigan datos
    //cuando el usuario solo quiere regresar a la pagina anterior.
    // PageChanging="PaginationKgSeguimiento_PageChanging" (Atributo para XAML)
    private async void PaginationKgSeguimiento_PageChanging(object sender, Syncfusion.Maui.DataGrid.DataPager.PageChangingEventArgs e)
    {
        if ( ( e.NewPageIndex > e.OldPageIndex ) || (e.NewPageIndex is 0 && e.OldPageIndex is 0)  )
            await this._materiaPrimaDetalleViewModel.cargarDatosKgSeguimiento();
    }

    private void MateriaPrimaSeguimiento_OnSwipeStarting(object? sender, DataGridSwipeStartingEventArgs e)
    {
        this._materiaPrimaDetalleViewModel.KgSeguimientoSeleccionado = (e.RowData as KgSeguimiento)!;
    }
    
    protected override bool OnBackButtonPressed()
    {
        return this._materiaPrimaDetalleViewModel.ControlarNavegacion();
    }
    
}