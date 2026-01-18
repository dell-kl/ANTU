using Data.Rest.RestInterfaces;
using ANTU.Views;
using Business.Services.IServices;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.ViewModel
{
    public partial class DashboardViewModel : ParentViewModel
    {
        public DashboardViewModel(IRestManagement IRestManagement, IPopupService popupService, IManagementService managementService) : base(IRestManagement, popupService, managementService) { 
            
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
