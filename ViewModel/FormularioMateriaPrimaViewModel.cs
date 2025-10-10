using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Components.PopupComponents;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Shapes;
using Mopups.Services;

namespace ANTU.ViewModel
{
    public partial class FormularioMateriaPrimaViewModel : ParentViewModel
    {
        private readonly IPopupService _popupService;
        public FormularioMateriaPrimaViewModel(IRestManagement restManagement, IPopupService popupService)
        : base(restManagement)
        {
            _popupService = popupService;
        }

        public MateriaPrimaFormularioDto _materiaPrimaDTO { get; set; } = new MateriaPrimaFormularioDto();


        private bool statusButtonTrash = true;
        public bool StatusButtonTrash { set => SetProperty(ref statusButtonTrash, value); get => statusButtonTrash; }


        private bool statusButtonPickerFile = true;
        public bool StatusButtonPickerFile { set => SetProperty(ref statusButtonPickerFile, value); get => statusButtonPickerFile; }

        [RelayCommand]
        public override Task SeleccionarArchivoMostrar()
        {
            return base.SeleccionarArchivoMostrar();
        }

        [RelayCommand]
        public async Task RegistrarMateriaPrima()
        {
            //cubrir con ventana emergente.
            await MopupService.Instance.PushAsync(new VentanaEmergente(
                "Registrando Materia Prima",
                "Esto puede tomar algunos minutos ⏰, espere por favor. Hasta mientras puede hacerse un café🍵.",
                true));

            MateriaPrimaRequestDto materiaPrimaRequestDto = new MateriaPrimaRequestDto() { 
                id_dto = Guid.NewGuid().ToString(),
                nombre_dto = _materiaPrimaDTO.MateriaPrima,
                KgMonitoringDtos = new List<KgSeguimientoRequestDto>()
                {
                    new KgSeguimientoRequestDto()
                    {
                        id_dto = null, 
                        cantidad_dto = int.Parse(_materiaPrimaDTO.Cantidad),
                        kg_standard = double.Parse(_materiaPrimaDTO.KGStandard),
                        price_dto = decimal.Parse( _materiaPrimaDTO.Precio )
                    }
                }
            };

            bool solicitud = await _restManagement.MateriaPrima.Add(materiaPrimaRequestDto);

            if (!solicitud)
            {
                await MopupService.Instance.PopAsync();
                await MopupService.Instance.PushAsync(new VentanaEmergente(
                "Error Guardado",
                "No pudo ser posible registrar tu materia prima",
                false,
                true,
                "alert.png",
                true));

                return;
            }

            if (FileManyResults.Any())
                await _restManagement.MateriaPrima.SaveImages(FileManyResults, materiaPrimaRequestDto.id_dto);

            await MopupService.Instance.PopAsync();

            await MopupService.Instance.PushAsync(new VentanaEmergente(
            "Guardado Exitosamente",
            "Tu materia prima se ha registrado exitosamente",
            false,
            true,
            "successful.png",
            true));

            _materiaPrimaDTO.limpiarFormulario();
            FileManyResults.Clear();
    

        }
    }
}
