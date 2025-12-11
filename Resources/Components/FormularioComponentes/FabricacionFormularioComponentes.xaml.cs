using ANTU.Models.Dto;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Inputs;

namespace ANTU.Resources.Components.FormularioComponentes;

public partial class FabricacionFormularioComponentes : ContentView,  IDataFormSourceProvider
{
	public FabricacionFormulario fabricacionFormulario = new FabricacionFormulario();

	public FabricacionFormularioComponentes()
	{
		InitializeComponent();

        FormularioFabricacion.ItemsSourceProvider = this;
        FormularioFabricacion.DataObject = this.fabricacionFormulario;
        FormularioFabricacion.GenerateDataFormItem += FormularioFabricacion_OnGenerateDataFormItem;
        FormularioFabricacion.RegisterEditor("CatalogoProducto", DataFormEditorType.ComboBox);
        FormularioFabricacion.RegisterEditor("DataVenta", DataFormEditorType.ComboBox);
        FormularioFabricacion.RegisterEditor("MateriaPrima", DataFormEditorType.ComboBox);
        FormularioFabricacion.RegisterEditor("Cantidad", DataFormEditorType.Numeric);
    }

    
    public object GetSource(string sourceName)
    {
        if (sourceName == "CatalogoProducto" || sourceName == "DataVenta")
        {
            List<string> details = new List<string>()
            {
                "Elemento 1",
                "Elemento 2",
                "Elemento 3"
            };

            return details;
        }

        return new List<string>();
    }

    private void FormularioFabricacion_OnGenerateDataFormItem(object? sender, GenerateDataFormItemEventArgs e)
    {
        if (e.DataFormGroupItem is not null)
        {
            DataFormTextStyle style = new DataFormTextStyle()
            {
                TextColor = Color.FromRgb(191, 122, 36),
                FontSize = 15,
                FontAttributes = FontAttributes.Bold,
            };
            
            e.DataFormGroupItem.IsExpanded = true;
            e.DataFormGroupItem.AllowExpandCollapse = false;
            e.DataFormGroupItem.HeaderBackground = Color.FromRgba(191, 122, 36, 30);
            e.DataFormGroupItem.HeaderTextStyle = style;
        }
        
        
        if (e.DataFormItem is not null)
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
                ShowHelperText = true,
                FocusedStroke = Color.FromRgb(191, 122, 36),
                Stroke = Color.FromRgb(191, 122, 36)
            };

            
            switch (e.DataFormItem.FieldName)
            {
                case "CatalogoProducto":
                    LabelView.Text = "\uf5a4";

                    break;
                case "DataVenta":
                    LabelView.Text = "\uf039";
                    break;
            }   

            e.DataFormItem.EditorTextStyle = style;
            e.DataFormItem.LabelTextStyle = style;
            e.DataFormItem.TextInputLayoutSettings = settings;
            e.DataFormItem.LeadingView = LabelView;
        }
    }

    private void AgregarEntradasCatalogoProductoUsar_OnClicked(object? sender, EventArgs e)
    {
        FormularioFabricacion.Items.Add(new DataFormNumericItem()
        {
            FieldName = "Cantidad",
            GroupName = "Materia Prima a Usar",
        });
    }

    private void EliminarEntradasCatalogoProductoUsar_OnClicked(object? sender, EventArgs e)
    {
        throw new NotImplementedException();
    }
}

