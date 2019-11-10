

using System.ComponentModel;
using System.Runtime.CompilerServices;
using FroggerStarter.Annotations;
using FroggerStarter.Model;

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
        /// Gets the high scores.
        /// </summary>
        /// <value>
        /// The high scores.
        /// </value>
        public HighScoreCollection HighScores { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameViewModel"/> class.
        /// </summary>
        public GameViewModel()
        {

        }

        /// <summary>
        /// Sets the high scores.
        /// </summary>
        /// <param name="highScores">The high scores.</param>
        public void SetHighScores(HighScoreCollection highScores)
        {
            this.HighScores = highScores;
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