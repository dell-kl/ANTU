using ANTU.Models.Dto;
using ANTU.Resources.Components.PopupComponents;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using Syncfusion.Maui.DataForm;
using System.Collections.ObjectModel;

namespace ANTU.ViewModel
{
    public partial class ParentViewModel : ObservableObject, IQueryAttributable
    {
        protected readonly IPopupService _popupService;
        protected readonly IRestManagement _restManagement;

        [ObservableProperty]
        private object dataQuery = new object();

        [ObservableProperty]
        private ObservableCollection<FileResultExtensible> _fileManyResults = new ObservableCollection<FileResultExtensible>();
        
        public ParentViewModel(IRestManagement restManagement, IPopupService popupService) {
            _restManagement = restManagement;
            _popupService = popupService;
        }

        //Navigate

        public virtual async Task NavegarFormulario(string objeto)
        {
            await MostrarSpinner();
            await Shell.Current.GoToAsync(objeto);
        }

        public virtual async Task NavegarFormulario(string objeto, ShellNavigationQueryParameters queryParameters = null)
        {
            await MostrarSpinner();
            await Shell.Current.GoToAsync(objeto, queryParameters);
        }

        public async Task MostrarSpinner()
        {
            bool resultado = MopupService.Instance.PopupStack.Where(item => item is VentaSpinnerLoading).Any();

            if (!resultado)
                await MopupService.Instance.PushAsync(new VentaSpinnerLoading());
        }

        public virtual async Task DesmontarSpinner() {
            bool resultado = MopupService.Instance.PopupStack.Where(item => item is VentaSpinnerLoading).Any();
            if (resultado)
                await MopupService.Instance.PopAsync();
        }

        // File Picker

        public virtual async Task SeleccionarArchivoMostrar()
        {

            PickOptions options = new PickOptions();
            options.FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "image/jpeg" } }, // MIME type
                });

            IEnumerable<FileResult> resultado = await FilePicker.PickMultipleAsync(options);


            if (resultado is null || (this.FileManyResults.Count()) + (resultado.Count()) > 5 )
                return;


            var fileResultList = resultado.Select(item =>
            {
                return new FileResultExtensible(item) { };
            });

            this.FileManyResults = this.FileManyResults.Concat(fileResultList).ToObservableCollection();
        }

        public virtual void EliminarArchivo(string codigo)
        {
            FileResultExtensible? archivo = this.FileManyResults.Where(item => item.codigo.Equals(codigo)).FirstOrDefault();

            if (archivo != null)
                this.FileManyResults.Remove(archivo);

        }

        public virtual async Task EliminarArchivoPrueba(string codigo)
        {
            FileResultExtensible? archivo = this.FileManyResults.Where(item => item.codigo.Equals(codigo)).FirstOrDefault();

            if (archivo != null)
                this.FileManyResults.Remove(archivo);

        }

        public virtual void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("DataQuery"))
                this.DataQuery = query["DataQuery"];
        }

        public virtual async Task MostrarVentanaConfirmacion(
            string titulo,
            string descripcion,
            string mensaje,
            string textButtonCancelar,
            string textButtonConfirmar,
            Func<Task> FuncProcess
        )
        {
            if (!MopupService.Instance.PopupStack.Where(item => item is VentanaConfirmacionEmergente).Any() )
            {
                VentanaConfirmacionEmergente ventanaConfirmacion = new VentanaConfirmacionEmergente();
                ventanaConfirmacion.FindByName<Label>("Titulo").Text = titulo;
                ventanaConfirmacion.FindByName<Label>("Descripcion").Text = descripcion;
                ventanaConfirmacion.FindByName<Label>("Mensaje").Text = mensaje;
                
                Button buttonRegresaar = ventanaConfirmacion.FindByName<Button>("BotonRegresar");
                buttonRegresaar.Text = textButtonCancelar;
                
                Button buttonConfirmacion = ventanaConfirmacion.FindByName<Button>("BotonConfirmacion");
                buttonConfirmacion.Text = textButtonConfirmar;
                buttonConfirmacion.Clicked += async (sender, e) => {
                    buttonConfirmacion.IsEnabled = false;
                    buttonConfirmacion.IsVisible = false;
                    buttonRegresaar.IsEnabled = false;
                    buttonRegresaar.IsVisible = false;
                    //ventanaConfirmacion.FindByName<Border>("MensajeCargando").IsVisible = true;
                    //await MopupService.Instance.PopAsync();
                    await MostrarSpinner();
                    await MopupService.Instance.RemovePageAsync(ventanaConfirmacion);
                    await FuncProcess();
                };
                await MopupService.Instance.PushAsync(ventanaConfirmacion);
            }
        }

        public virtual async Task EjecutarFormularioEmergente(string titulo, string parrafo, object formulario, Action<ReadOnlyDictionary<string,object>> FuncValidateForm, Func<Task<bool>> FuncProcessData)
        {
            FormularioEmergente Formulario = new FormularioEmergente();
            Formulario.FindByName<Label>("TituloFormularioEmergente").Text = titulo;
            Formulario.FindByName<Label>("DescripcionFormularioEmergente").Text = parrafo;
            SfDataForm pagina = Formulario.FindByName<SfDataForm>("Formulario");
            pagina.DataObject = formulario;
            //metodos para implementar dentro de nuestro formulario.

            pagina.ValidateForm += (sender, e) => {
                var DatosActualizados = e.NewValues;
                FuncValidateForm(DatosActualizados);
            };

            Formulario.FindByName<Button>("BontonCancelarFormulario").Clicked += async (sender, e) => {
                if (MopupService.Instance.PopupStack.Where(item => item is FormularioEmergente).Any())
                    await MopupService.Instance.PopAsync();

            };

            Formulario.FindByName<Button>("BotonConfirmarFormulario").Clicked += async (sender, e) => { 
                if ( pagina.Validate() )
                {

                    await MostrarVentanaConfirmacion(
                        "Confirmar Formulario",
                        "Si estas seguro de tus datos puede dar en REGISTRAR, caso contrario presiona REGRESAR para volver al formulario",
                        "Registrando, espere...",
                        "Regresar",
                        "Registrar",
                        async () =>
                        {
                            await FuncProcessData();
                        }
                    );
                    
                }
            };

            await MopupService.Instance.PushAsync(Formulario);
        }
    }
}
