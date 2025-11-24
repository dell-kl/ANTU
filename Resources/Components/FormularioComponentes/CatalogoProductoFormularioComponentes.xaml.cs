using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.Resources.Rest.RestInterfaces;
using ANTU.ViewModel;
using ANTU.ViewModel.PopupServicesViewModel;
using CommunityToolkit.Maui.Alerts;
using Syncfusion.Maui.DataForm;

namespace ANTU.Resources.Components.FormularioComponentes;

public partial class CatalogoProductoFormularioComponentes : ContentView
{
    private CatalogoProductoFormulario catalogoProductoFormulario;

	public CatalogoProductoFormularioComponentes()
	{
		InitializeComponent();

        this.catalogoProductoFormulario = new CatalogoProductoFormulario();

        FormularioCatalogoProducto.DataObject = this.catalogoProductoFormulario;
        FormularioCatalogoProducto.Items.Add(new DataFormTextItem() { FieldName = "NombreProducto", GroupName = "Datos Productos" });
        FormularioCatalogoProducto.Items.Add(new DataFormNumericItem() { FieldName = "Precio", GroupName = "Datos Venta" });
        FormularioCatalogoProducto.Items.Add(new DataFormNumericItem() { FieldName = "Kg", GroupName = "Datos Venta" });
    }


    private void OnGenerateDataFormItem(object? sender, GenerateDataFormItemEventArgs e)
    {
        
        if (e.DataFormGroupItem != null)
        {
        
            DataFormTextStyle style = new DataFormTextStyle() {
                TextColor = Color.FromRgb(191, 122, 36),
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
            };

            switch(e.DataFormGroupItem.Name)
            {
                case "Datos Producto":
                    if(BindingContext is VentanaPopupServiceViewModel viewModelPopup) 
                        if(viewModelPopup.Accion is "agregar_datosVenta")
                            e.DataFormGroupItem.IsVisible = false;
                   
                    break;

                case "Datos Venta":
                    if (BindingContext is VentanaPopupServiceViewModel viewModelPopup2)
                        if (viewModelPopup2.Accion is "editar_catalogProducto")
                            e.DataFormGroupItem.IsVisible = false;
                    break;
            }

            e.DataFormGroupItem.IsExpanded = true;
            e.DataFormGroupItem.AllowExpandCollapse = false;
            e.DataFormGroupItem.HeaderBackground = Color.FromRgba(191, 122, 36, 30);
            e.DataFormGroupItem.HeaderTextStyle = style;
        }

        if (e.DataFormItem != null)
        {

            e.DataFormItem.LayoutType = DataFormLayoutType.TextInputLayout;
            e.DataFormItem.LeadingViewPosition = TextInputLayoutViewPosition.Outside;
            e.DataFormItem.ShowTrailingView = true;

            Label LabelView = new Label() {
                FontFamily = "Ionicons",
                FontSize = 32,
                TextColor = Color.FromRgb(191, 122, 36),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            DataFormTextStyle style = new DataFormTextStyle() {
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                FontFamily = "SegoeUILight",
                TextColor = Color.FromRgb(191, 11, 36)
            };

            TextInputLayoutSettings settings = new TextInputLayoutSettings() {
                ShowHelperText = false,
                FocusedStroke = Color.FromRgb(191, 122, 36),
                Stroke = Color.FromRgb(191, 122, 36)
            };


            switch(e.DataFormItem.FieldName)
            {
                case "NombreProducto":
                    LabelView.Text = "\uf3ec";
                    break;
                case "DatosVentas.Precio":
                    LabelView.Text = "\uf316";
                    break;
                case "DatosVentas.Kg":
                    LabelView.Text = "\uf3f2";
                    break;
                case "DatosVentas.Cantidad":
                    LabelView.Text = "\uf2a5";
                    settings.ShowHelperText = true;
                    e.DataFormItem.EditorHeight = 130;
                    break;
            }

            e.DataFormItem.EditorTextStyle = style;
            e.DataFormItem.LabelTextStyle = style;
            e.DataFormItem.TextInputLayoutSettings = settings;
            e.DataFormItem.LeadingView = LabelView;
            

        }

    }

    private void dataForm_ValidateForm(object sender, DataFormValidateFormEventArgs e)
    {
        this.catalogoProductoFormulario.NombreProducto = (e.NewValues["NombreProducto"] is null) ? "" : e.NewValues["NombreProducto"].ToString();
        this.catalogoProductoFormulario.DatosVentas.Precio = (e.NewValues["DatosVentas.Precio"] is null) ? 0 : (double)e.NewValues["DatosVentas.Precio"];
        this.catalogoProductoFormulario.DatosVentas.Kg = (e.NewValues["DatosVentas.Kg"] is null) ? 0 : (double)e.NewValues["DatosVentas.Kg"];
        this.catalogoProductoFormulario.DatosVentas.Cantidad = (e.NewValues["DatosVentas.Cantidad"] is null) ? 0 : int.Parse(e.NewValues["DatosVentas.Cantidad"].ToString()!);
    }


    private async void RegistrarDatosCatalogoProducto_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is VentanaPopupServiceViewModel viewModelPopup) {

            //vamos a controlar que campos tenemos que validar.
            List<string> camposValidacion = new List<string>();
            if (viewModelPopup.Accion is "agregar_datosVenta")
                camposValidacion = new List<string>() { "DatosVentas.Precio", "DatosVentas.Kg", "DatosVentas.Cantidad" };
            else if(viewModelPopup.Accion is "editar_catalogProducto")
                camposValidacion = new List<string>() { "NombreProducto" };

            if (FormularioCatalogoProducto.Validate(camposValidacion))
            {
                CatalogoProducto datos = (viewModelPopup.DatosAdicionales as CatalogoProducto)!;

                if (viewModelPopup.CerrarPopupCommand.CanExecute(null) && (viewModelPopup.Accion is "agregar_datosVenta" || viewModelPopup.Accion is "editar_catalogProducto"))
                {
                    //vamos a cerrar nuestro popup actual, pero enviamos los datos del formulario para que en el 
                    //CatalogoProductoDetalleViewModel se encargue de realizar la peticion de mandar a guardar.
                    await viewModelPopup.CerrarPopupCommand.ExecuteAsync(new List<object>()
                    {
                        this.catalogoProductoFormulario ,
                        datos.Identificador
                    });
                }
            }

        }

        if(FormularioCatalogoProducto.Validate() && BindingContext is CatalogoProductoFormularioViewModel viewModel && viewModel.RegistarCatalogoProductoCommand.CanExecute(null))
            await viewModel.RegistarCatalogoProductoCommand.ExecuteAsync(this.catalogoProductoFormulario);
    }
}