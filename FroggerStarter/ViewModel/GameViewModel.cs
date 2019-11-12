using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Annotations;
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
            get => highScores;
            set
            {
                highScores = value;
                onPropertyChanged();
            }
        }

        /// <summary>Gets or sets the sort combobox selection.</summary>
        /// <value>The sort combobox selection.</value>
        public ComboBoxItem SortComboboxSelection
        {
            get => sortComboboxSelection;
            set
            {
                sortComboboxSelection = value;
                sortScores(value);
                onPropertyChanged();
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
            get => initials;
            set
            {
                initials = value;
                AddCommand.OnCanExecuteChanged();
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GameViewModel" /> class.
        /// </summary>
        public GameViewModel()
        {
            initials = "";
            AddCommand = new RelayCommand(addScore, canAddScore);
            record = new HighScoreRecord();
            HighScores = record.HighScores.ToObservableCollection();
        }

        private void sortScores(object obj)
        {
            switch (sortComboboxSelection.Content)
            {
                case "Score/Name/Level":
                    record.HighScores.Sort(new HighScorePlayerInfo.SortByScoreNameLevel());
                    break;
                case "Name/Score/Level":
                    record.HighScores.Sort(new HighScorePlayerInfo.SortByNameScoreLevel());
                    break;
                default:
                    record.HighScores.Sort(new HighScorePlayerInfo.SortByLevelScoreName());
                    break;
            }

            HighScores = record.HighScores.ToObservableCollection();
        }

        #endregion

        #region Methods

        /// <summary>Occurs when a property value changes.</summary>
        /// <returns></returns>
        public event PropertyChangedEventHandler PropertyChanged;

        private bool canAddScore(object obj)
        {
            return Initials.Length == 3 && sortComboboxSelection != null;
        }

        private void addScore(object obj)
        {
            record.AddInfo(new HighScorePlayerInfo(Initials, 5, 3));
            sortScores(obj);
            HighScores = record.HighScores.ToObservableCollection();
            //HighScoreFileWriter.FindFileAndWriteHighScoreToFile(this.record);
        }


        /// <summary>Ons the property changed.</summary>
        /// <param name="propertyName">Name of the property.</param>
        [NotifyPropertyChangedInvocator]
        public virtual void onPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}