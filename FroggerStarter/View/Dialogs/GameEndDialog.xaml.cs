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
    public sealed partial class GameEndDialog
    {
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
            this.addButton.IsEnabled = false;
        }

        /// <summary>Resets this instance.</summary>
        public void Reset()
        {
            this.addButton.IsEnabled = true;
            this.initialsTextBox.Text = "";
        }

        #endregion
    }
}