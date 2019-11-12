using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace FroggerStarter.View.Dialogs
{
    /// <summary>
    ///     Stores information for the game end dialog
    /// </summary>
    /// <seealso cref="Windows.UI.Xaml.Controls.ContentDialog" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector" />
    /// <seealso cref="Windows.UI.Xaml.Markup.IComponentConnector2" />
    public sealed partial class GameEndDialog : ContentDialog
    {
        #region Data members

        /// <summary>The add to high scores button clicked event handler</summary>
        public EventHandler<AddToHighScoresButtonClickedEventArgs> AddToHighScoresButtonClicked;

        /// <summary>
        ///     The high scores button clicked event handler
        /// </summary>
        public EventHandler<EventArgs> HighScoresButtonClicked;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameEndDialog" /> class.
        /// </summary>
        public GameEndDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Methods

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.initialsTextBox.Text.Length < 3)
            {
                this.initialsErrorTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                this.initialsErrorTextBlock.Visibility = Visibility.Collapsed;
                this.addButton.IsEnabled = false;

                var initials = new AddToHighScoresButtonClickedEventArgs {Initials = this.initialsTextBox.Text};
                this.AddToHighScoresButtonClicked?.Invoke(this, initials);
            }
        }

        private void ViewHighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            this.HighScoresButtonClicked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>Resets this instance.</summary>
        public void Reset()
        {
            this.addButton.IsEnabled = true;
            this.initialsTextBox.Text = "";
        }

        /// <summary></summary>
        /// <seealso cref="System.EventArgs" />
        public class AddToHighScoresButtonClickedEventArgs : EventArgs
        {
            #region Properties

            /// <summary>Gets or sets the initials.</summary>
            /// <value>The initials.</value>
            public string Initials { get; set; }

            #endregion
        }

        #endregion
    }
}