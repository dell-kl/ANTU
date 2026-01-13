using ANTU.Resources.Components.FormularioComponentes;
using ANTU.ViewModel;
using Syncfusion.Maui.DataGrid.DataPager;

namespace ANTU.Views.Detalles;

public partial class ProductoListoDetalle : ContentPage
{
    private ProductoListoDetalleViewModel productoListoDetalleViewModel;
    
    public ProductoListoDetalle(ProductoListoDetalleViewModel productoListoDetalleViewModel)
    {
        InitializeComponent();
        this.productoListoDetalleViewModel = productoListoDetalleViewModel;
        BindingContext = this.productoListoDetalleViewModel;
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        await this.productoListoDetalleViewModel.TraerDatosDeVenta();
        await this.productoListoDetalleViewModel.agregarMasDatosVenta(); //esto es para mostrar en la tabla.
        this.productoListoDetalleViewModel.FormularioProductosListosView = new ProductosListosFormularioComponentes();
        this.productoListoDetalleViewModel.FormularioProductosListosView.BindingContext = this.productoListoDetalleViewModel;
        ContenedorVistaFormulario.Add(this.productoListoDetalleViewModel.FormularioProductosListosView);
        await this.productoListoDetalleViewModel.DesmontarSpinner();
    }
    
    protected override bool OnBackButtonPressed() => this.productoListoDetalleViewModel.ControlarNavegacion();
    
    private async void PagerProduccionLista_OnPageChanging(object? sender, PageChangingEventArgs e)
    {
        if ( ( e.NewPageIndex > e.OldPageIndex ) || (e.NewPageIndex is 0 && e.OldPageIndex is 0) )
            await this.productoListoDetalleViewModel.agregarMasDatosVenta();
    }
}