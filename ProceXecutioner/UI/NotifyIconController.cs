using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace ProceXecutioner.UI {

    /// <summary>Allows WPF applications to collapse to the notification tray. </summary>
    public class NotifyIconController {

        /// <summary>Initializes a new instance of the <see cref="NotifyIconController"/> class. </summary>
        /// <param name="window">The application window.</param>
        /// <exception cref="ArgumentNullException">window cannot be null.</exception>
        public NotifyIconController(Window window) {
            if (window == null) throw new ArgumentNullException(nameof(window));

            this.window = window;
            this.window.StateChanged += Window_StateChanged;

            this.notifyIcon = new NotifyIcon();
            this.notifyIcon.Visible = false;
            this.notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
        }

        /// <summary>The application window. </summary>
        private readonly Window window;

        /// <summary>Gets or sets the notification icon. </summary>
        public Icon Icon {
            get { return this.notifyIcon.Icon; }
            set {
                if (value == null) throw new ArgumentNullException(nameof(value));
                this.notifyIcon.Icon = value;
            }
        }
        /// <summary>Underlying WinForms icon. </summary>
        private readonly NotifyIcon notifyIcon;

        /// <summary>Gets or sets the notification text. </summary>
        public string Text {
            get { return this.notifyIcon.Text; }
            set {
                if (value == null) throw new ArgumentNullException(nameof(value));
                this.notifyIcon.Text = value;
            }
        }

        /// <summary>Handles the DoubleClick event of the NotifyIcon control. </summary>
        private void NotifyIcon_DoubleClick(object sender, EventArgs e) => ExpandFromTray();

        /// <summary>Handles the StateChanged event of the Window control. </summary>
        private void Window_StateChanged(object sender, EventArgs e) {
            if (this.window.WindowState == WindowState.Minimized)
                CollapseToTray();
        }

        /// <summary>Collapses application window to notification tray.</summary>
        public void CollapseToTray() {
            this.notifyIcon.Visible = true;
            this.window.Hide();
        }

        /// <summary>Expands application window from notification tray. </summary>
        public void ExpandFromTray() {
            this.notifyIcon.Visible = false;
            this.window.Show();
            this.window.WindowState = WindowState.Normal;

            var prevTopmost = this.window.Topmost;
            this.window.Topmost = true;
            this.window.Topmost = prevTopmost;
        }
    }
}
