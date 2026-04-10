using System.Runtime.Versioning;
using Modelos.Dto;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Modelos;
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
            await base.MostrarSpinner();
            var resultado =
                await ManagementService.materiaPrimaService.RegistrarMateriaPrima(materiaPrimaFormulario,
                    FileManyResults);
            
            string mensajeErrorCompleto = "", mensajeSuccesful = "";
            foreach (var resultadoError in resultado.Errors)
                mensajeErrorCompleto += $" - {resultadoError.Message}\n";
            foreach (var resultadoSuccessful in resultado.Successful)
            {
                mensajeSuccesful += $"- {resultadoSuccessful}\n";
            }
            
            if (!resultado.Success && resultado.Successful.Count == 0) // No se registro nada                                                                                       
                await Mensaje.MensajeError($"Error Registrar", mensajeErrorCompleto);
            else
            {
                this.MateriaPrimaFormularioComponentes.ResetearValoresFormulario();
                FileManyResults.Clear();
                
                if (!resultado.Success && resultado.Successful.Count != 0) // Se registro la materia prima y no las imagenes
                    await Mensaje.MensajeAdvertencia($"Materia Prima Registrada", $"{mensajeSuccesful}\n{mensajeErrorCompleto}");
                else if(resultado.Success) // Se ejecutaron exitosamente todos los procesos.
                    await Mensaje.MensajeCorrecto("Solicitud Aceptada", mensajeSuccesful);
            }
            
            await EliminarSpinnerDirectamente();
            
        }
    }
}
