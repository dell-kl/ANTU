using CommunityToolkit.Mvvm.ComponentModel;

namespace ANTU.Models;

public partial class DataCatalogoProduccion : ObservableObject
{
    [ObservableProperty] 
    private string identificadorDataCatalogProducion;

    [ObservableProperty]
    private string textoDescriptivo;
}