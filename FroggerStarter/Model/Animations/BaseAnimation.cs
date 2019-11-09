namespace FroggerStarter.Model.Animations
{
    /// <summary>
    ///     Stores basic information about the death animation frame object
    /// </summary>
    /// <seealso cref="FroggerStarter.Model.BaseObject" />
    public abstract class BaseAnimation : BaseObject
    {
        #region Methods

        /// <summary>
        ///     Sets the location.
        ///     Precondition: None
        ///     Postcondition: base.X = x, base.Y = y
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        public void SetLocation(double x, double y)
        {
            X = x;
            Y = y;
        }

        #endregion
    }
}