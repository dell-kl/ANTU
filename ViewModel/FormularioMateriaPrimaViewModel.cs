using System.Runtime.Versioning;
using Modelos.Dto;
using Modelos.RequestDto;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Modelos.ResultDto;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class FormularioMateriaPrimaViewModel : ParentViewModel
    {
        [ObservableProperty]
        private bool statusButtonTrash = true;

        [ObservableProperty]
        private bool statusButtonPickerFile = true;

        // Componente de formulario.
        [ObservableProperty]
        private MateriaPrimaFormularioComponentes _materiaPrimaFormularioComponentes;
        
        public FormularioMateriaPrimaViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje)
        : base(restManagement, popupService, managementService, mensaje)
        {
            this.MateriaPrimaFormularioComponentes = new MateriaPrimaFormularioComponentes();
            this.MateriaPrimaFormularioComponentes.BindingContext = this;
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task RegistrarMateriaPrima(MateriaPrimaFormulario materiaPrimaFormulario)
        {
            //cubrir con ventana emergente.
            await base.MostrarSpinner();

            RequestResultDto<string> resultado = await ManagementService.materiaPrimaService.RegistrarMateriaPrima(materiaPrimaFormulario, FileManyResults);

            // if (!resultado.IsSuccess)
            //     Mensaje.MensajeCorrecto("Error Registrar", resultado.Error!);
            // else
            // {
            //     await ManagementService.materiaPrimaService.RegistarImagenesMateriaPrima(FileManyResults);
            //     
            //     Mensaje.MensajeCorrecto("Registrado Materia Prima", resultado.Value!);
            // }
            //
            //
            // await base.DesmontarSpinner();    
            // FileManyResults.Clear();
            
        }
    }
}
