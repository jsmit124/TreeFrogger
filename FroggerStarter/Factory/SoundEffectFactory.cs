﻿
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using FroggerStarter.Enums;

namespace FroggerStarter.Factory
{
    /// <summary>Stores information for the Sound Effect Factory class</summary>
    public static class SoundEffectFactory
    {

        /// <summary>Finds the file location for the specified sound effect.</summary>
        /// <param name="type">The type of sound effect to build.</param>
        /// <exception cref="NotImplementedException"></exception>
        /// <returns>Returns the file location of the specified sound effect</returns>
        public static async Task<MediaElement> BuildEffectElement(SoundEffectType type)
        {
            var element = new MediaElement();
            var folder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("Assets" + Path.DirectorySeparatorChar + "SoundEffects");
            StorageFile file = null;
            switch (type)
            {
                case SoundEffectType.Death:
                    file = await folder.GetFileAsync("Death");
                    break;
                case SoundEffectType.CompletedLevel:
                    file = await folder.GetFileAsync("LevelComplete");
                    break;
                case SoundEffectType.GameOver:
                    file = await folder.GetFileAsync("GameOver");
                    break;
                case SoundEffectType.MadeItHome:
                    file = await folder.GetFileAsync("MadeItHome");
                    break;
                case SoundEffectType.PowerUpActivated:
                    file = await folder.GetFileAsync("PowerUpActivated");
                    break;
                default:
                    throw new NotImplementedException("sound effect not implemented");
            }

            var stream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            element.SetSource(stream, "");

            return element;
        }
    }
}