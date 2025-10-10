using ANTU.ViewModel;

namespace ANTU.Views.dashboard;

public partial class Dashboard : ContentPage
{
	public Dashboard(DashboardViewModel dashboardViewModel)
	{
		InitializeComponent();

		BindingContext = dashboardViewModel;
	}

}