using System;
using System.Windows;

namespace ProceXecutioner.UI {

    /// <summary>Displays common alert dialogs. </summary>
    public static class Alerts {

        /// <summary>Displays a Yes/No dialog to the user with the given message, 
        /// and returns a value representing whether the user clicked "Yes". </summary>
        /// <param name="caption">The window caption.</param>
        /// <param name="message">The user prompt.</param>
        /// <returns><c>true</c>, if the user clicks "Yes"; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException">caption and message cannot be null.</exception>
        public static bool GetApproval(string caption, string message) {
            if (caption == null) throw new ArgumentNullException(nameof(caption));
            if (message == null) throw new ArgumentNullException(nameof(message));

            var result = MessageBox.Show(
                caption: caption,
                messageBoxText: message,
                button: MessageBoxButton.YesNo,
                icon: MessageBoxImage.Warning);

            return result == MessageBoxResult.Yes;
        }

        /// <summary>Displays an OK dialog to the user with the given message. </summary>
        /// <param name="caption">The window caption.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">caption and message cannot be null.</exception>
        public static void Notify(string caption, string message) {
            if (caption == null) throw new ArgumentNullException(nameof(caption));
            if (message == null) throw new ArgumentNullException(nameof(message));

            var result = MessageBox.Show(
                caption: caption,
                messageBoxText: message,
                button: MessageBoxButton.OK,
                icon: MessageBoxImage.Information);
        }
    }
}
