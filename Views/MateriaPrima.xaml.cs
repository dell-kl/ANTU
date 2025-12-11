using ANTU.Resources.Components.CollectionViewComponents;
using ANTU.Resources.Components.ControlersComponents;
using ANTU.ViewModel;

namespace ANTU.Views;

public partial class MateriaPrima : ContentPage
{

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
        
        if(!ComponenteEntradaDinamico.Children.Any())
            ComponenteEntradaDinamico.Add(this.materiaPrimaViewModel.PanelComponents);

        if (!ListadoDatosPresentacion.Children.Any())
        {
            switch (this.materiaPrimaViewModel.DataQuery)
            {
                case "MateriaPrima":
                    Titulo.Text = "Materia Prima";
                    Descripcion.Text = "Inventario de productos comprados";
                    this.materiaPrimaViewModel.MateriaPrimaView!.BindingContext = this.materiaPrimaViewModel;
                    ListadoDatosPresentacion.Add(this.materiaPrimaViewModel.MateriaPrimaView);
                    break;
                case "Catalogo":
                    Titulo.Text = "Catalogo Productos";
                    Descripcion.Text = "Listado de todos tus productos que fabricas";
                    this.materiaPrimaViewModel.CatalogoProductoView!.BindingContext = this.materiaPrimaViewModel;
                    ListadoDatosPresentacion.Add(this.materiaPrimaViewModel.CatalogoProductoView);
                    break;
                case "Fabricacion":
                    Titulo.Text = "Fabricacion";
                    Descripcion.Text = "Gestiona la fabricacion de tus productos";
                    this.materiaPrimaViewModel.FabricacionView.BindingContext = this.materiaPrimaViewModel;
                    ListadoDatosPresentacion.Add(this.materiaPrimaViewModel.FabricacionView);
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