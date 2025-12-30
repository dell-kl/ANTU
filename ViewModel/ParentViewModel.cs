using ANTU.Models.Dto;
using ANTU.Resources.Components.FormularioComponentes;
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
using ANTU.Resources.Utilidades;

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

        [ObservableProperty]
        private ImagenesGuardarFormularioComponentes imagenesGuardarFormularioComponentes = new ImagenesGuardarFormularioComponentes();

        public ParentViewModel(IRestManagement restManagement, IPopupService popupService) {
            _restManagement = restManagement;
            _popupService = popupService;

            //Este formulario de imagenes se utilizara en varios formularios;
            this.imagenesGuardarFormularioComponentes.BindingContext = this;
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

        public bool ControlarNavegacion()
        {
            bool resultado = MopupService.Instance.PopupStack.Where(item => item is VentaSpinnerLoading).Any();

            return resultado;
        }

        // File Picker
        [RelayCommand(AllowConcurrentExecutions = false)]
        public virtual async Task SeleccionarArchivoMostrar()
        {
            try
            {
                PickOptions options = new PickOptions();
                options.FileTypes = FilePickerFileType.Images;
                options.PickerTitle = "Selecciona hasta 5 imagenes";

                IEnumerable<FileResult?> resultado = await FilePicker.PickMultipleAsync(options);
                
                if (resultado is null || (this.FileManyResults.Count()) + (resultado.Count()) > 5)
                    return;
                
                var fileResultList = resultado.Select(item =>
                {
                    return new FileResultExtensible(item) { };
                });
                
                this.FileManyResults = this.FileManyResults.Concat(fileResultList).ToObservableCollection();
            }
            catch (TaskCanceledException ex) {
                Console.WriteLine(ex.Message);
            }
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

    }
}
