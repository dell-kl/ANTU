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
                var ruta = $"{host}{Endpoints.ENDPOINTS[6]}/{value}";
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
