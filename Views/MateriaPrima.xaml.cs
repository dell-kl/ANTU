using ANTU.Models;
using ANTU.Resources.Components.CollectionViewComponents;
using ANTU.ViewModel;
using Syncfusion.Maui.ListView;
using System.Threading.Tasks;

namespace ANTU.Views;

public partial class MateriaPrima : ContentPage
{
    private MateriaPrimaCollectionViewComponents? materiaPrimaView;
    private CatalogoProductoCollectionViewComponents? catalogoProductoView;
    private MateriaPrimaViewModel materiaPrimaViewModel;


	public MateriaPrima(MateriaPrimaViewModel materiaPrimaViewModel)
	{
		InitializeComponent();
        this.materiaPrimaViewModel = materiaPrimaViewModel;
        BindingContext = this.materiaPrimaViewModel;
    
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (!this.materiaPrimaViewModel.DatosPresentacion.Any())
        {
            if (this.materiaPrimaViewModel.DataQuery.Equals("MateriaPrima"))
            {
                Titulo.Text = "Materia Prima";
                Descripcion.Text = "Inventario de productos comprados";
                this.materiaPrimaView = new MateriaPrimaCollectionViewComponents();
                this.materiaPrimaView!.BindingContext = this.materiaPrimaViewModel;
                ListadoDatosPresentacion.Add(materiaPrimaView);
            }
            else if (this.materiaPrimaViewModel.DataQuery.Equals("Catalogo"))
            {
                Titulo.Text = "Catalogo Productos";
                Descripcion.Text = "Listado de todos tus productos que fabricas";
                this.catalogoProductoView = new CatalogoProductoCollectionViewComponents();
                this.catalogoProductoView.BindingContext = this.materiaPrimaViewModel;
                //ListadoDatosPresentacion.Add(catalogoProductoView);
            }
        }
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        var searchBar = (sender as SearchBar);
        SfListView? ListadoDatos = null;
        
        if ( this.materiaPrimaViewModel.DataQuery.Equals("MateriaPrima") )
            ListadoDatos = this.materiaPrimaView.FindByName<SfListView>("ListadoMateriaPrima");

        if (ListadoDatos?.DataSource is not null)
        {
            ListadoDatos.DataSource!.Filter = (object obj) =>
            {
                if (searchBar == null || searchBar.Text == null)
                    return true;

                if (this.materiaPrimaViewModel.DataQuery.Equals("MateriaPrima"))
                {
                    var taskInfo = obj as MateriaPrimaProducto;
                    if (taskInfo!.nombreProducto.ToLower().Contains(searchBar.Text.ToLower()))
                        return true;
                }
            
                
                return false;
            };
            ListadoDatos.DataSource!.RefreshFilter();
        }

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

}