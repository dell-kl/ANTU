using ANTU.Resources.Rest;
using ANTU.Resources.Rest.RestInterfaces;
using System.Globalization;

namespace ANTU.Resources.ValueConverter
{
    public class ImageValueConverter : IValueConverter
    {

        public IRestManagement restManagement;

        public ImageValueConverter()
        {
            restManagement = IPlatformApplication.Current?.Services.GetService<IRestManagement>()!;
        }


        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            string host = restManagement.httpClient.BaseAddress!.AbsoluteUri;

            if (value is not null && !value!.Equals("default_icon.png"))
            {
                if (parameter is Binding binding)
                {
                    if (binding.Source is Image image)
                        parameter = image.ClassId;
                }

                var ruta = $"{host}{Endpoints.VISOR_IMG[0]}?imagen={value}&tipo={parameter}";
                return ruta;
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
