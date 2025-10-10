using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Resources.Components.PopupComponents;
using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;
using Syncfusion.Maui.DataForm;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace ANTU.ViewModel
{
    public partial class MateriaPrimaDetalleViewModel : ParentViewModel
    {
        private MateriaPrimaProducto materiaPrimaProducto = new MateriaPrimaProducto(0);
        public MateriaPrimaProducto MateriaPrimaProducto { set => SetProperty(ref materiaPrimaProducto, value); get => materiaPrimaProducto; }

        private MateriaPrimaDetalle materiaPrimaDetalle = new MateriaPrimaDetalle();
        public MateriaPrimaDetalle MateriaPrimaDetalle { set => SetProperty(ref materiaPrimaDetalle, value); get => materiaPrimaDetalle; }

        private MateriaPrimaDetalleFormulario materiaPrimaDetalleFormulario = new MateriaPrimaDetalleFormulario();
        public MateriaPrimaDetalleFormulario MateriaPrimaDetalleFormulario { set => SetProperty(ref materiaPrimaDetalleFormulario, value); get => materiaPrimaDetalleFormulario; }

        public MateriaPrimaDetalleViewModel(IRestManagement IRestManagement) : base(IRestManagement) {
        }

        public override void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            base.ApplyQueryAttributes(query);

            if ( base.DataQuery is not null )
                this.MateriaPrimaProducto = (base.DataQuery as MateriaPrimaProducto)!;
        }

        public async Task cargarDatosMateriaPrimaDetalle()
        {
            this.MateriaPrimaDetalle = new MateriaPrimaDetalle()
            {
                KgTotal = 130.50,
                TotalCompras = 20,
                UltimaCompra = 20.50m,
                KgSeguimiento = new ObservableCollection<KgSeguimiento>() { 
                    new KgSeguimiento() { 
                        fechaCompra = DateTime.Now,
                        precioTotal = 40,
                        KgTotal = 200,
                        precioUnitario = 20,
                        KgEstandar = 100
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 50,
                        KgTotal = 100,
                        precioUnitario = 10,
                        KgEstandar = 150
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 50,
                        KgTotal = 100,
                        precioUnitario = 25,
                        KgEstandar = 200
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 80,
                        KgTotal = 250,
                        precioUnitario = 50,
                        KgEstandar = 100
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 80,
                        KgTotal = 250,
                        precioUnitario = 50,
                        KgEstandar = 100
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 80,
                        KgTotal = 250,
                        precioUnitario = 50,
                        KgEstandar = 100
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 80,
                        KgTotal = 250,
                        precioUnitario = 50,
                        KgEstandar = 100
                    },
                                        new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 40,
                        KgTotal = 200,
                        precioUnitario = 20,
                        KgEstandar = 100
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 50,
                        KgTotal = 100,
                        precioUnitario = 10,
                        KgEstandar = 150
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 50,
                        KgTotal = 100,
                        precioUnitario = 25,
                        KgEstandar = 200
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 80,
                        KgTotal = 250,
                        precioUnitario = 50,
                        KgEstandar = 100
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 80,
                        KgTotal = 250,
                        precioUnitario = 50,
                        KgEstandar = 100
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 80,
                        KgTotal = 250,
                        precioUnitario = 50,
                        KgEstandar = 100
                    },
                    new KgSeguimiento() {
                        fechaCompra = DateTime.Now,
                        precioTotal = 80,
                        KgTotal = 250,
                        precioUnitario = 50,
                        KgEstandar = 100
                    },
                }
            };
        }

       

        [RelayCommand(AllowConcurrentExecutions = false)]
        public async Task VentanaFormularioMateriaPrimaDetalleStock(object data)
        {
            FormularioEmergente formulario = new FormularioEmergente();
            formulario.FindByName<SfDataForm>("Formulario").DataObject = this.MateriaPrimaDetalleFormulario;
            formulario.FindByName<SfDataForm>("Formulario").ValidateForm += (sender, e) => {
                this.MateriaPrimaDetalleFormulario.Cantidad = e.NewValues["Cantidad"] is null ? 0 : int.Parse(e.NewValues["Cantidad"].ToString()!);
                this.MateriaPrimaDetalleFormulario.KGStandard = e.NewValues["KGStandard"] is null ? 0.0d : double.Parse(e.NewValues["KGStandard"].ToString()!);
                this.MateriaPrimaDetalleFormulario.PrecioUnitario = e.NewValues["PrecioUnitario"] is null ? 0.0d : double.Parse(e.NewValues["PrecioUnitario"].ToString()!);
            };
            formulario.FindByName<Button>("BotonConfirmarFormulario").Clicked += async (sender, e) => {
                SfDataForm dataForm = formulario.FindByName<SfDataForm>("Formulario");

                if (dataForm.Validate())
                {
                    bool respuesta = MopupService.Instance.PopupStack.Where(item => item is VentanaConfirmacionEmergente).Any();
                    MateriaPrimaDetalleFormulario datosFormulario = (dataForm.DataObject as MateriaPrimaDetalleFormulario)!;

                    if ( !respuesta )
                    {
                        VentanaConfirmacionEmergente ventanaConfirmacion = new VentanaConfirmacionEmergente();

                        //ventanaConfirmacion.FindByName<Button>("BotonConfirmacion").Command = ;

                        //ventanaConfirmacion.FindByName<Button>("BotonConfirmacion").Clicked += (sender, e) => {
                        //    ventanaConfirmacion.FindByName<Button>("BotonConfirmacion").IsEnabled = false;
                        //    ventanaConfirmacion.FindByName<Button>("BotonConfirmacion").IsVisible = false;

                        //    ventanaConfirmacion.FindByName<Button>("BotonRegresar").IsEnabled = false;
                        //    ventanaConfirmacion.FindByName<Button>("BotonRegresar").IsVisible = false;

                        //    ventanaConfirmacion.FindByName<Border>("MensajeCargando").IsVisible = true;

                        //};
                        await MopupService.Instance.PushAsync(ventanaConfirmacion);
                    }
                
                }
            };
            await MopupService.Instance.PushAsync(formulario);
        }
    }
}
