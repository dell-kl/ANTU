using Modelos.Dto;
using ANTU.Resources.Components.FormularioComponentes;
using ANTU.Resources.Components.PopupComponents;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using ANTU.Resources.Utilidades;
using Business.Services.IServices;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public abstract partial class ParentViewModel : ObservableObject, IQueryAttributable
    {
        protected readonly IPopupService PopupService;
        protected readonly IRestManagement RestManagement;
        protected readonly IManagementService ManagementService;

        [ObservableProperty]
        private object dataQuery = new object();
        
        [ObservableProperty]
        private object _dataFormSource = new object();
        
        [ObservableProperty]
        private Mensaje _mensaje;

        [ObservableProperty] private ObservableCollection<FileResultExtensible> _fileManyResults;

        [ObservableProperty] private ImagenesGuardarFormularioComponentes _imagenesGuardarFormularioComponentes;

        public ParentViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService, Mensaje mensaje) {
            RestManagement = restManagement;
            PopupService = popupService;
            ManagementService = managementService;
            Mensaje = mensaje;

            //inicializar nuestra lista de imagenes que vamos a gaurdar.
            this.FileManyResults = new ObservableCollection<FileResultExtensible>();
            
            //Este formulario de imagenes se utilizara en varios formularios;
            this.ImagenesGuardarFormularioComponentes = new ImagenesGuardarFormularioComponentes();
            this.ImagenesGuardarFormularioComponentes.BindingContext = this;
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
        
        public virtual async Task RegresarFormulario(ShellNavigationQueryParameters queryParameters = null) => await Shell.Current.GoToAsync("..", queryParameters);

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

        public async Task EliminarSpinnerDirectamente()
        {
            if (MopupService.IsSupported)
            {
                var pagina = MopupService.Instance.PopupStack.Where(item => item is VentaSpinnerLoading).ToList().First();
                await MopupService.Instance.RemovePageAsync(pagina);
            }
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
                IEnumerable<FileResultExtensible> listadoFileResult =
                    resultado.Select(item => new FileResultExtensible(item));

                //restamos cinco para ver las imagenes que faltan para completar las 5.
                int longitudImagenesTotal = 5 - FileManyResults.Count;
                int longitudAgregarImagenes = listadoFileResult.Count();
                
                if(longitudImagenesTotal < longitudAgregarImagenes)
                    listadoFileResult = listadoFileResult.Take(longitudImagenesTotal);
                    
                foreach (var file in listadoFileResult)
                    FileManyResults.Add(file);
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

        [RelayCommand(AllowConcurrentExecutions = false)]
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
