using ANTU.Resources.Customizer;

namespace ANTU
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new AppShell();
        
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(EntryCustomizer), (handler, view) => {

#if __ANDROID__
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
