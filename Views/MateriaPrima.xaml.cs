using System.Runtime.Versioning;
using ANTU.ViewModel;
using ANTU.ViewModel.ComponentsViewModel;

namespace ANTU.Views;

[SupportedOSPlatform("Android")]
public partial class MateriaPrima
{
    private MateriaPrimaViewModel? _materiaPrimaViewModel;
	public MateriaPrima(MateriaPrimaViewModel materiaPrimaViewModel)
	{
		InitializeComponent();
        this._materiaPrimaViewModel = materiaPrimaViewModel;
        BindingContext = this._materiaPrimaViewModel;
    }
    
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
        if(!ComponenteEntradaDinamico.Children.Any())
            ComponenteEntradaDinamico.Add(this._materiaPrimaViewModel!.PanelComponents);

        if (!ListadoDatosPresentacion.Children.Any())
        {
            switch (this._materiaPrimaViewModel!.DataQuery)
            {
                case "MateriaPrima":
                    Titulo.Text = "Materia Prima";
                    Descripcion.Text = "Inventario de productos comprados";
                    if (this._materiaPrimaViewModel.MateriaPrimaView != null && this._materiaPrimaViewModel.MateriaPrimaView.BindingContext is
                        MateriaPrimaCollectionViewComponentsVIewModel item && item.CargarDatosMateriaPrimaProductoCommand.CanExecute(this))
                        await item.CargarDatosMateriaPrimaProductoCommand.ExecuteAsync(this);
                    ListadoDatosPresentacion.Add(this._materiaPrimaViewModel.MateriaPrimaView);
                    break;
                case "Catalogo":
                    Titulo.Text = "Catalogo Productos";
                    Descripcion.Text = "Listado de todos tus productos que fabricas";
                    if (this._materiaPrimaViewModel.CatalogoProductoView != null &&
                        this._materiaPrimaViewModel.CatalogoProductoView.BindingContext is
                            CatalogoProductoCollectionViewComponentsViewModel item2 &&
                        item2.CargarDatosCatalogoProductoCommand.CanExecute(this))
                        await item2.CargarDatosCatalogoProductoCommand.ExecuteAsync(this);
                    ListadoDatosPresentacion.Add(this._materiaPrimaViewModel.CatalogoProductoView);
                    break;
                case "Fabricacion":
                    Titulo.Text = "Fabricacion";
                    Descripcion.Text = "Gestiona la fabricacion de tus productos";
                    if (this._materiaPrimaViewModel.FabricacionView != null &&
                        this._materiaPrimaViewModel.FabricacionView.BindingContext is
                            FabricacionCollectionViewComponentsViewModel item3 &&
                        item3.ObtenerDatosProduccionCommand.CanExecute(this))
                        await item3.ObtenerDatosProduccionCommand.ExecuteAsync(this);
                    ListadoDatosPresentacion.Add(this._materiaPrimaViewModel.FabricacionView);
                    break;
                case "ProductosListos":
                    Titulo.Text = "Productos Listos";
                    Descripcion.Text = "Gestiona tus productos fabricados";
                    BotonNavegacionFormularios.IsVisible = false;
                    if (this._materiaPrimaViewModel.ProductosListosView != null &&
                        this._materiaPrimaViewModel.ProductosListosView.BindingContext is
                            ProductosListosCollectionViewComponentsViewModel item4 &&
                        item4.CargarDatosProductosListosCommand.CanExecute(this))
                        await item4.CargarDatosProductosListosCommand.ExecuteAsync(this);
                    ListadoDatosPresentacion.Add(this._materiaPrimaViewModel.ProductosListosView);
                    break;
            }

            await this._materiaPrimaViewModel.DesmontarSpinner();
        }

        // if (ListadoDatosPresentacion.Children.Any() && this.DataFormSource != null)
        // {
        //     switch (this.DataQuery)
        //     {
        //         case "ProductosListos":
        //             //realizar accion que encontramos en DataFormSource.
        //             Dictionary<string, object>? listadoAcciones = (this.DataFormSource as Dictionary<string, object>)!;
        //
        //             if (listadoAcciones != null && listadoAcciones.TryGetValue("Accion", out object? accion)  &&  listadoAcciones.TryGetValue("Parametros", out object? parametros) )
        //                 this._productosCollectionViewComponentViewModel!.acciones(accion!.ToString()!, parametros);
        //     
        //             break;
        //     }
        // }
    }
    
    protected override bool OnBackButtonPressed()
    {
        return this._materiaPrimaViewModel!.ControlarNavegacion();
    }
}