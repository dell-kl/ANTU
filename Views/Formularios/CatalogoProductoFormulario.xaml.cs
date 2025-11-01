using ANTU.ViewModel;
using Syncfusion.Maui.DataForm;

namespace ANTU.Views.Formularios;

public partial class CatalogoProductoFormulario : ContentPage
{
    private CatalogoProductoFormularioViewModel catalogoProductoViewModel;
	public CatalogoProductoFormulario(CatalogoProductoFormularioViewModel catalogoProductoViewModel)
	{
		InitializeComponent();
        this.catalogoProductoViewModel = catalogoProductoViewModel;
        BindingContext = this.catalogoProductoViewModel;

        dataForm.DataObject = this.catalogoProductoViewModel.catalogoProductoFormulario;
        dataForm.Items.Add(new DataFormTextItem() { FieldName = "NombreProducto", GroupName = "Datos Productos" });
        dataForm.Items.Add(new DataFormNumericItem() { FieldName = "Precio", GroupName = "Datos Venta" });
        dataForm.Items.Add(new DataFormNumericItem() { FieldName = "Kg", GroupName = "Datos Venta" });
        
    }

    private void OnGenerateDataFormItem(object? sender, GenerateDataFormItemEventArgs e)
    {

        if (e.DataFormGroupItem != null) {
            if (e.DataFormGroupItem.Name == "Datos Producto" || e.DataFormGroupItem.Name == "Datos Venta")
            {
                e.DataFormGroupItem.IsExpanded = true;
                e.DataFormGroupItem.AllowExpandCollapse = false;
                e.DataFormGroupItem.HeaderBackground = Color.FromRgba(191, 122, 36, 30);

                e.DataFormGroupItem.HeaderTextStyle = new DataFormTextStyle()
                {
                    TextColor = Color.FromRgb(191, 122, 36),
                    FontSize = 15,
                    FontAttributes = FontAttributes.Bold,
                    
                };

            }
            
            //if (e.DataFormGroupItem.Name == "Datos Venta")
            //{
            //    e.DataFormGroupItem.ColumnCount = 2;
            //}

        }

        if (e.DataFormItem != null)
        {


            if (e.DataFormItem.FieldName == "NombreProducto" || e.DataFormItem.FieldName == "datosVentas.Kg" || e.DataFormItem.FieldName == "datosVentas.Precio" || e.DataFormItem.FieldName == "datosVentas.Cantidad")
            {
                e.DataFormItem.LayoutType = DataFormLayoutType.TextInputLayout;
                e.DataFormItem.LeadingViewPosition = TextInputLayoutViewPosition.Outside;
                e.DataFormItem.ShowTrailingView = true;

                e.DataFormItem.EditorTextStyle = new DataFormTextStyle()
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 15,
                    FontFamily = "SegoeUILight",
                    TextColor = Color.FromRgb(191, 11, 36)
                };
                e.DataFormItem.LabelTextStyle = new DataFormTextStyle()
                {
                    FontFamily = "SegoeUILight",
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 15,
                    TextColor = Color.FromRgb(191, 122, 36)
                };

                e.DataFormItem.TextInputLayoutSettings = new TextInputLayoutSettings()
                {
                    ShowHelperText = (e.DataFormItem.FieldName == "datosVentas.Cantidad") ? true : false,
                    FocusedStroke = Color.FromRgb(191, 122, 36),
                    Stroke = Color.FromRgb(191, 122, 36)
                };

                
                
            }

            if (e.DataFormItem.FieldName == "datosVentas.Precio")
            {
                e.DataFormItem.LeadingView = new Label
                {
                    FontFamily = "Ionicons",
                    Text = "\uf316",
                    FontSize = 32,
                    TextColor = Color.FromRgb(191, 122, 36),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };

            }

            if (e.DataFormItem.FieldName == "NombreProducto" )
            {
                e.DataFormItem.LeadingView = new Label
                {
                    FontFamily = "Ionicons",
                    Text = "\uf3ec",
                    FontSize = 32,
                    TextColor = Color.FromRgb(191, 122, 36),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };
            }

            if (e.DataFormItem.FieldName == "datosVentas.Cantidad")
            {
                e.DataFormItem.LeadingView = new Label
                {
                    FontFamily = "Ionicons",
                    Text = "\uf2a5",
                    FontSize = 28,
                    TextColor = Color.FromRgb(191, 122, 36),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };

                e.DataFormItem.EditorHeight = 100;
            }

            if (e.DataFormItem.FieldName == "datosVentas.Kg")
            {
                e.DataFormItem.LeadingView = new Label
                {
                    FontFamily = "Ionicons",
                    Text = "\uf3f2",
                    FontSize = 32,
                    TextColor = Color.FromRgb(191, 122, 36),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                };
            }

        }
    }

    protected override async void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        await this.catalogoProductoViewModel.DesmontarSpinner();
    }
}
