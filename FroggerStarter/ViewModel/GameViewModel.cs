using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FroggerStarter.Annotations;
using FroggerStarter.Extensions;
using FroggerStarter.IO;
using FroggerStarter.Model;
using FroggerStarter.Utility;

namespace FroggerStarter.ViewModel
{
    /// <summary>
    ///     Stores information for the game's ViewModel class
    /// </summary>
    public class GameViewModel : INotifyPropertyChanged
    {
        #region Data members

        private readonly HighScoreRecord record;
        private ObservableCollection<HighScorePlayerInfo> highScores;
        private string initials;

        #endregion

        #region Properties

        /// <summary>
        ///     Gets the add command.
        /// </summary>
        /// <value>
        ///     The add command.
        /// </value>
        public RelayCommand AddCommand { get; }

        /// <summary>
        ///     Gets or sets the high HighScores.
        /// </summary>
        /// <value>
        ///     The high HighScores.
        /// </value>
        public ObservableCollection<HighScorePlayerInfo> HighScores
        {
            get => this.highScores;
            set
            {
                this.highScores = value;
                this.onPropertyChanged();
            }
        }

        /// <summary>
        ///     Gets or sets the initials.
        /// </summary>
        /// <value>
        ///     The initials.
        /// </value>
        public string Initials
        {
            get => this.initials;
            set
            {
                this.initials = value;
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameViewModel" /> class.
        /// </summary>
        public GameViewModel()
        {
            this.initials = "";
            this.AddCommand = new RelayCommand(this.addScore, this.canAddScore);
            this.record = new HighScoreRecord();
            this.HighScores = this.record.HighScores.ToObservableCollection();
        }

        #endregion

        #region Methods

        /// <summary>Occurs when a property value changes.</summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool canAddScore(object obj)
        {
            return this.Initials.Length == 3;
        }

        private void addScore(object obj)
        {
            this.record.AddInfo(new HighScorePlayerInfo(this.Initials, 5, 3));
            this.record.HighScores.Sort();
            this.HighScores = this.record.HighScores.ToObservableCollection();
            //HighScoreFileWriter.FindFileAndWriteHighScoreToFile(this.record);
        }



        /// <summary>Ons the property changed.</summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        public virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}