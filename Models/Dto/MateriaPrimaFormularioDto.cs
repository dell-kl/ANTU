using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Text.RegularExpressions;

namespace ANTU.Models.Dto
{
    public partial class MateriaPrimaFormularioDto : ObservableObject
    {

        private string materiaPrima;
        public string MateriaPrima { set {

            value = value.TrimStart();

            //realizar algunas validaciones.... 
            if(Regex.IsMatch(value, "^([A-Za-z]+\\s*)+$") && value.Length <= 20)
                StatusMateriaPrima = false;
            else
            {
                MensajeMateriaPrima = "Este campo solo acepta letras y maximo 20 caracteres";
                StatusMateriaPrima = true;
            }
            
            SetProperty(ref materiaPrima, value);
            activarBotonRegistrar();
        } get=> materiaPrima; }


        private string kgStandard;
        public string KGStandard { set {

            if (Regex.IsMatch(value, "^[0-9]+(\\.[0-9]+)?$") && double.Parse(value) > 0.0d)
                StatusKGStandard = false;
            else
                StatusKGStandard = true;

            SetProperty(ref kgStandard, value);     
            activarBotonRegistrar();
        } get => kgStandard; }

        private string precio;
        public string Precio { set { 
           
            if (Regex.IsMatch(value, "^[0-9]+(\\.[0-9]+)?$") && decimal.Parse(value) > 0.0m )
                StatusPrecio = false;
            else
                StatusPrecio = true;

            SetProperty(ref precio, value); 
            activarBotonRegistrar();
        } get => precio; }


        private string cantidad;
        public string Cantidad { set {

            if (value.Contains("."))
                value = value.Replace(".", "");

            if (value.Contains(","))
                value = value.Replace(",", "");

            if ( Regex.IsMatch(value, "^[1-9][0-9]*$") )
                StatusCantidad = false;
            else
                StatusCantidad = true;

            SetProperty(ref cantidad, value); 
            activarBotonRegistrar();
        } get => cantidad; }


        // status para mensajes de error y para el boton de registrar
        private bool statusBotonRegistrar = false;
        public bool StatusBotonRegistrar {  set =>  SetProperty(ref statusBotonRegistrar, value); get => statusBotonRegistrar; }


        private bool statusMateriaPrima = true;
        public bool StatusMateriaPrima { set => SetProperty(ref statusMateriaPrima, value); get => statusMateriaPrima; }


        private bool statusKGStandard = true;
        public bool StatusKGStandard { set => SetProperty(ref statusKGStandard, value); get => statusKGStandard; }


        private bool statusPrecio = true;
        public bool StatusPrecio { set => SetProperty(ref statusPrecio, value); get => statusPrecio; }


        private bool statusCantidad = true;
        public bool StatusCantidad { set => SetProperty(ref statusCantidad, value); get => statusCantidad; }

        //campos para mostrar los mensajes de error.

        private string mensajeMateriaPrima = "Debes ingresar un nombre del producto";
        public string MensajeMateriaPrima { set => SetProperty(ref mensajeMateriaPrima, value); get => mensajeMateriaPrima; }

        private string mensajeKGStandard = "Debes ingresar un valor KG valido mayor a 0.0KG";
        public string MensajeKGStandard { set => SetProperty(ref mensajeKGStandard, value); get => mensajeKGStandard; }

        private string mensajePrecio = "Es obligatorio que ponga el precio unitario del producto";
        public string MensajePrecio { set => SetProperty(ref mensajePrecio, value); get => mensajePrecio; }

        private string mensajeCantidad = "Ingresa que has adquirido como mínimo uno";
        public string MensajeCantidad { set => SetProperty(ref mensajeCantidad, value); get => mensajeCantidad; }


        private string[] propertiesButtonRegistrar = ["Gray", "LightGray", "🚫 Registrar"];

        public string[] PropertiesButtonRegistrar { set => SetProperty(ref propertiesButtonRegistrar, value); get => propertiesButtonRegistrar; }

        
        [RelayCommand]
        public void limpiarFormulario()
        {
            this.MateriaPrima = "";
            this.KGStandard = "";
            this.Cantidad = "";
            this.Precio = "";
        }

     
        public void activarBotonRegistrar()
        {
            if (!StatusMateriaPrima && !StatusKGStandard && !StatusCantidad && !StatusPrecio)
            {
                StatusBotonRegistrar = true;
                this.PropertiesButtonRegistrar = ["White", "#BF7A24", "Registrar"];
            }
            else
            {
                StatusBotonRegistrar = false;
                this.PropertiesButtonRegistrar = ["Gray", "LightGray", "🚫 Registrar"]; 
            }
        }

    }
}
       
