using System.Collections.ObjectModel;
using System.Runtime.Versioning;
using Modelos;
using Modelos.RequestDto;
using ANTU.Resources.Components.FormularioComponentes;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Data;

namespace ANTU.ViewModel
{
    [SupportedOSPlatform("Android")]
    public partial class FabricacionFormularioViewModel : ParentViewModel
    {
        [ObservableProperty]
        private ObservableCollection<CatalogoProducto> catalogoProductos = new ObservableCollection<CatalogoProducto>();

        [ObservableProperty]
        private ObservableCollection<MateriaPrimaProducto> materiaPrimaProductos = new ObservableCollection<MateriaPrimaProducto>();
        
        [ObservableProperty]
        private FabricacionFormularioComponentes fabricacionFormularioComponentes = new FabricacionFormularioComponentes();

        public FabricacionFormularioViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService) : base(restManagement, popupService, managementService) {
        }

        public async Task RegistrarDatosFabricacion(ProduccionReqestDto produccionReqestDto)
        {
            await base.MostrarSpinner();
            await RestManagement.Produccion.Add(produccionReqestDto, () => base.DesmontarSpinner(), true);
        }
        
        public async Task ObtenerDatosCatalogoProducto()
        {
            if (this.CatalogoProductos.Count() == 0 || this.CatalogoProductos.Count() >= 10)
            {
                IEnumerable<CatalogoProducto> listadoCatalogo = await RestManagement.CatalogoProduct.Get(this.CatalogoProductos.Count().ToString());
                
                if (listadoCatalogo.Any())
                    this.CatalogoProductos = this.CatalogoProductos.Union(listadoCatalogo).ToObservableCollection();
            }
        }

        public async Task ObtenerDatosMateriaPrimaProducto()
        {
            if ( this.MateriaPrimaProductos.Count() == 0 || this.MateriaPrimaProductos.Count() >= 10)
            {
                IEnumerable<MateriaPrimaProducto> listadoMateriaPrima = await RestManagement.MateriaPrima.Get(this.MateriaPrimaProductos.Count().ToString());

                if (listadoMateriaPrima.Any())
                    this.MateriaPrimaProductos = this.MateriaPrimaProductos.Union(listadoMateriaPrima).ToObservableCollection();
            }
        }
    }
}
