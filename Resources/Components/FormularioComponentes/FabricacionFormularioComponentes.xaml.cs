using System.Collections.ObjectModel;
using ANTU.Models.Dto;
using ANTU.Models.RequestDto;
using ANTU.ViewModel;
using Syncfusion.Maui.Data;
using Syncfusion.Maui.DataForm;

namespace ANTU.Resources.Components.FormularioComponentes;

public partial class FabricacionFormularioComponentes : ContentView
{
    private int contadorCampos = 1;

    public DataFormComboBoxItem CampoMateriaPrima = new DataFormComboBoxItem();
    public DataFormNumericItem CampoCantidadMateriaPrima = new DataFormNumericItem();
    
    public Dictionary<string, object> datosItemManager = new Dictionary<string, object>()
    {
        { "CatalogoProducto", ""},
        { "NombreProduccion", "SIN NOMBRE"},
        { "NumeroVecesProduccion", 1},
        { "MateriaPrima", ""},
        { "CantidadMateriaPrima", 0},
    };
    
    //ponemos de manera global la clase, porque luego tenemos que 
    //aprovechar "LA REFERENCIA" para ir agregando campos dinamicamente.
    DataFormGroupItem groupMateriaPrimaProducto = new DataFormGroupItem()
    {
        Name = "Materia Prima a Usar",
        ColumnCount = 2,
    };
    
    
	public FabricacionFormularioComponentes()
	{
		InitializeComponent();
        
        FormularioFabricacion.AutoGenerateItems = false;
        FormularioFabricacion.ValidationMode = DataFormValidationMode.PropertyChanged;
        FormularioFabricacion.ValidateProperty += FormularioFabricacion_OnValidateProperty;
        FormularioFabricacion.ValidateForm += FormularioFabricacion_OnValidateForm;
        FormularioFabricacion.ItemManager = new DataFormItemManagerExt(datosItemManager);
        FormularioFabricacion.Items = new ObservableCollection<DataFormViewItem>();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
    
        ConfiguracionCamposGrupoCatalogoProducto();
        ConfiguracionCamposMateriaPrimaUsar();
    }

    private void AgregarEntradasCatalogoProductoUsar_OnClicked(object? sender, EventArgs e)
    {
        if (BindingContext is FabricacionFormularioViewModel fabricacionFormularioViewModel)
        {
            if ( contadorCampos > 12 )
                return;
            
            this.CampoMateriaPrima = new DataFormComboBoxItem()
            {
                FieldName = $"MateriaPrima{contadorCampos}",
                LabelText = "M.Prima",
                ItemsSource = fabricacionFormularioViewModel.MateriaPrimaProductos,
                LayoutType = DataFormLayoutType.TextInputLayout,
                RowOrder = contadorCampos,
                ItemsOrderInRow = 0,
                SelectedValuePath = "guid",
                DisplayMemberPath = "nombreProducto"
            };
            
            this.CampoCantidadMateriaPrima = new DataFormNumericItem()
            {
                FieldName = $"CantidadMateriaPrima{contadorCampos}",
                LabelText = "Cantidad (KG)",
                LayoutType = DataFormLayoutType.TextInputLayout,
                GroupName = "Materia Prima a Usar",
                RowOrder = contadorCampos,
                ItemsOrderInRow = 1
            };
            
            FormularioFabricacion.ItemManager.SetValue(this.CampoMateriaPrima, "");
            this.groupMateriaPrimaProducto.Items.Add(this.CampoMateriaPrima);
            
            FormularioFabricacion.ItemManager.SetValue(this.CampoCantidadMateriaPrima, 0);
            this.groupMateriaPrimaProducto.Items.Add(this.CampoCantidadMateriaPrima);
            
            contadorCampos += 1;
            
        }
        
    }
       
    private void EliminarEntradasCatalogoProductoUsar_OnClicked(object? sender, EventArgs e)
    {
        if (this.groupMateriaPrimaProducto.Items.Count() == 2)
            return;
        
        var element = this.groupMateriaPrimaProducto
            .Items
            .Last();

        this.groupMateriaPrimaProducto.Items.Remove(element);

        element = this.groupMateriaPrimaProducto.Items.Last();
        
        this.groupMateriaPrimaProducto.Items.Remove(element);
    
        if(contadorCampos > 1)
            contadorCampos -= 1;
    }
  
    private void FormularioFabricacion_OnValidateProperty(object? sender, DataFormValidatePropertyEventArgs e)
    {
        
    }
    
    private void FormularioFabricacion_OnValidateForm(object? sender, DataFormValidateFormEventArgs e)
    {
       
    }
    
