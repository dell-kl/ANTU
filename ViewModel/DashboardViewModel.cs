using System.Runtime.Versioning;
using ANTU.Resources.Utilidades;
using Data.Rest.RestInterfaces;
using ANTU.Views;
using Business.Services.IServices;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class DashboardViewModel : ParentViewModel
    {
        public DashboardViewModel(IRestManagement IRestManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) : base(IRestManagement, popupService, managementService, mensaje) { 
            
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
