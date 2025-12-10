using ANTU.Resources.Components.FormularioComponentes;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ANTU.ViewModel
{
    public partial class FabricacionFormularioViewModel : ParentViewModel
    {
        
        [ObservableProperty]
        private FabricacionFormularioComponentes fabricacionFormularioComponentes = new FabricacionFormularioComponentes();

        public FabricacionFormularioViewModel(IRestManagement restManagement, IPopupService popupService) : base(restManagement, popupService) {
            this.FabricacionFormularioComponentes.BindingContext = this;
        }
        
    }
}