    private async void RegistrarFabricacion_OnClicked(object? sender, EventArgs e)
    {
        if (BindingContext is FabricacionFormularioViewModel fabricacionFormularioViewModel)
        {
            //Grupos de Items
            var grupoCatalogoProducto = FormularioFabricacion.Items.First() as DataFormGroupItem;
            
            DataFormComboBoxItem comboBoxCatalogoProducto = grupoCatalogoProducto.Items[0] as DataFormComboBoxItem;
            DataFormTextItem nombreCatalogoProducto = grupoCatalogoProducto.Items[1] as DataFormTextItem;
            DataFormNumericItem numeroVecesProduccionCatalogoProducto = grupoCatalogoProducto.Items[2] as DataFormNumericItem;

            //Obteniendo valores.
            string? idCatalogo = FormularioFabricacion.ItemManager.GetValue(comboBoxCatalogoProducto) as string;
            string? nombreProduccion = FormularioFabricacion.ItemManager.GetValue(nombreCatalogoProducto) as string;
            // int numeroVecesProduccion = (int) ( (double) FormularioFabricacion.ItemManager.GetValue(numeroVecesProduccionCatalogoProducto) );
            double numeroVecesProduccion = (double) FormularioFabricacion.ItemManager.GetValue(numeroVecesProduccionCatalogoProducto);
            
            // Grupo Materia Prima a Usar
            var grupoMateriaPrimaProducto = FormularioFabricacion.Items.Last() as DataFormGroupItem;
            
            ICollection<MaterialProduccion> listadoMaterialProduccion = new List<MaterialProduccion>();
            
            for (int i = 0; i < grupoMateriaPrimaProducto.Items.Count() ; i += 2)
            {
                DataFormComboBoxItem comboBoxMateriaPrima = grupoMateriaPrimaProducto.Items[i] as DataFormComboBoxItem;
                DataFormNumericItem cantidadAUsar = grupoMateriaPrimaProducto.Items[i+1] as DataFormNumericItem;
            
                listadoMaterialProduccion.Add(new MaterialProduccion()
                {
                    id_dto = FormularioFabricacion.ItemManager.GetValue(comboBoxMateriaPrima) as string ?? "",
                    KgUse_dto = (double)(FormularioFabricacion.ItemManager.GetValue(cantidadAUsar) ?? 0)
                });
            }
            
            await fabricacionFormularioViewModel.RegistrarDatosFabricacion(new ProduccionReqestDto()
            {
                catalogoIdentificador = idCatalogo,
                numeroVecesProduccion = (int) numeroVecesProduccion,
                nombreProduccion = nombreProduccion,
                materialesProduccion = listadoMaterialProduccion
            });
        }
        
    }
 
    
    
    // metodos de ayuda

    public void ConfiguracionCamposMateriaPrimaUsar()
    {
        //vamos a setear unos campos iniciales de materia prima a usar... luego dinamicamente se podran agregar mas campos.
        if (BindingContext is FabricacionFormularioViewModel fabricacionFormularioViewModel)
        {
            var CampoMateriaPrima = new DataFormComboBoxItem()
            {
                FieldName = "MateriaPrima",
                LabelText = "M.Prima",
                ItemsSource = fabricacionFormularioViewModel.MateriaPrimaProductos,
                LayoutType = DataFormLayoutType.TextInputLayout,
                RowOrder = 0,
                ItemsOrderInRow = 0,
                SelectedValuePath = "guid",
                DisplayMemberPath = "nombreProducto",
            };
            
            var CampoCantidadMateriaPrima = new DataFormNumericItem()
            {
                FieldName = "CantidadMateriaPrima",
                LabelText = "Cantidad (KG)",
                LayoutType = DataFormLayoutType.TextInputLayout,
                RowOrder = 0,
                ItemsOrderInRow = 1
            };
            
            this.groupMateriaPrimaProducto.Items.Add(CampoMateriaPrima);
            this.groupMateriaPrimaProducto.Items.Add(CampoCantidadMateriaPrima);
            
            //Agregando el grupo, ya agrega los campos.
            FormularioFabricacion.Items.Add(this.groupMateriaPrimaProducto);
        }
    }
    
    public void ConfiguracionCamposGrupoCatalogoProducto()
    {
        if (BindingContext is FabricacionFormularioViewModel fabricacionFormularioViewModel)
        {
            var CamposCatalogoProducto = new DataFormComboBoxItem()
            {
                FieldName = "CatalogoProducto",
                ItemsSource = fabricacionFormularioViewModel.CatalogoProductos,
                LabelText = "Producto",
                PlaceholderText = "Escoge producto que vendes",
                LayoutType = DataFormLayoutType.TextInputLayout,
                SelectedValuePath = "Identificador",
                DisplayMemberPath = "NombreProducto"
            };

            var NombreProduccion = new DataFormTextItem()
            {
                FieldName = "NombreProduccion",
                LabelText = "Nombre Produccion",
                PlaceholderText = "Nombre unico descriptivo de produccion",
                LayoutType = DataFormLayoutType.TextInputLayout,
                
            };
            
            var NumeroVecesProduccion = new DataFormNumericItem()
            {
                FieldName = "NumeroVecesProduccion",
                LabelText = "N° Veces Produccion",
                CustomFormat = "00",
                PlaceholderText = "Numero de veces a producir",
                LayoutType = DataFormLayoutType.TextInputLayout,
                UpDownPlacementMode = NumericEditorUpDownPlacementMode.Inline,
                Minimum = 1,
                AllowNull = false,
                Maximum = 20,
                ShowClearButton = true,
                IsReadOnly = false
            };
            
            DataFormGroupItem groupCatalogoProducto = new DataFormGroupItem()
            {
                Name = "Catalogo Producto",
                Items =
                {
                    CamposCatalogoProducto,
                    NombreProduccion,
                    NumeroVecesProduccion
                }
            };
            
            //Agregando el grupo, ya agrega los campos.
            FormularioFabricacion.Items.Add(groupCatalogoProducto);
        }
    }
    
}

public class DataFormItemManagerExt : DataFormItemManager
{
    Dictionary<string, object> dataFormDictionary;

    public DataFormItemManagerExt(Dictionary<string, object> datos)
    {
        dataFormDictionary = datos;
    }

    public override object GetValue(DataFormItem dataFormItem)
    {
        return dataFormDictionary[dataFormItem.FieldName];
    }

    public override void SetValue(DataFormItem dataFormItem, object value)
    {
        dataFormDictionary[dataFormItem.FieldName] = value;
    }
    
}