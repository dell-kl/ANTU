using ANTU.Models;
using ANTU.Resources.Components.PopupComponents;
using ANTU.Resources.Rest.RestInterfaces;
using ANTU.Resources.ValueConverter;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using System.Collections.ObjectModel;

namespace ANTU.ViewModel
{
    public partial class MostrarImagenesDetalleViewModel : ParentViewModel
    {

        private string identificadorMateriaPrima = "";

        private int alturaDinamicaListaImagenes = 700;
        public int AlturaDinamicaListaImagenes { set => SetProperty(ref alturaDinamicaListaImagenes, value); get => alturaDinamicaListaImagenes; }


        private bool activarPanelNuevasImagenes = false;
        public bool ActivarPanelNuevasImagenes { set => SetProperty(ref activarPanelNuevasImagenes, value); get => activarPanelNuevasImagenes; }

        private bool activarCheckBoxEliminar = false;
        public bool ActivarCheckBoxEliminar { set => SetProperty(ref activarCheckBoxEliminar, value); get => activarCheckBoxEliminar; }

        private bool activarPanelAcciones = false;
        public bool ActivarPanelAcciones { set => SetProperty(ref activarPanelAcciones, value); get => activarPanelAcciones; }


        private ObservableCollection<DataImage> data = new ObservableCollection<DataImage>();
        public ObservableCollection<DataImage> Data { set => SetProperty(ref data, value); get => data; }


        public MostrarImagenesDetalleViewModel(IRestManagement restManagement) : base(restManagement) {
           
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);
                
            if ( base.DataQuery is not null )
            {
                object[] listobject = (base.DataQuery as object[])!;
                var listadoDatos = (ObservableCollection<DataImage>)listobject[0];
                Data = listadoDatos.Where(item => item.Url.Equals("default_icon.png")).Any() ? [] : listadoDatos;
                this.identificadorMateriaPrima = listobject[1].ToString()!;
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task SeleccionarOpcionBarraHerramientas()
        {
            await base.MostrarVentanaConfirmacion(
                "Proceso Eliminar",
                "Estas completamente seguro en querer eliminar las imagenes",
                "Eliminando, espere...",
                "Cancelar",
                "Continuar",
                async () => {

                    ICollection<DataImage> ListDataImages = this.Data.Where(item => item.Estado).ToList();

                    bool resultado = await _restManagement.MateriaPrima.DeleteImages(ListDataImages, async() => { await base.DesmontarSpinner(); });


                    if (resultado) {
                        //tenemos que quitar manualmente de la coleccion en caso de que si se hayan eliminado exitosamente en el servidor.
                        this.Data = this.Data.Where(item => !item.Estado).ToObservableCollection();
                    }
                }
            );

        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public override async Task SeleccionarArchivoMostrar()
        {
            await base.SeleccionarArchivoMostrar();

            if (FileManyResults.Any())
            {
                ActivarPanelNuevasImagenes = true;
                AlturaDinamicaListaImagenes = 500;
            }
        }

        public override void EliminarArchivo(string codigo)
        {
            base.EliminarArchivo(codigo);

            if (!FileManyResults.Any())
            {
                ActivarPanelNuevasImagenes = false;
                AlturaDinamicaListaImagenes = 700;
            }
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task GuardarNuevasImagenes()
        {
            if ( FileManyResults.Any() )
            {
                await base.MostrarVentanaConfirmacion(
                   "Registrar Imagenes",
                   "Deseas continuar con el registro de las nuevas imagenes?",
                   "Registrar, espere...",
                   "Cancelar",
                   "Continuar",
                   async () => {

                       Dictionary<string, object> resultado = await _restManagement.MateriaPrima.SaveImages(FileManyResults, this.identificadorMateriaPrima, true, async() => { await base.DesmontarSpinner(); });

                       if ((bool)resultado["estado"])
                       {
                           FileManyResults.Clear();
                           ActivarPanelNuevasImagenes = false;
                           AlturaDinamicaListaImagenes = 700;

                           ICollection<DataImage> data = (resultado["imagenes"] as ICollection<DataImage>)!;
                           this.Data = this.Data.Union(data).ToObservableCollection();
                       }

                   }
                );

            }
        }
    }
}
