using ANTU.Models;
using ANTU.Models.Dto;
using ANTU.ViewModel;
using ANTU.ViewModel.PopupServicesViewModel;
using Syncfusion.Maui.DataForm;

namespace ANTU.Resources.Components.FormularioComponentes;

public partial class MateriaPrimaFormularioComponentes : ContentView
{
	private MateriaPrimaFormulario materiaPrimaFormulario = new MateriaPrimaFormulario();

    public MateriaPrimaFormularioComponentes()
	{
		InitializeComponent();

		FormularioMateriaPrima.DataObject = this.materiaPrimaFormulario;
        FormularioMateriaPrima.Items.Add(new DataFormTextItem() { FieldName = "MateriaPrima", GroupName = "Datos Producto" });
        FormularioMateriaPrima.Items.Add(new DataFormNumericItem() { FieldName = "KgStandard", GroupName = "Datos Compra" });
        FormularioMateriaPrima.Items.Add(new DataFormNumericItem() { FieldName = "Cantidad", GroupName = "Datos Compra" });
        FormularioMateriaPrima.Items.Add(new DataFormNumericItem() { FieldName = "Precio", GroupName = "Datos Compra" });
    }

    private void FormularioMateriaPrima_GenerateDataFormItem(object sender, Syncfusion.Maui.DataForm.GenerateDataFormItemEventArgs e)
    {

        if ( e.DataFormGroupItem is not null )
        {
            DataFormTextStyle style = new DataFormTextStyle()
            {
                TextColor = Color.FromRgb(191, 122, 36),
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
            };


            switch (e.DataFormGroupItem.Name)
            {
                case "Datos Producto":
                    if (BindingContext is VentanaPopupServiceViewModel viewModelPopup)
                        if (viewModelPopup.Accion is "agregar_compra")
                            e.DataFormGroupItem.IsVisible = false;
                    break;

                case "Datos Compra":
                    if (BindingContext is VentanaPopupServiceViewModel viewModelPopup2)
                        if (viewModelPopup2.Accion is "editar_materiaPrima")
                            e.DataFormGroupItem.IsVisible = false;
                    break;
            }

            e.DataFormGroupItem.IsExpanded = true;
            e.DataFormGroupItem.AllowExpandCollapse = false;
            e.DataFormGroupItem.HeaderBackground = Color.FromRgba(191, 122, 36, 30);
            e.DataFormGroupItem.HeaderTextStyle = style;
        }

        if ( e.DataFormItem is not null )
        {
            e.DataFormItem.LayoutType = DataFormLayoutType.TextInputLayout;
            e.DataFormItem.LeadingViewPosition = TextInputLayoutViewPosition.Outside;
            e.DataFormItem.ShowTrailingView = true;

            Label LabelView = new Label()
            {
                FontFamily = "MaterialSymbolSharp",
                FontSize = 32,
                TextColor = Color.FromRgb(191, 122, 36),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            };

            DataFormTextStyle style = new DataFormTextStyle()
            {
                FontAttributes = FontAttributes.Bold,
                FontSize = 15,
                FontFamily = "SegoeUILight",
                TextColor = Color.FromRgb(191, 11, 36)
            };

            TextInputLayoutSettings settings = new TextInputLayoutSettings()
            {
                ShowHelperText = false,
                FocusedStroke = Color.FromRgb(191, 122, 36),
                Stroke = Color.FromRgb(191, 122, 36)
            };

            switch (e.DataFormItem.FieldName)
            {
                case "MateriaPrima":
                    LabelView.Text = "\uf5a4";

                    break;
                case "KgStandard":
                    LabelView.Text = "\uf039";
                    break;
                case "Cantidad":
                    LabelView.Text = "\ueb8d";
                    break;
                case "Precio":
                    LabelView.Text = "\uf04a";
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
        this.materiaPrimaFormulario.MateriaPrima = (e.NewValues["MateriaPrima"] is null) ? "" : e.NewValues["MateriaPrima"].ToString();
        this.materiaPrimaFormulario.KgStandard = (e.NewValues["KgStandard"] is null) ? 0 : (double)e.NewValues["KgStandard"];
        this.materiaPrimaFormulario.Precio = (e.NewValues["Precio"] is null) ? 0 : (double)e.NewValues["Precio"];
        this.materiaPrimaFormulario.Cantidad = (e.NewValues["Cantidad"] is null) ? 0 : int.Parse(e.NewValues["Cantidad"].ToString()!);
    }

    private async void RegistrarMateriaPrima_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is VentanaPopupServiceViewModel ventanaPopupServiceViewModel) {
            //vamos a controlar que tipo de campos se va a  validar. 
            List<string> camposValidacion = new List<string>();

            if (ventanaPopupServiceViewModel.Accion is "agregar_compra")
                camposValidacion = new List<string>() { "KgStandard", "Precio", "Cantidad" };
            else if (ventanaPopupServiceViewModel.Accion is "editar_materiaPrima")
                camposValidacion = new List<string>() {  "MateriaPrima" };


            if (FormularioMateriaPrima.Validate(camposValidacion)) {
                MateriaPrimaProducto materiaPrimaProducto = (ventanaPopupServiceViewModel.DatosAdicionales as MateriaPrimaProducto)!;

                if (ventanaPopupServiceViewModel.CerrarPopupCommand.CanExecute(null) && (ventanaPopupServiceViewModel.Accion is "agregar_compra" || ventanaPopupServiceViewModel.Accion is "editar_materiaPrima"))
                {
                    //vamos a cerrar nuestro popup actual, pero enviamos los datos del formulario para que en el 
                    //CatalogoProductoDetalleViewModel se encargue de realizar la peticion de mandar a guardar.
                    await ventanaPopupServiceViewModel.CerrarPopupCommand.ExecuteAsync(new List<object>()
                    {
                        this.materiaPrimaFormulario,
                        materiaPrimaProducto.guid
                    });
                }
            }
        }

        if (FormularioMateriaPrima.Validate() && BindingContext is FormularioMateriaPrimaViewModel formularioMateriaPrimaViewModel && formularioMateriaPrimaViewModel.RegistrarMateriaPrimaCommand.CanExecute(this.materiaPrimaFormulario))
            await formularioMateriaPrimaViewModel.RegistrarMateriaPrimaCommand.ExecuteAsync(this.materiaPrimaFormulario);
    }
}