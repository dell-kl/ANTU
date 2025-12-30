using ANTU.Resources.Components.CollectionViewComponents;
using ANTU.Resources.Components.ControlersComponents;
using ANTU.ViewModel;
using ANTU.ViewModel.ComponentsViewModel;
using Mopups.Services;

namespace ANTU.Views;


public partial class MateriaPrima : ContentPage, IQueryAttributable
{
    object dataQuery;

    public object DataQuery {
        get => dataQuery;
        set {
            dataQuery = value;
            OnPropertyChanged();    
        }
    }
    
    private MateriaPrimaViewModel materiaPrimaViewModel;
    private FabricacionCollectionViewComponentsViewModel fabricacionCollectionViewComponentViewModel;
    private ProductosListosCollectionViewComponentsViewModel productosCollectionViewComponentViewModel;
    
	public MateriaPrima(MateriaPrimaViewModel materiaPrimaViewModel, FabricacionCollectionViewComponentsViewModel fabricacionCollectionViewComponentsViewModel, ProductosListosCollectionViewComponentsViewModel productosCollectionViewComponentViewModel)
	{
		InitializeComponent();
        this.materiaPrimaViewModel = materiaPrimaViewModel;
        this.fabricacionCollectionViewComponentViewModel = fabricacionCollectionViewComponentsViewModel;
        this.productosCollectionViewComponentViewModel = productosCollectionViewComponentViewModel;
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        this.DataQuery = query["DataQuery"];

        if (this.DataQuery.Equals("Fabricacion"))
            this.fabricacionCollectionViewComponentViewModel.DataQuery = this.DataQuery;
        else if (this.DataQuery.Equals("ProductosListos"))
            this.productosCollectionViewComponentViewModel.DataQuery = this.DataQuery;
        else
            this.materiaPrimaViewModel.DataQuery = this.DataQuery;
    }
    
    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
        if(!ComponenteEntradaDinamico.Children.Any())
            ComponenteEntradaDinamico.Add(this.materiaPrimaViewModel.PanelComponents);

        if (!ListadoDatosPresentacion.Children.Any())
        {
            switch (this.DataQuery)
            {
                case "MateriaPrima":
                    Titulo.Text = "Materia Prima";
                    Descripcion.Text = "Inventario de productos comprados";
                    BindingContext = this.materiaPrimaViewModel;
                    this.materiaPrimaViewModel.MateriaPrimaView!.BindingContext = this.materiaPrimaViewModel;
                    ListadoDatosPresentacion.Add(this.materiaPrimaViewModel.MateriaPrimaView);
                    break;
                case "Catalogo":
                    Titulo.Text = "Catalogo Productos";
                    Descripcion.Text = "Listado de todos tus productos que fabricas";
                    BindingContext = this.materiaPrimaViewModel;
                    this.materiaPrimaViewModel.CatalogoProductoView!.BindingContext = this.materiaPrimaViewModel;
                    ListadoDatosPresentacion.Add(this.materiaPrimaViewModel.CatalogoProductoView);
                    break;
                case "Fabricacion":
                    Titulo.Text = "Fabricacion";
                    Descripcion.Text = "Gestiona la fabricacion de tus productos";
                    BindingContext = this.fabricacionCollectionViewComponentViewModel;  
                    this.fabricacionCollectionViewComponentViewModel.FabricacionView!.BindingContext = this.fabricacionCollectionViewComponentViewModel;
                    ListadoDatosPresentacion.Add(this.fabricacionCollectionViewComponentViewModel.FabricacionView);
                    break;
                case "ProductosListos":
                    Titulo.Text = "Productos Listos";
                    Descripcion.Text = "Gestiona tus productos fabricados";
                    BotonNavegacionFormularios.IsVisible = false;
                    BindingContext = this.productosCollectionViewComponentViewModel;
                    this.productosCollectionViewComponentViewModel.ProductosListosView!.BindingContext = this.productosCollectionViewComponentViewModel;
                    ListadoDatosPresentacion.Add(this.productosCollectionViewComponentViewModel.ProductosListosView);
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
        return this.materiaPrimaViewModel.ControlarNavegacion();
    }


}