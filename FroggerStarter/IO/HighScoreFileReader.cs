using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;
using FroggerStarter.Model;

namespace FroggerStarter.IO
{

    /// <summary>
    /// Stores information for the high score file reader class
    /// </summary>
    public class HighScoreFileReader
    {
        /// <summary>
        /// Reads the high scores file.
        /// </summary>
        /// <returns>Returns a Task of type HighScoreCollection</returns>
        public async Task<HighScoreCollection> ReadHighScoresFile()
        {
            var openPicker = new FileOpenPicker() {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,

            };
            openPicker.FileTypeFilter.Add(".xml");

            IStorageFile file = await openPicker.PickSingleFileAsync();

            return this.readFromXml(file).Result;
        }

        private async Task<HighScoreCollection> readFromXml(IStorageFile file)
        {
            var serializer = new XmlSerializer(typeof(HighScoreCollection));
            var readStream = await file.OpenStreamForReadAsync();

            return (HighScoreCollection) serializer.Deserialize(readStream);
        }
    }

}