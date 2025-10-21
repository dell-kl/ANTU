using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.DataForm;
using System.ComponentModel.DataAnnotations;


namespace ANTU.Models.Dto
{
    public class MateriaPrimaEditarDataFormulario : ObservableObject
    {
        private string nombreMateriaPrima = "";

        [DataFormDisplayOptions(ValidMessage = "Dato ingresado correctamente")]
        [Required(ErrorMessage = "Ingresa un nombre para cambiar el anterior.")]
        [Display(Name = "Nombre Producto", Prompt = "Nuevo nombre para tu materia prima")]
        [MaxLength(20, ErrorMessage = "Este campo solo acepta letras y maximo 20 caracteres")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Solo se permiten letras")]
        public string NombreMateriaPrima { set => SetProperty(ref nombreMateriaPrima, value); get => nombreMateriaPrima; }
    }
}
