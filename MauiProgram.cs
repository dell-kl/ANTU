using System.Runtime.Versioning;
using ANTU.ViewModel;
using ANTU.Views.Login;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using ANTU.Resources.Components;
using ANTU.Resources.Components.CollectionViewComponents;
using ANTU.Views.dashboard;
using ANTU.Views;
using ANTU.Views.Formularios;
using Syncfusion.Maui.Core.Hosting;
using Data.Rest.RestInterfaces;
using Data.Rest;
using Mopups.Hosting;
using Microsoft.Extensions.Http.Resilience;
using ANTU.Views.Detalles;
using ANTU.Resources.ValueConverter;
using ANTU.Resources.Components.PopupComponents;
using ANTU.Resources.Utilidades;
using ANTU.ViewModel.ComponentsViewModel;
using ANTU.ViewModel.PopupServicesViewModel;
using Business.Services;
using Business.Services.IServices;

namespace ANTU
{
    [SupportedOSPlatform("Android")]
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjGyl/Vkd+XU9FcVRDX3xKf0x/TGpQb19xflBPallYVBYiSV9jS3tTf0drWXpccXFQTmBeVU91Xg==");

            var builder = MauiApp.CreateBuilder();

            builder
                .ConfigureSyncfusionCore()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Segoe_UI_Bold.ttf", "SegoeUIBold");
                    fonts.AddFont("Segoe_UI_Light.ttf", "SegoeUILight");
                    fonts.AddFont("Segoe_Fluent_Icons.ttf", "SegoeFluentIcons");
                    fonts.AddFont("MauiMaterialAssets.ttf", "MaterialAssets");
                    fonts.AddFont("ionicons.ttf", "Ionicons");
                    fonts.AddFont("Material_Symbol_Sharp.ttf", "MaterialSymbolSharp");
                });

            builder.UseMauiApp<App>();
#if ANDROID || WINDOWS
            builder.UseMauiCommunityToolkit();
#endif

#if DEBUG
            builder.Logging.AddDebug();
#endif
   
            var n = builder.Services.AddHttpClient("HttpClientRest", client =>
            {
                //es necesario no poner una diagonal final.
#if DEBUG
                client.BaseAddress = new Uri("http://192.168.100.253:5055");
#endif
            });

            n.AddStandardResilienceHandler().Configure(configure =>
            {
                //numero de intentos permitidos para lograr la conexion o solicitud.
                configure.Retry.MaxRetryAttempts = 4;

                //se cancelar la solicitud si pasan 25 segundos y no se obtiene respueta.
                configure.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(25);
            });

            //Register
            Routing.RegisterRoute("Inicio", typeof(Dashboard));
            Routing.RegisterRoute("SnipperComponent", typeof(SnipperComponent));
            Routing.RegisterRoute(nameof(MateriaPrima), typeof(MateriaPrima));
            Routing.RegisterRoute("FormularioMateriaPrima", typeof(MateriaPrimaFormulario));
            Routing.RegisterRoute("CatalogoProductoFormulario", typeof(CatalogoProductoFormulario));
            Routing.RegisterRoute("MateriaPrimaDetalle", typeof(MateriaPrimaDetalle));
            Routing.RegisterRoute("MostrarImagenesDetalle", typeof(MostrarImagenesDetalle));
            Routing.RegisterRoute("CatalogoProductoDetalle", typeof(CatalogoProductoDetalle));
            Routing.RegisterRoute("FabricacionFormulario", typeof(FabricacionFormulario));
            Routing.RegisterRoute("ProductoListoDetalle", typeof(ProductoListoDetalle));

            //View
            builder.Services.AddTransient<InicioSesion>();
            builder.Services.AddTransient<Dashboard>();
            builder.Services.AddTransient<MateriaPrima>();
            builder.Services.AddTransient<MateriaPrimaFormulario>();
            builder.Services.AddTransient<MateriaPrimaDetalle>();
            builder.Services.AddTransient<MostrarImagenesDetalle>();
            builder.Services.AddTransient<CatalogoProductoFormulario>();
            builder.Services.AddTransient<VentanaPopupService>();
            builder.Services.AddTransient<CatalogoProductoDetalle>();
            builder.Services.AddTransient<FabricacionFormulario>();

            //Popup View
            builder.Services.AddTransient<Mensaje>();
            
            //ViewModel
            builder.Services.AddTransient<InicioSesionViewModel>();
            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddTransient<MateriaPrimaViewModel>();
            builder.Services.AddTransient<FormularioMateriaPrimaViewModel>();
            builder.Services.AddTransient<MateriaPrimaDetalleViewModel>();
            builder.Services.AddTransient<MostrarImagenesDetalleViewModel>();
            builder.Services.AddTransient<CatalogoProductoFormularioViewModel>();
            builder.Services.AddTransient<VentanaPopupServiceViewModel>();
            builder.Services.AddTransient<CatalogoProductoDetalleViewModel>();
            builder.Services.AddTransient<FabricacionFormularioViewModel>();
            builder.Services.AddTransient<ProductoListoDetalleViewModel>();
            
            //CompoentesViewModel
            builder.Services.AddTransient<MateriaPrimaCollectionViewComponentsVIewModel>();
            builder.Services.AddTransient<FabricacionCollectionViewComponentsViewModel>();
            builder.Services.AddTransient<ProductosListosCollectionViewComponentsViewModel>();
            builder.Services.AddTransient<CatalogoProductoCollectionViewComponentsViewModel>();
            
            //Compoentes Collection View
            builder.Services.AddTransient<MateriaPrimaCollectionViewComponents>();
            builder.Services.AddTransient<CatalogoProductoCollectionViewComponents>();
            builder.Services.AddTransient<FabricacionCollectionViewComponents>();
            builder.Services.AddTransient<ProductosListosCollectionViewComponents>();
            
            //otros
            builder.Services.AddTransient<ImageValueConverter>();
            builder.Services.AddTransient<IRestManagement, RestManagement>();
            builder.Services.AddTransientPopup<VentanaPopupService, VentanaPopupServiceViewModel>();
            builder.Services.AddTransient<IManagementService, ManagementService>();

            return builder.Build();
        }
    }
}
