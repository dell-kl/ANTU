using ANTU.Models;
using ANTU.Resources.Rest.RestInterfaces;
using ANTU.Views;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace ANTU.ViewModel
{
    public partial class DashboardViewModel : ParentViewModel
    {

        public DashboardViewModel(IRestManagement IRestManagement, IPopupService popupService) : base(IRestManagement, popupService) { 
        }
        
        [RelayCommand(AllowConcurrentExecutions = false)]
        public override async Task NavegarFormulario(string objeto)
        {
            var datosNavegacion = new ShellNavigationQueryParameters {
                {
                    "DataQuery", objeto
                }
            };

            await base.NavegarFormulario(nameof(MateriaPrima), datosNavegacion);
        }

    }
}
