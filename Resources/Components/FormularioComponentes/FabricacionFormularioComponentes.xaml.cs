using ANTU.Models.Dto;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Inputs;

namespace ANTU.Resources.Components.FormularioComponentes;

public partial class FabricacionFormularioComponentes : ContentView
{
	public FabricacionFormulario fabricacionFormulario = new FabricacionFormulario();

	public FabricacionFormularioComponentes()
	{
		InitializeComponent();

        FormularioFabricacion.DataObject = this.fabricacionFormulario;
        configurarGrupoCatalogoProducto();
        // FormularioFabricacion.Items.Add(personalizacionCatalogProductos("CatalogoProducto", new List<string>()
        // {
        //     "Patatas",
        //     "Carne",
        //     "Cafe"
        // }, "\uf801"));
        //
        // FormularioFabricacion.Items.Add(personalizacionCatalogProductos("DataVenta", new List<string>()
        // {
        //     "Patatas",
        //     "Carne",
        //     "Cafe"
        // }, "\uefa9"));
     
        
    }

    public void configurarGrupoCatalogoProducto()
    {
        FormularioFabricacion.AutoGenerateItems = true;
        FormularioFabricacion.GenerateDataFormItem += (sender, args) =>
        {
            if (args.DataFormItem is DataFormComboBoxItem item)
            {
                
            }   
            
            // if (args.DataFormItem.FieldName.Equals("CatalogoProducto") || args.DataFormItem.FieldName.Equals("DataVenta"))
            //     args.DataFormItem.SetValue( args.DataFormItem as DataFormComboBoxItem );
            //
            // if (args.DataFormItem is DataFormComboBoxItem comboBoxItem)
            // {
            //     comboBoxItem.ItemsSource = new List<string>()
            //     {
            //         "Editable"
            //     };
            // }
        };

    }
    

    public DataFormComboBoxItem personalizacionCatalogProductos(string fieldName, List<string> items, string icono)
    {
        
        
        return new DataFormComboBoxItem()
        {
            FieldName = fieldName,
            ItemsSource = items,
            IsEditable = true,
            LayoutType = DataFormLayoutType.TextInputLayout,
            LeadingViewPosition = TextInputLayoutViewPosition.Outside,
            ShowTrailingView = true,
            LeadingView = new Label()
            {
                FontFamily = "MaterialSymbolSharp",
                FontSize = 32,
                TextColor = Color.FromRgb(191, 122, 36),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Text = icono
            },
            GroupName = "CatalogoProducto",
        };
    }
}

