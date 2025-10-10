using ANTU.Resources.Rest.RestInterfaces;
using CommunityToolkit.Mvvm.Input;
using Mopups.Services;

namespace ANTU.ViewModel
{
    public partial class DashboardViewModel : ParentViewModel
    {

        public DashboardViewModel(IRestManagement IRestManagement) : base(IRestManagement) { 
        
        }

        private bool buttonOption = true;
        public bool ButtonOption { set => SetProperty(ref buttonOption, value); get => buttonOption; }


        [RelayCommand(AllowConcurrentExecutions = false)]
        public override Task NavegarFormulario(string objeto)
        {
            this.buttonOption = false;

            return base.NavegarFormulario(objeto);
        }

    }
}
