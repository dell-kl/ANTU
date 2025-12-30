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
}