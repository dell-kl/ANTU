using Modelos.Dto;
using Modelos.RequestDto;
using ANTU.Resources.Components.FormularioComponentes;
using Business.Services.IServices;
using Data.Rest.RestInterfaces;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ANTU.ViewModel
{
    public partial class CatalogoProductoFormularioViewModel : ParentViewModel
    {
        //public CatalogoProductoFormulario catalogoProductoFormulario { set; get; } = new CatalogoProductoFormulario();

        [ObservableProperty]
        private CatalogoProductoFormularioComponentes catalogoProductoFormularioComponenets = new CatalogoProductoFormularioComponentes();

        public CatalogoProductoFormularioViewModel(IRestManagement restManagement, IPopupService popupService, IManagementService managementService) : base(restManagement, popupService, managementService)
        {
            catalogoProductoFormularioComponenets.BindingContext = this;
        }

        //[RelayCommand(AllowConcurrentExecutions = false)]
        //public override async Task SeleccionarArchivoMostrar()
        //{
        //    await base.SeleccionarArchivoMostrar();
        //}

        //este codigo de aqui tiene que ver con la parte de nuestras imagenes, el 
        //boton con la funcionalidad de eliminar la imagen de la lista 
        //este metodo es de prueba solamente, mas adelante hay que reemplazarlo.
        [RelayCommand(AllowConcurrentExecutions = false)]
        public override async Task EliminarArchivoPrueba(string codigo)
        {
            await base.EliminarArchivoPrueba(codigo);
        }

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task RegistarCatalogoProducto(CatalogoProductoFormulario catalogoProductoFormulario)
        {
            await MostrarSpinner();
            bool resultado = await _restManagement.CatalogoProduct.Add(
                new Modelos.RequestDto.CatalogoProductoRequestDto()
                {
                    identificador = Guid.NewGuid().ToString(),
                    nombreProducto = catalogoProductoFormulario.NombreProducto!,
                    dataCatalogProducts = new List<DataProduct>()
                    {
                        new DataProduct()
                        {
                            precio = (decimal) catalogoProductoFormulario.DatosVentas.Precio,
                            pesoKg = catalogoProductoFormulario.DatosVentas.Kg,
                            cantidadTotal = catalogoProductoFormulario.DatosVentas.Cantidad
                        }
                    }
                },
                () => DesmontarSpinner(),
                FileManyResults
            );

            catalogoProductoFormulario.limpiarDatos();
            FileManyResults.Clear();
        }
    }
}
