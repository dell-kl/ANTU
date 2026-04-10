using System.Runtime.Versioning;
using Modelos.Dto;
using Modelos;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class CatalogoProductoFormularioViewModel : ParentViewModel
    {
        //public CatalogoProductoFormulario catalogoProductoFormulario { set; get; } = new CatalogoProductoFormulario();

        [ObservableProperty]
        private CatalogoProductoFormularioComponentes catalogoProductoFormularioComponenets = new CatalogoProductoFormularioComponentes();

        public CatalogoProductoFormularioViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) : base(restManagement, popupService, managementService, mensaje)
        {
            catalogoProductoFormularioComponenets.BindingContext = this;
        }
        
        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task RegistarCatalogoProducto(CatalogoProductoFormulario catalogoProductoFormulario)
        {
            await MostrarSpinner();

            var resultado = await ManagementService.CatalogoProductoService.RegistrarCatalogoProductoAsync(
                catalogoProductoFormulario,
                FileManyResults
            );
            
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
                FileManyResults.Clear();
                
                if (!resultado.Success && resultado.Successful.Count != 0) // Se registro la materia prima y no las imagenes
                    await Mensaje.MensajeAdvertencia($"Catalogo Producto Registrado", $"{mensajeSuccesful}\n{mensajeErrorCompleto}");
                else if(resultado.Success) // Se ejecutaron exitosamente todos los procesos.
                    await Mensaje.MensajeCorrecto("Solicitud Aceptada", mensajeSuccesful);
            }
            this.CatalogoProductoFormularioComponenets.ResetearValoresFormulario();
            await EliminarSpinnerDirectamente();
        }
    }
}
