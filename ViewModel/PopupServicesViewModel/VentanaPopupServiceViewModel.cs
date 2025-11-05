
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.ViewModel.PopupServicesViewModel
{
    public partial class VentanaPopupServiceViewModel : ParentViewModel
    {
        
        public VentanaPopupServiceViewModel(IRestManagement restManagement, IPopupService popupService)
        :base(restManagement, popupService)
        {
        }

        
    }
}
