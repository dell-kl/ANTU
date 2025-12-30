using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANTU.ViewModel;

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

        await this.productoListoDetalleViewModel.DesmontarSpinner();
    }
    
    protected override bool OnBackButtonPressed()
    {
        return this.productoListoDetalleViewModel.ControlarNavegacion();
    }
}