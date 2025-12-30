using ANTU.Models;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ANTU.ViewModel;

public partial class ProductoListoDetalleViewModel : ParentViewModel
{
    [ObservableProperty] 
    private Produccion produccion;
    
    public ProductoListoDetalleViewModel(IRestManagement restManagement, IPopupService popupService) 
        : base(restManagement, popupService)
    {
        
    }

    public override void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        base.ApplyQueryAttributes(query);
        
        if ( base.DataQuery is not null )
            Produccion = (base.DataQuery as Produccion)!;
    }
    
    
}