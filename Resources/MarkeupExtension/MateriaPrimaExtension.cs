namespace ANTU.Resources.MarkeupExtension
{
    public partial class MateriaPrimaExtension : IMarkupExtension
    {
        public object enlace { set; get; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {

            if ( enlace is Binding binding )
            {
                var providedValue = (IProvideValueTarget) serviceProvider.GetService(typeof(IProvideValueTarget))!;

                if ( providedValue.TargetObject is BindableObject bindable && providedValue.TargetProperty is BindableProperty bindableProperty)
                {
                    bindable.SetBinding(bindableProperty, binding);

                    var n = bindable.GetValue(bindableProperty);
                }
            }

            return Colors.Red; 
        }

    }
}
