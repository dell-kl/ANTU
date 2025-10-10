using ANTU.Models.Dto;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using Mopups.Services;
using System.Collections.ObjectModel;
using ANTU.Resources.Components.PopupComponents;

namespace ANTU.ViewModel
{
    public partial class ParentViewModel : ObservableObject, IQueryAttributable
    {
        private object dataQuery;
        public object DataQuery { set => SetProperty(ref dataQuery, value); get => dataQuery; }

        private ObservableCollection<FileResultExtensible> fileManyResults = new ObservableCollection<FileResultExtensible>();
        public ObservableCollection<FileResultExtensible> FileManyResults { set => SetProperty(ref fileManyResults, value); get => fileManyResults; }

        protected readonly IRestManagement _restManagement;
        public ParentViewModel(IRestManagement restManagement) {
            _restManagement = restManagement;
        }

        //Navigate

        public virtual async Task NavegarFormulario(string objeto)
        {
            await MopupService.Instance.PushAsync(new VentanaEmergente(
                "⚠️ Cargando Pagina",
                "Estamos preparando todo el contenido, espera por favor 💻",
            true));

            await Shell.Current.GoToAsync(objeto);
        }

        public virtual async Task NavegarFormulario(string objeto, ShellNavigationQueryParameters queryParameters = null)
        {
            await MopupService.Instance.PushAsync(new VentanaEmergente(
                "⚠️ Cargando Pagina",
                "Estamos preparando todo el contenido, espera por favor 💻",
            true));

            await Shell.Current.GoToAsync(objeto, queryParameters);
        }


        public virtual async Task DesmontarSpinner() => await MopupService.Instance.PopAsync();

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

        public void EliminarArchivo(string codigo)
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

        public virtual async Task EjecutarFormularioEmergente()
        {
            Console.WriteLine("conjunto de datos");
        }
    }
}
