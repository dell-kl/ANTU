using Modelos;
using ANTU.ViewModel;
using Syncfusion.Maui.ListView;

namespace ANTU.Resources.Components.ControlersComponents;

public partial class PanelComponents : ContentView
{
	public PanelComponents()
	{
		InitializeComponent();
	}

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
    }

    private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if ( BindingContext is MateriaPrimaViewModel materiaPrimaViewModel)
        {
            var searchBar = (sender as SearchBar);
            SfListView? ListadoDatos = null;

            if (materiaPrimaViewModel.DataQuery.Equals("MateriaPrima"))
                ListadoDatos = materiaPrimaViewModel.MateriaPrimaView.FindByName<SfListView>("ListadoMateriaPrima");

            if (ListadoDatos?.DataSource is not null)
            {
                ListadoDatos.DataSource!.Filter = (object obj) =>
                {
                    if (searchBar == null || searchBar.Text == null)
                        return true;

                    if (materiaPrimaViewModel.DataQuery.Equals("MateriaPrima"))
                    {
                        var taskInfo = obj as MateriaPrimaProducto;
                        if (taskInfo!.nombreProducto.ToLower().Contains(searchBar.Text.ToLower()))
                            return true;
                    }


                    return false;
                };
                ListadoDatos.DataSource!.RefreshFilter();
            }
        }

    }
}