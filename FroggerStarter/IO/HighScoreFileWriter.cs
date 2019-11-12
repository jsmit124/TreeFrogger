using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Windows.Storage;
using Windows.Storage.Pickers;
using FroggerStarter.Model;

namespace FroggerStarter.IO
{
    /// <summary>
    ///     Stores information for the High Score File Writer class
    /// </summary>
    public static class HighScoreFileWriter
    {
        #region Methods

        /// <summary>
        ///     Writes the high score to file.
        /// </summary>
        /// <param name="info">The information object.</param>
        public static async void FindFileAndWriteHighScoreToFile(HighScoreRecord info)
        {
            try
            {
                var savePicker = new FileSavePicker();
                savePicker.FileTypeChoices.Add("xml", new List<string> {".xml"});
                savePicker.SuggestedFileName = "FroggerHighScores";

                IStorageFile newFile = await savePicker.PickSaveFileAsync();

                WriteToXml(newFile, info);
            }
            catch (IOException)
            {
                //TODO make new file and write xml to it
            }
        }

        /// <summary>Writes information to XML.</summary>
        /// <param name="newFile">The new file.</param>
        /// <param name="info">The information.</param>
        public static async void WriteToXml(IStorageFile newFile, HighScoreRecord info)
        {
            var serializer = new XmlSerializer(typeof(HighScoreRecord));
            var writeStream = await newFile.OpenStreamForWriteAsync();

            serializer.Serialize(writeStream, info);

            writeStream.Close();
        }

        #endregion
    }
}