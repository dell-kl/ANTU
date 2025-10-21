using ANTU.Resources.Rest.RestInterfaces;

namespace ANTU.ViewModel
{
    public partial class CatalogoViewModel : ParentViewModel
    {
        public CatalogoViewModel(IRestManagement restManagement) : base(restManagement)
        {
        }
    }
}
