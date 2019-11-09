using Windows.UI.Xaml;
using FroggerStarter.Enums;
using FroggerStarter.View.Sprites;
using FroggerStarter.View.Sprites.PlayerMovementAnimation;

namespace FroggerStarter.Model.Animations
{
    /// <summary>
    ///     Stores basic information about the death animation frame object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class PlayerMovementAnimation : GameObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="PlayerMovementAnimation" /> class.
        /// </summary>
        public PlayerMovementAnimation()
        {
            Sprite = new FirstPlayerMovementFrame {Visibility = Visibility.Collapsed};
        }

        #endregion
    }
}