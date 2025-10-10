using CommunityToolkit.Mvvm.Messaging.Messages;


namespace ANTU.Resources.Messenger
{
    public class AppShellMessenger : ValueChangedMessage<Dictionary<string, bool>>
    {
        public AppShellMessenger(Dictionary<string, bool> datos) : base(datos) { 
        
        }
    }
}
