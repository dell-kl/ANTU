using Modelos;
using ANTU.Resources.Components.PopupComponents;
using Data.Rest.RestInterfaces;
using ANTU.Resources.ValueConverter;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Business.Services.IServices;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class MostrarImagenesDetalleViewModel : ParentViewModel
    {

        private string identificador = "";

        private int alturaDinamicaListaImagenes = 700;
        public int AlturaDinamicaListaImagenes { set => SetProperty(ref alturaDinamicaListaImagenes, value); get => alturaDinamicaListaImagenes; }

        [ObservableProperty]
        private bool _activarPanelNuevasImagenes = false;

        [ObservableProperty]
        private bool _activarCheckBoxEliminar = false;

        [ObservableProperty]
        private bool _activarPanelAcciones = false;

        [ObservableProperty]
        private ObservableCollection<DataImage> data = new ObservableCollection<DataImage>();
        
        [ObservableProperty]
        private object _dataModel;

        public MostrarImagenesDetalleViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService) : base(restManagement, popupService, managementService) {
            
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);
                
            if ( base.DataQuery is not null )
            {
                List<object> listobject = (base.DataQuery! as List<object>)!;
                this.identificador = listobject[1].ToString()!;
                this.DataModel = listobject[0]!;

                if (DataModel is MateriaPrimaDetalle materiaPrimaDetalle)
                {
                    this.Data = materiaPrimaDetalle.imagenes.Where(item => item.Url.Equals("default_icon.png")).Any() ? [] : materiaPrimaDetalle.imagenes;
                }
                else if (DataModel is CatalogoProductoDetalle catalogoProductoDetalle)
                {
                    this.Data = catalogoProductoDetalle.Imagenes.Where(item => item.Url.Equals("default_icon.png")).Any() ? [] : catalogoProductoDetalle.Imagenes;
                }
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
                (Func<Task>)(async () => {

                    ICollection<DataImage> ListDataImages = this.Data.Where((Func<DataImage, bool>)(item => (bool)item.Estado)).ToList();
                    bool resultado = false;

                    if(DataModel is MateriaPrimaDetalle materiaPrimaDetalle)
                        resultado = await RestManagement.MateriaPrima.DeleteImages(ListDataImages, async() => { await base.DesmontarSpinner(); });
                    else if(DataModel is CatalogoProductoDetalle catalogoProductoDetalle)
                        resultado = await RestManagement.CatalogoProduct.DeleteImages(ListDataImages, async() => { await base.DesmontarSpinner(); });

                    if (resultado)
                    {
                        //tenemos que quitar manualmente de la coleccion en caso de que si se hayan eliminado exitosamente en el servidor.
                        this.Data = this.Data.Where((Func<DataImage, bool>)(item => (bool)!item.Estado)).ToObservableCollection();

                        if (DataModel is MateriaPrimaDetalle modelo1)
                            modelo1.imagenes = this.Data;
                        else if (DataModel is CatalogoProductoDetalle modelo2)
                            modelo2.Imagenes = this.Data;
                    }
                })
            );

        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task SeleccionarArchivos()
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

                       Dictionary<string, object> resultado = new Dictionary<string, object>();

                       if(DataModel is MateriaPrimaDetalle materiaPrimaDetalle)
                            resultado = await RestManagement.MateriaPrima.SaveImages(FileManyResults, this.identificador, true, async() => { await base.DesmontarSpinner(); });
                       else if(DataModel is CatalogoProductoDetalle catalogoProductoDetalle)
                            resultado = await RestManagement.CatalogoProduct.SaveImages(FileManyResults, this.identificador, true, async() => { await base.DesmontarSpinner(); });

                       if ((bool)resultado["estado"])
                       {
                           FileManyResults.Clear();
                           ActivarPanelNuevasImagenes = false;
                           AlturaDinamicaListaImagenes = 700;

                           ICollection<DataImage> data = (resultado["imagenes"] as ICollection<DataImage>)!;
                           this.Data = this.Data.Union(data).ToObservableCollection();

                           if (DataModel is MateriaPrimaDetalle modelo1)
                               modelo1.imagenes = this.Data;
                           else if (DataModel is CatalogoProductoDetalle modelo2)
                               modelo2.Imagenes = this.Data;
                       }

                   }
                );

            }
        }

    }
}
