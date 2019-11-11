using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FroggerStarter.Extensions;
using FroggerStarter.Model;
using FroggerStarter.Utility;

namespace FroggerStarter.ViewModel
{
    /// <summary>
    ///     Stores information for the game's ViewModel class
    /// </summary>
    public class GameViewModel
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
            set => this.highScores = value;
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
            this.AddCommand = new RelayCommand(this.addStudent, this.canAddStudent);
        }

        #endregion

        #region Methods

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

            this.highScores.Add(new HighScorePlayerInfo("JTS", 3, 5));
            this.HighScores = this.highScores.ToObservableCollection();
        }

        #endregion
    }
}