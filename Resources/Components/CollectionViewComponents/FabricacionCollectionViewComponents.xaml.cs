using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANTU.Models;
using ANTU.ViewModel;
using ANTU.ViewModel.ComponentsViewModel;
using Syncfusion.Maui.ListView;
using Syncfusion.Maui.Toolbar;

namespace ANTU.Resources.Components.CollectionViewComponents;

public partial class FabricacionCollectionViewComponents : ContentView
{

    
    public FabricacionCollectionViewComponents()
    {
        InitializeComponent();
    }

    protected async override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (this.BindingContext is FabricacionCollectionViewComponentsViewModel fabricacionViewmodel)
        {
            await fabricacionViewmodel.cargaProductos();
            await fabricacionViewmodel.DesmontarSpinner();
        }
    }

    private void ListadoProduccion_OnSelectionChanging(object? sender, ItemSelectionChangingEventArgs e)
    {
        if (this.BindingContext is FabricacionCollectionViewComponentsViewModel fabricacionViewModel)
        {
            if (!fabricacionViewModel.CheckBoxEstado)
            {
                fabricacionViewModel.CheckBoxEstado = true;
                fabricacionViewModel.PanelVisible = true;
                ListadoProduccion.SelectionGesture = TouchGesture.Tap;
            }
            
            Produccion? produccion2 = (e.AddedItems?.FirstOrDefault() as Produccion);
            
            if ( produccion2 is null )
                produccion2 = (e.RemovedItems?.FirstOrDefault() as Produccion);

            var listadoProducion = fabricacionViewModel.DatosPresentacion
                .Where(item => item is Produccion produccion && produccion.Equals(produccion2))
                .ToList();

            if (listadoProducion.Any() && listadoProducion.First() is Produccion produccion3)
                produccion3.EstadoFabricado = !produccion2!.EstadoFabricado;
        }
    }

    private void ToolbarProduccion_OnTapped(object? sender, ToolbarTappedEventArgs e)
    {
        if ( this.BindingContext is FabricacionCollectionViewComponentsViewModel fabricacionViewModel )
        {
            string? type = e.NewToolbarItem?.Name;

            switch (type)
            {
                case "Cancelar":
                    //reestablecer valores.
                    fabricacionViewModel.CheckBoxEstado = false;
                    fabricacionViewModel.PanelVisible = false;
                    ListadoProduccion.SelectionGesture = TouchGesture.LongPress;
                    break;
                case "Fabricados":
                    if (fabricacionViewModel.GenerarCambioEstadoProductosFabricacionCommand.CanExecute(this))
                        fabricacionViewModel.GenerarCambioEstadoProductosFabricacionCommand.Execute(this);
                    break;
                case "Todos":
                    //no implementar esto, hasta ver si es posible mas adelante.
                    break;
            }
        }
    }
}