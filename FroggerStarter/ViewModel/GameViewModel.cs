using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using FroggerStarter.Annotations;
using FroggerStarter.Controller;
using FroggerStarter.Extensions;
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
        ///     Gets or sets the high scores.
        /// </summary>
        /// <value>
        ///     The high scores.
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
        ///     Gets or sets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public int Level { get; set; }

        /// <summary>
        ///     Gets or sets the score.
        /// </summary>
        /// <value>
        ///     The score.
        /// </value>
        public int Score { get; set; }

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
                this.onPropertyChanged();
                this.AddCommand.OnCanExecuteChanged();
            }
        }

        private GameManager manager;

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameViewModel" /> class.
        /// </summary>
        public GameViewModel(GameManager manager)
        {
            this.initials = "";
            this.AddCommand = new RelayCommand(this.addStudent, this.canAddStudent);
            this.Level = 1;
            this.manager = manager;

        }

        #endregion

        #region Methods

        /// <summary>
        ///     Occurs when a property value changes.
        /// </summary>
        /// <returns>The PropertyChangedEventHandler</returns>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool canAddStudent(object obj)
        {
            return true;
        }

        private void addStudent(object obj)
        {
            if (this.HighScores == null)
            {
                this.HighScores = new List<HighScorePlayerInfo>().ToObservableCollection();
            }

            this.highScores.Add(new HighScorePlayerInfo(this.Initials, this.Score, this.Level));
            this.HighScores = this.highScores.ToObservableCollection();
        }

        /// <summary>
        ///     Ons the property changed.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        protected virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}