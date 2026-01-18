using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace ANTU.ViewModel.PopupServicesViewModel;

public partial class VentanaEmergenteSinConexionViewModel
{
    public VentanaEmergenteSinConexionViewModel( ) 
    {
        
    }

    [RelayCommand(AllowConcurrentExecutions = false)]
    public async Task ReintentarPeticion()
    {
        await MopupService.Instance.PopAllAsync();
        await Shell.Current.GoToAsync("../");
        // await base.MostrarSpinner();
        // await base.cargaProductos();
    }
}