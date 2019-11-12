using System;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
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
        private ComboBoxItem sortComboboxSelection;
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

        /// <summary>Gets or sets the sort combobox selection.</summary>
        /// <value>The sort combobox selection.</value>
        public ComboBoxItem SortComboboxSelection
        {
            get => this.sortComboboxSelection;
            set
            {
                this.sortComboboxSelection = value;
                this.sortScores(value);
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

        private void sortScores(object obj)
        {
            switch (this.sortComboboxSelection.Content)
            {
                case "Score/Name/Level":
                    this.record.HighScores.Sort(new HighScorePlayerInfo.SortByScoreNameLevel());
                    break;
                case "Name/Score/Level":
                    this.record.HighScores.Sort(new HighScorePlayerInfo.SortByNameScoreLevel());
                    break;
                default:
                    this.record.HighScores.Sort(new HighScorePlayerInfo.SortByLevelScoreName());
                    break;
            }

            this.HighScores = this.record.HighScores.ToObservableCollection();
        }

        #endregion

        #region Methods

        /// <summary>Occurs when a property value changes.</summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool canAddScore(object obj)
        {
            return this.Initials.Length == 3 && this.sortComboboxSelection != null;
        }

        private void addScore(object obj)
        {
            this.record.AddInfo(new HighScorePlayerInfo(this.Initials, 5, 3));
            this.sortScores(obj);
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