using ANTU.ViewModel;
using ANTU.ViewModel.ComponentsViewModel;


namespace ANTU.Views;


public partial class MateriaPrima : ContentPage, IQueryAttributable
{
    private object? _dataQuery = null;
    private object? _dataFormSource = null;
    
    public object? DataQuery {
        get => _dataQuery;
        set {
            _dataQuery = value;
            OnPropertyChanged();    
        }
    }
    
    public object? DataFormSource {
        get => _dataFormSource;
        set {
            _dataFormSource = value;
            OnPropertyChanged();    
        }
    }
    
    private MateriaPrimaViewModel? _materiaPrimaViewModel;
    private FabricacionCollectionViewComponentsViewModel? _fabricacionCollectionViewComponentViewModel;
    private ProductosListosCollectionViewComponentsViewModel? _productosCollectionViewComponentViewModel;
    
	public MateriaPrima(MateriaPrimaViewModel materiaPrimaViewModel, FabricacionCollectionViewComponentsViewModel fabricacionCollectionViewComponentsViewModel, ProductosListosCollectionViewComponentsViewModel productosCollectionViewComponentViewModel)
	{
		InitializeComponent();
        this._materiaPrimaViewModel = materiaPrimaViewModel;
        this._fabricacionCollectionViewComponentViewModel = fabricacionCollectionViewComponentsViewModel;
        this._productosCollectionViewComponentViewModel = productosCollectionViewComponentViewModel;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("DataQuery", out object? dataQueryValue))
        {
            this.DataQuery = dataQueryValue;

            if (this.DataQuery.Equals("Fabricacion"))
                this._fabricacionCollectionViewComponentViewModel?.DataQuery = this.DataQuery;
            else if (this.DataQuery.Equals("ProductosListos"))
                this._productosCollectionViewComponentViewModel?.DataQuery = this.DataQuery;
            else
                this._materiaPrimaViewModel?.DataQuery = this.DataQuery;
        }
        else if (query.TryGetValue("DataFormSource", out object? dataFormSourceValue))
        {
            this.DataFormSource = dataFormSourceValue;
            
            if(this.DataQuery!.Equals("ProductosListos"))
                this._productosCollectionViewComponentViewModel?.DataFormSource = this.DataFormSource;
        }
    }
    
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
        if(!ComponenteEntradaDinamico.Children.Any())
            ComponenteEntradaDinamico.Add(this._materiaPrimaViewModel!.PanelComponents);

        if (!ListadoDatosPresentacion.Children.Any())
        {
            switch (this.DataQuery)
            {
                case "MateriaPrima":
                    Titulo.Text = "Materia Prima";
                    Descripcion.Text = "Inventario de productos comprados";
                    BindingContext = this._materiaPrimaViewModel;
                    this._materiaPrimaViewModel!.MateriaPrimaView!.BindingContext = this._materiaPrimaViewModel;
                    ListadoDatosPresentacion.Add(this._materiaPrimaViewModel.MateriaPrimaView);
                    break;
                case "Catalogo":
                    Titulo.Text = "Catalogo Productos";
                    Descripcion.Text = "Listado de todos tus productos que fabricas";
                    BindingContext = this._materiaPrimaViewModel;
                    this._materiaPrimaViewModel!.CatalogoProductoView!.BindingContext = this._materiaPrimaViewModel;
                    ListadoDatosPresentacion.Add(this._materiaPrimaViewModel.CatalogoProductoView);
                    break;
                case "Fabricacion":
                    Titulo.Text = "Fabricacion";
                    Descripcion.Text = "Gestiona la fabricacion de tus productos";
                    BindingContext = this._fabricacionCollectionViewComponentViewModel;  
                    this._fabricacionCollectionViewComponentViewModel!.FabricacionView!.BindingContext = this._fabricacionCollectionViewComponentViewModel;
                    ListadoDatosPresentacion.Add(this._fabricacionCollectionViewComponentViewModel.FabricacionView);
                    break;
                case "ProductosListos":
                    Titulo.Text = "Productos Listos";
                    Descripcion.Text = "Gestiona tus productos fabricados";
                    BotonNavegacionFormularios.IsVisible = false;
                    BindingContext = this._productosCollectionViewComponentViewModel;
                    this._productosCollectionViewComponentViewModel!.ProductosListosView!.BindingContext = this._productosCollectionViewComponentViewModel;
                    ListadoDatosPresentacion.Add(this._productosCollectionViewComponentViewModel.ProductosListosView);
                    break;
            }
        }

        if (ListadoDatosPresentacion.Children.Any() && this.DataFormSource != null)
        {
            switch (this.DataQuery)
            {
                case "ProductosListos":
                    //realizar accion que encontramos en DataFormSource.
                    Dictionary<string, object>? listadoAcciones = (this.DataFormSource as Dictionary<string, object>)!;

                    if (listadoAcciones != null && listadoAcciones.TryGetValue("Accion", out object? accion)  &&  listadoAcciones.TryGetValue("Parametros", out object? parametros) )
                        this._productosCollectionViewComponentViewModel!.acciones(accion!.ToString()!, parametros);
            
                    break;
            }
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

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

    protected override bool OnBackButtonPressed()
    {
        return this._materiaPrimaViewModel!.ControlarNavegacion();
    }


}