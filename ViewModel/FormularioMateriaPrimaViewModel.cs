using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.ViewModel
{
    public partial class FormularioMateriaPrimaViewModel : ParentViewModel
    {
        [ObservableProperty]
        private bool statusButtonTrash = true;

        [ObservableProperty]
        private bool statusButtonPickerFile = true;

        // Componente de formulario.
        [ObservableProperty]
        private MateriaPrimaFormularioComponentes materiaPrimaFormularioComponentes = new MateriaPrimaFormularioComponentes();
        
        public FormularioMateriaPrimaViewModel(IRestManagement restManagement, IPopupService popupService)
        : base(restManagement, popupService)
        {
            this.materiaPrimaFormularioComponentes.BindingContext = this;
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task RegistrarMateriaPrima(MateriaPrimaFormulario materiaPrimaFormulario)
        {
            //cubrir con ventana emergente.
            await base.MostrarSpinner();

            await _restManagement.MateriaPrima.Add(
                new MateriaPrimaRequestDto()
                {
                    id_dto = Guid.NewGuid().ToString(),
                    nombre_dto = materiaPrimaFormulario.MateriaPrima,
                    KgMonitoringDtos = new List<KgSeguimientoRequestDto>()
                    {
                        new KgSeguimientoRequestDto()
                        {
                            id_dto = null,
                            cantidad_dto = materiaPrimaFormulario.Cantidad,
                            kg_standard = materiaPrimaFormulario.KgStandard,
                            price_dto = (decimal) materiaPrimaFormulario.Precio
                        }   
                    }
                }, 
                () => DesmontarSpinner(),
                FileManyResults);


            FileManyResults.Clear();

        }
    }
}
