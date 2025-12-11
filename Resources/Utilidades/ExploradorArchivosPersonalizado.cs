using Android.Content;

namespace ANTU.Resources.Utilidades;

public static class ExploradorArchivosPersonalizado
{
    private static Intent? intent { get; set; } = null;
    
    public static async Task AbrirExploradorArchivos()
    {
#if ANDROID

        if (intent is null && OperatingSystem.IsAndroidVersionAtLeast(30))
        {
            intent = new Intent(Intent.ActionOpenDocument);
            intent.SetType("*/*");
            intent.AddCategory(Intent.CategoryOpenable);
            intent.PutExtra(Intent.ExtraAllowMultiple, true);
            
            var picker = Intent.CreateChooser(intent, "Selecciona solo 5 imágenes");
            
            var activity = ActivityStateManager.Default.GetCurrentActivity()!;
            activity.StartActivityForResult(picker, 11001);
        }
        
#endif
    }
}