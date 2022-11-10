using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InjectDiscord
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string originalCode;
        public MainWindow()
        {
            InitializeComponent();
            string indexPath = Injector.findPath($"C:\\Users\\{Environment.UserName}\\Appdata\\Local\\Discord", "discord_desktop_core") + @"\discord_desktop_core\index.js";
            string beforeCode = File.ReadAllText(indexPath);
            originalCode = beforeCode.Replace("`", "\\`");
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            browserCode.Source =
               new Uri(System.IO.Path.Combine(
                   System.AppDomain.CurrentDomain.BaseDirectory,
                   @"Monaco\index.html"));
            await browserCode.EnsureCoreWebView2Async();

        }

        private async void NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            await browserCode.ExecuteScriptAsync($"editor.setValue(`{originalCode}`);");
            await browserCode.ExecuteScriptAsync("monaco.editor.setTheme('vs-dark')");

        }

        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            string value = await browserCode.ExecuteScriptAsync("editor.getValue();");
            Injector.MainFunc(value.Replace("\\n", "\r\n").Remove(value.Length - 1, 1).Remove(0, 1), false);
        }

        private async void sne_Click(object sender, RoutedEventArgs e)
        {
            string value = await browserCode.ExecuteScriptAsync("editor.getValue();");
            Injector.MainFunc(value.Replace("\\n", "\r\n").Remove(value.Length - 1, 1).Remove(0, 1), true);
            Environment.Exit(0);
        }
    }
}
