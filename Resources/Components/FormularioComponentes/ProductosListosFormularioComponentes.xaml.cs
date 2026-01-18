using Modelos;
using ANTU.ViewModel;
using Syncfusion.Maui.DataForm;
using Syncfusion.Maui.Inputs;

namespace ANTU.Resources.Components.FormularioComponentes;

public partial class ProductosListosFormularioComponentes : ContentView
{
    public ProductosListosFormularioComponentes()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (BindingContext is ProductoListoDetalleViewModel item)
        {
            FormularioProductosListos.DataObject = item.ProductosListosFormulario;
            FormularioProductosListos.RegisterEditor("DatosVenta", new ComboBoxEditor(FormularioProductosListos, this));
            FormularioProductosListos.Items.Add(new DataFormNumericItem(){ FieldName = "NumeroCostales" });
        }
    }

    private void FormularioProductosListos_OnGenerateDataFormItem(object? sender, GenerateDataFormItemEventArgs e)
    {
        if(e.DataFormGroupItem != null)
            switch (e.DataFormGroupItem.Name)
            {
                case "Ingresar costales a Bodega":
                    DataFormTextStyle style = new DataFormTextStyle()
                    {
                        TextColor = Color.FromRgb(191, 122, 36),
                        FontSize = 15,
                        FontAttributes = FontAttributes.Bold,
                    };
                    e.DataFormGroupItem.IsExpanded = true;
                    e.DataFormGroupItem.AllowExpandCollapse = false;
                    e.DataFormGroupItem.HeaderBackground = Color.FromRgba(191, 122, 36, 40);
                    e.DataFormGroupItem.HeaderTextStyle = style;
                    break;
            }

        if (e.DataFormItem != null)
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
                case "NumeroCostales":
                    LabelView.Text = "\uf5a4";
                    break;
                case "DatosVenta":
                    LabelView.Text = "\uf5a4";
                    break;
            }
            e.DataFormItem.EditorTextStyle = style;
            e.DataFormItem.LabelTextStyle = style;
            e.DataFormItem.TextInputLayoutSettings = settings;
            e.DataFormItem.LeadingView = LabelView;
        }
    }

    private async void RegistrarDatosProductoBodega_OnClicked(object? sender, EventArgs e)
    {
        if (FormularioProductosListos.Validate() && BindingContext is ProductoListoDetalleViewModel item && item.RegistrarProductosABodegaCommand.CanExecute(this))
            await item.RegistrarProductosABodegaCommand.ExecuteAsync(this);
    }

    private void FormularioProductosListos_OnValidateForm(object? sender, DataFormValidateFormEventArgs e)
    {
        
    }
}

//Editor ComoBox...
public class ComboBoxEditor : IDataFormEditor
{
    private SfDataForm dataForm;
    private ProductosListosFormularioComponentes item;
    private DataFormCustomItem? dataFormCustomItem;
    
    public ComboBoxEditor(SfDataForm dataForm, ProductosListosFormularioComponentes item)
    {
        this.dataForm = dataForm;
        this.item = item;
    }

    public View CreateEditorView(DataFormItem dataFormItem)
    {
        SfComboBox comboBox = new SfComboBox();
        DataFormTextStyle textStyle = this.dataForm.EditorTextStyle;
        
        comboBox.ItemsSource = (item.BindingContext as ProductoListoDetalleViewModel)!.ListadoDataCatalogoProduccions;
        comboBox.DisplayMemberPath = "TextoDescriptivo";
        comboBox.SelectedValuePath = "IdentificadorDataCatalogProducion";
        comboBox.MaximumSuggestion = 10;
        comboBox.IsEditable = false;
        comboBox.IsFilteringEnabled = true;
        comboBox.TextColor = textStyle.TextColor;
        comboBox.FontFamily = textStyle.FontFamily;
        comboBox.FontAttributes = textStyle.FontAttributes;
        comboBox.FontSize = textStyle.FontSize;
        comboBox.MaxDropDownHeight = 200;
        comboBox.SelectionMode = ComboBoxSelectionMode.Single;
        comboBox.SelectionChanging += SeleccionandoElemento;
        comboBox.LoadMoreButtonTapped += ComboBox_LoadMoreButtonTapped;
        
        this.dataFormCustomItem = (DataFormCustomItem)dataFormItem;
        this.dataFormCustomItem.EditorValue = string.Empty;
        
        return comboBox;
    }

    private void SeleccionandoElemento(object? sender, SelectionChangingEventArgs e)
    {
        string valor = e.CurrentSelection!.Any()
            ? (e.CurrentSelection!.First() as DataCatalogoProduccion)!.IdentificadorDataCatalogProducion
            : "";
        this.dataFormCustomItem!.EditorValue = valor; //Setemos EditorValue (se usa para hacer validacion manual)
        this.ValidateValue(dataFormCustomItem!);
        this.dataFormCustomItem!.SetValue(valor); // Para setear en nuestro atributo "IdentificadorDataCatalogProducion" del DataObject
    }

    private async void ComboBox_LoadMoreButtonTapped(object? sender, EventArgs e)
    {
        SfComboBox itemView = (sender as SfComboBox)!;
        
        if (this.item.BindingContext is ProductoListoDetalleViewModel itemViewModel)
        {
            await itemViewModel.TraerDatosDeVenta();
            itemView.ItemsSource = itemViewModel.ListadoDataCatalogoProduccions; 
        }
    }
    
    private void ValidateValue(DataFormItem dataFormItem)
    {
        this.dataForm.Validate(new List<string>() { dataFormItem.FieldName });
    }
    
    public void CommitValue(DataFormItem dataFormItem, View view)
    {
    }

    public void UpdateReadyOnly(DataFormItem dataFormItem)
    {
        throw new NotImplementedException();
    }
}