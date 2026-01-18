using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.DataForm;
using System.ComponentModel.DataAnnotations;

namespace Modelos.Dto
{
    public partial class FabricacionFormulario : INotifyPropertyChanged, ICustomTypeDescriptor
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        private readonly Dictionary<string, object?> _values = new Dictionary<string, object?>();
        
        private void NotifyPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        
        public void SetValue(string name, object value)
        {
            _values[name] = value;
            NotifyPropertyChanged(name);
        }

        public object? GetValue(string name)
        {
            _values.TryGetValue(name, out object? valor);
            return valor;
        }
        
        public AttributeCollection GetAttributes()
        {
            throw new NotImplementedException();
        }

        public string? GetClassName() => null;
        public string? GetComponentName() => null;
        public TypeConverter? GetConverter() => null;
        public EventDescriptor? GetDefaultEvent() => null;
        public PropertyDescriptor? GetDefaultProperty() => null;
        public object? GetEditor(Type editorBaseType) => null;
        
        public EventDescriptorCollection GetEvents()
        {
            throw new NotImplementedException();
        }

        public EventDescriptorCollection GetEvents(Attribute[]? attributes)
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties()
        {
            throw new NotImplementedException();
        }

        public PropertyDescriptorCollection GetProperties(Attribute[]? attributes)
        {
            throw new NotImplementedException();
        }

        public object? GetPropertyOwner(PropertyDescriptor? pd) => null;
        
        private class DynamicPropertyDescriptor : PropertyDescriptor
        {
            private readonly Type _propertyType;

            public DynamicPropertyDescriptor(string name, Type propertyType)
                : base(name, null)
            {
                _propertyType = propertyType;
            }

            public override Type ComponentType => typeof(FabricacionFormulario);
            public override bool IsReadOnly => false;
            public override Type PropertyType => _propertyType;

            public override bool CanResetValue(object component) => false;
            public override object? GetValue(object component)
            {
                var de = component as FabricacionFormulario;
                return de?.GetValue(Name);
            }

            public override void ResetValue(object component) { }
            public override void SetValue(object component, object? value)
            {
                var de = component as FabricacionFormulario;
                de?.SetValue(Name, value);
            }

            public override bool ShouldSerializeValue(object component) => false;
        }
    }
}
