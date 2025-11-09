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
        await this.ViewModel.cargarDatos();
        await this.ViewModel.DesmontarSpinner();
        ShimmerKgTotal.IsActive = false;
        ShimmerPrecioPromedio.IsActive = false;
        ShimmerTotalCompra.IsActive = false;
        ShimmerUltimaCompra.IsActive = false;
    }
}