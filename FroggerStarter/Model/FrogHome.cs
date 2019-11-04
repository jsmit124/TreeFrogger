using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    ///     Stores information about the home frog object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class FrogHome : BaseObject
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="FrogHome" /> class.
        /// </summary>
        public FrogHome()
        {
            Sprite = new HomeSprite();
        }

        #endregion
    }
}