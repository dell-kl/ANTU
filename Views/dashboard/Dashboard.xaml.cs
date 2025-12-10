using ANTU.ViewModel;

namespace ANTU.Views.dashboard;

public partial class Dashboard : ContentPage
{
	private DashboardViewModel dashboardViewModel;

	public Dashboard(DashboardViewModel dashboardViewModel)
	{
		InitializeComponent();
		this.dashboardViewModel = dashboardViewModel;
		BindingContext = this.dashboardViewModel;
	}

	protected override bool OnBackButtonPressed()
	{
		return this.dashboardViewModel.ControlarNavegacion();
	}
}