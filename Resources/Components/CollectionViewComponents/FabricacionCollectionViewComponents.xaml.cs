using Modelos;
using ANTU.ViewModel.ComponentsViewModel;
using Syncfusion.Maui.ListView;
using Syncfusion.Maui.Toolbar;

namespace ANTU.Resources.Components.CollectionViewComponents;

public partial class FabricacionCollectionViewComponents : ContentView
{

    private FabricacionCollectionViewComponentsViewModel itemViewModel;
    
    public FabricacionCollectionViewComponents(FabricacionCollectionViewComponentsViewModel itemViewModel)
    {
        InitializeComponent();
        this.itemViewModel = itemViewModel;
        BindingContext = this.itemViewModel;
        
        // Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, EventArgs e)
    {
        await this.itemViewModel.ObtenerDatosProduccion();
    }
    
    private void ListadoProduccion_OnSelectionChanging(object? sender, ItemSelectionChangingEventArgs e)
    {
        
        if (!this.itemViewModel.CheckBoxEstado)
        {
            this.itemViewModel.CheckBoxEstado = true;
            this.itemViewModel.PanelVisible = true;
            ListadoProduccion.SelectionGesture = TouchGesture.Tap;
        }
        
        Produccion? produccion2 = (e.AddedItems?.FirstOrDefault() as Produccion);
        
        if ( produccion2 is null )
            produccion2 = (e.RemovedItems?.FirstOrDefault() as Produccion);

        var listadoProducion = this.itemViewModel.DatosProducciones
            .Where(item => item.Equals(produccion2))
            .ToList();

        if (listadoProducion.Any() && listadoProducion.First() is Produccion produccion3)
            produccion3.EstadoFabricado = !produccion2!.EstadoFabricado;
        
    }

    private void ToolbarProduccion_OnTapped(object? sender, ToolbarTappedEventArgs e)
    {
        string? type = e.NewToolbarItem?.Name;
        switch (type)
        {
            case "Cancelar":
                //reestablecer valores.
                this.itemViewModel.CheckBoxEstado = false;
                this.itemViewModel.PanelVisible = false;
                ListadoProduccion.SelectionGesture = TouchGesture.LongPress;
                break;
            case "Fabricados":
                if (this.itemViewModel.GenerarCambioEstadoProductosFabricacionCommand.CanExecute(this))
                    this.itemViewModel.GenerarCambioEstadoProductosFabricacionCommand.Execute(this);
                break;
            case "Todos":
                //no implementar esto, hasta ver si es posible mas adelante.
                break;
        }
    }
}