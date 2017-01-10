
namespace DMS_M306.Interfaces
{
    /// <summary>
    /// The database context factory interface.
    /// </summary>
    /// <typeparam name="T">Type to be manufactured.</typeparam>
    public interface IDbContextFactory<T>
    {
        #region Public Methods and Operators

        /// <summary>
        /// The build.
        /// </summary>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Build();

        #endregion
    }
}