using System;
using System.IO;
using System.Runtime.CompilerServices;
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
    public static class HighScoreFileReader
    {
        /// <summary>
        /// Reads the high scores file.
        /// </summary>
        /// <returns>Returns a Task of type HighScoreCollection</returns>
        public static async Task<HighScoreCollection> ReadHighScoresFile()
        {
            try
            {
                var openPicker = new FileOpenPicker() {
                    ViewMode = PickerViewMode.Thumbnail,
                    SuggestedStartLocation = PickerLocationId.DocumentsLibrary,

                };
                openPicker.FileTypeFilter.Add(".xml");

                IStorageFile file = await openPicker.PickSingleFileAsync();

                return await readFromXml(file);
            }
            catch (InvalidOperationException)
            {
                return new HighScoreCollection();
            }
        }

        private static async Task<HighScoreCollection> readFromXml(IStorageFile file)
        {
            var serializer = new XmlSerializer(typeof(HighScoreCollection));
            var readStream = await file.OpenStreamForReadAsync();

            return (HighScoreCollection) serializer.Deserialize(readStream);
        }
    }

}