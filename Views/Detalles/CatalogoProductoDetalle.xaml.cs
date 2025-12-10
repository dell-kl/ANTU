using ANTU.ViewModel;
using System.Threading.Tasks;

namespace ANTU.Views.Detalles;

public partial class CatalogoProductoDetalle : ContentPage
{
	private CatalogoProductoDetalleViewModel ViewModel { set; get; }

    public CatalogoProductoDetalle(CatalogoProductoDetalleViewModel catalogoProductoViewModel)
	{
		InitializeComponent();
        this.ViewModel = catalogoProductoViewModel;
        BindingContext = this.ViewModel;
	}

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if(this.ViewModel.PermitirCargaDatos)
        {
            await this.ViewModel.cargarDatos();
            await this.ViewModel.cargarDatosDetalle();
            await this.ViewModel.DesmontarSpinner();
            ShimmerKgTotal.IsActive = false;
            ShimmerPrecioPromedio.IsActive = false;
            ShimmerTotalCompra.IsActive = false;
            ShimmerUltimaCompra.IsActive = false;
            this.ViewModel.PermitirCargaDatos = false;
        }

        if(!this.ViewModel.CatalogoProductoDetalle.Imagenes.Any())
            this.ViewModel.CatalogoProducto.RutaImagen = "default_icon.png";
        else if (
            (
            this.ViewModel.CatalogoProducto!.RutaImagen is "default_icon.png" || 
            !this.ViewModel.CatalogoProductoDetalle.Imagenes.Where(item => item.Url == this.ViewModel.CatalogoProducto.RutaImagen).Any()
            ) 
            && 
            this.ViewModel.CatalogoProductoDetalle.Imagenes.Any()
        )
            this.ViewModel.CatalogoProducto.RutaImagen = this.ViewModel.CatalogoProductoDetalle.Imagenes.First().Url;
    }

    private async void PaginationCatalogProduct_PageChanging(object sender, Syncfusion.Maui.DataGrid.DataPager.PageChangingEventArgs e)
    {
        if (e.NewPageIndex is 1)
        {
            await this.ViewModel.cargarDatos();
            PaginationCatalogProduct.MoveToNextPage();
        }
    }


    protected override bool OnBackButtonPressed()
    {
        return this.ViewModel.ControlarNavegacion();
    }
}