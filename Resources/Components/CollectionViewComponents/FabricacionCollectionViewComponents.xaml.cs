using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ANTU.ViewModel;

namespace ANTU.Resources.Components.CollectionViewComponents;

public partial class FabricacionCollectionViewComponents : ContentView
{
    public FabricacionCollectionViewComponents()
    {
        InitializeComponent();
    }

    protected async override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        if (this.BindingContext is MateriaPrimaViewModel materiaPriamViewmodel)
        {
            await materiaPriamViewmodel.DesmontarSpinner();
        }
    }
}