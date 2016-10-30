using System.Windows;
using ProceXecutioner.Configuration;
using ProceXecutioner.UI;

namespace ProceXecutioner {

    /// <summary>Interaction logic for App.xaml</summary>
    public partial class App : Application {

        /// <summary>Raises the <see cref="E:System.Windows.Application.Startup" /> event.</summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            var settings = new SystemSettings(); //MockSettings();
            var controller = new Controller(settings);
            var window = new MainWindow(controller);
            window.Show();
        }
    }
}
