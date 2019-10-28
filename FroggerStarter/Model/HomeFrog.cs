using FroggerStarter.View.Sprites;

namespace FroggerStarter.Model
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public class HomeFrog : BaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeFrog"/> class.
        /// </summary>
        public HomeFrog()
        {
            Sprite = new HomeSprite();
        }
    }
}
