using ANTU.Models;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ANTU.ViewModel.ComponentsViewModel;

public partial class ProductosListosCollectionViewComponentsViewModel : MateriaPrimaViewModel
{
    [ObservableProperty]
    private bool checkBoxEstado = false;

    [ObservableProperty]
    private bool panelVisible = false;
    
    public ProductosListosCollectionViewComponentsViewModel(IRestManagement restManagement, IPopupService popupService)
    : base(restManagement, popupService)
    {
        
    }

    public void acciones(string accion, object parametros)
    {
        switch (accion)
        {
            case "ELIMINAR_ITEM":
                
                string identificadorProductoListo = ( parametros is string ) ? parametros.ToString()! : string.Empty;

                Produccion? registro = this.DatosPresentacion.Where(item =>
                    item is Produccion produccion && produccion.Identificador.Equals(identificadorProductoListo))
                    .ToList()
                    .FirstOrDefault() as Produccion;
                
                this.DatosPresentacion.Remove(registro);
                
                break;
        }
    }
}