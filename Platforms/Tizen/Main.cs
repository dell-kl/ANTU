using System;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace ANTU
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CrteateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
