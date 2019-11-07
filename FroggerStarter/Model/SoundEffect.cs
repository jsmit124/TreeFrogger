
using Windows.UI.Xaml.Controls;
using FroggerStarter.Enums;
using FroggerStarter.Factory;

namespace FroggerStarter.Model
{ 
    /// <summary>Stores information for the Sound Effect model class</summary>
    public class SoundEffect
    {
        private readonly MediaElement soundElement;

        /// <summary>Initializes a new instance of the <see cref="SoundEffect"/> class.</summary>
        /// <param name="type">The type of sound effect.</param>
        public SoundEffect(SoundEffectType type)
        {
            this.soundElement = SoundEffectFactory.BuildEffectElement(type).Result;
        }

        /// <summary>Plays the sound effect.</summary>
        public void Play()
        {
            this.soundElement.Play();
        }
    }
}
