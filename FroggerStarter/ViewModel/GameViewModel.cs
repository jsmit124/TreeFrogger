

using System.ComponentModel;
using System.Runtime.CompilerServices;
using FroggerStarter.Annotations;

namespace FroggerStarter.ViewModel
{
    /// <summary>
    /// Stores information for the game's ViewModel class
    /// </summary>
    public class GameViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        /// <returns>The PropertyChangedEventHandler</returns>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModel"/> class.
        /// </summary>
        public GameViewModel()
        {

        }

        /// <summary>
        /// Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}