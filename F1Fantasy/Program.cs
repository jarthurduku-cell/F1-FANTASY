using System;
using System.Windows;

namespace F1Fantasy
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // Arranque correcto de una aplicación WPF.
            var app = new App();
            app.InitializeComponent(); // viene de App.xaml
            app.Run(); // usará StartupUri o el manejo en App.xaml.cs
        }
    }
}