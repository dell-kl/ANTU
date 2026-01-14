
using ANTU.Models.Dto;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.ViewModel.PopupServicesViewModel
{
    public partial class VentanaPopupServiceViewModel : ParentViewModel
    {
        //Esto servira para el code-behind de VentanaPopupService y decidira que tipo de ContentView inyectar.
        [ObservableProperty]
        private string _tipoFormulario = "";

        //Esto servira para la ContentView inyectada y decidira que accion realizar (agregar, editar, ver, etc).
        [ObservableProperty]
        private string _accion = "";

        //La propiedad de a continuacion son datos adicionales que se pueden enviar al ViewModel de la ContentView inyectada.
        [ObservableProperty]
        private object datosAdicionales;

        [ObservableProperty]
        private bool _botonCancelar = true;

        [ObservableProperty]
        private CatalogoProductoFormularioComponentes formularioCatalogoProducto = new CatalogoProductoFormularioComponentes();

        [ObservableProperty]
        private MateriaPrimaFormularioComponentes materiaPrimaFormulario = new MateriaPrimaFormularioComponentes();

        public VentanaPopupServiceViewModel(IPopupService popupService, IRestManagement restManagement) : base(restManagement, popupService)
        {
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);

            List<object> datos = (base.DataQuery as List<object>)!;
            this.TipoFormulario = (datos[0] as string)!;
            this.Accion = (datos[1] as string)!;
            this.datosAdicionales = datos[2];

            setearBindingContext();
        }

        public void setearBindingContext()
        {
            switch (this.TipoFormulario)
            {
                case "CatalogoProductoFormulario":
                    this.FormularioCatalogoProducto.BindingContext = this;
                    break;

                case "MateriaPrimaFormulario":
                    this.MateriaPrimaFormulario.BindingContext = this;
                    break;
            }
        }


        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task CerrarPopup(object datos)
        {
            await _popupService.ClosePopupAsync<object>(Shell.Current, datos);
        }

    }
}
