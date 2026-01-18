using CommunityToolkit.Mvvm.ComponentModel;

namespace Modelos;

public partial class DataCatalogoProduccion : ObservableObject
{
    [ObservableProperty] 
    private string identificadorDataCatalogProducion;

    [ObservableProperty]
    private string textoDescriptivo;
}