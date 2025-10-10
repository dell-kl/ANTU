using ANTU.ViewModel;
using ANTU.Views.Login;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using ANTU.Resources.Components;
using ANTU.Views.dashboard;
using ANTU.Views;
using ANTU.Views.Formularios;
using Syncfusion.Maui.Core.Hosting;
using ANTU.Resources.Rest.RestInterfaces;
using ANTU.Resources.Rest;
using Mopups.Hosting;
using Microsoft.Extensions.Http.Resilience;
using ANTU.Views.Detalles;


namespace ANTU
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjGyl/Vkd+XU9FcVRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3tTf0drWXpccXFQTmBeVU91Xg==");

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Segoe_UI_Bold.ttf", "SegoeUIBold");
                    fonts.AddFont("Segoe_UI_Light.ttf", "SegoeUILight");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            

            var n = builder.Services.AddHttpClient("HttpClientRest", client =>
            {
                client.BaseAddress = new Uri("http://192.168.100.19:5055/");
            
            });

            n.AddStandardResilienceHandler().Configure(configure =>
            {
                //numero de intentos permitidos para lograr la conexion o solicitud.
                configure.Retry.MaxRetryAttempts = 4;

                //se cancelar la solicitud si pasan 25 segundos y no se obtiene respueta.
                configure.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(25);
            });

            Routing.RegisterRoute("Inicio", typeof(Dashboard));
            Routing.RegisterRoute("SnipperComponent", typeof(SnipperComponent));
            Routing.RegisterRoute(nameof(MateriaPrima), typeof(MateriaPrima));
            Routing.RegisterRoute(nameof(Catalogo), typeof(Catalogo));
            Routing.RegisterRoute(nameof(Fabricacion), typeof(Fabricacion));
            Routing.RegisterRoute(nameof(ProductosListos), typeof(ProductosListos));
            Routing.RegisterRoute("FormularioMateriaPrima", typeof(MateriaPrimaFormulario));
            Routing.RegisterRoute("MateriaPrimaDetalle", typeof(MateriaPrimaDetalle));


            builder.Services.AddTransient<InicioSesion>();
            builder.Services.AddTransient<Dashboard>();
            builder.Services.AddTransient<Catalogo>();
            builder.Services.AddTransient<Fabricacion>();
            builder.Services.AddTransient<MateriaPrima>();
            builder.Services.AddTransient<ProductosListos>();
            builder.Services.AddTransient<MateriaPrimaFormulario>();
            builder.Services.AddTransient<MateriaPrimaDetalle>();

            builder.Services.AddTransient<InicioSesionViewModel>();
            builder.Services.AddTransient<WindowAlertComponent>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<CatalogoViewModel>();
            builder.Services.AddTransient<FabricacionViewModel>();
            builder.Services.AddTransient<MateriaPrimaViewModel>();
            builder.Services.AddTransient<ProductosListosViewModel>();
            builder.Services.AddTransient<FormularioMateriaPrimaViewModel>();
            builder.Services.AddTransient<MateriaPrimaDetalleViewModel>();

            builder.Services.AddTransient<IRestManagement, RestManagement>();
 
            return builder.Build();
        }
    }
}
