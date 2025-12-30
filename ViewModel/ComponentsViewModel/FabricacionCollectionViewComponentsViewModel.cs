using ANTU.Models;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Syncfusion.Maui.Data;

namespace ANTU.ViewModel.ComponentsViewModel;

public partial class FabricacionCollectionViewComponentsViewModel : MateriaPrimaViewModel
{
    [ObservableProperty]
    private bool checkBoxEstado = false;

    [ObservableProperty]
    private bool panelVisible = false;
    
    public FabricacionCollectionViewComponentsViewModel(IRestManagement restManagement, IPopupService popupService)
    : base(restManagement, popupService)
    {
    }
    
    //LLama al api para cambiar productos en produccion a ya fabricados.
    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task GenerarCambioEstadoProductosFabricacion()
    {
        var productosFabricados = base.DatosPresentacion
            .Where(item => item is Produccion produccion && produccion.EstadoFabricado)
            .Select(item => new Produccion()
            {
                Identificador = (item as Produccion)!.Identificador,
                Estado = 2
            } );

        if (productosFabricados.Any())
        {
            base.MostrarSpinner();
            
            bool resultado =
                await _restManagement.Produccion.cambiarEstadoProduccionAFabricado(productosFabricados,
                    () => base.DesmontarSpinner());
            if (resultado)
                base.DatosPresentacion = base.DatosPresentacion.Where(item => item is Produccion produccion && !produccion.EstadoFabricado).ToObservableCollection();
        }
        
    }
}