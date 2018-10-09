using System.Threading.Tasks;

namespace Phishtank.Common.Persistence.Abstractions
{
    public interface IPhishSubmissionsRepository
    {
        /// <summary>
        /// Checks if the given URL has been flagged as a phish in our database
        /// </summary>
        /// <param name="host">Host of the url</param>
        Task<bool> IsPhishAsync(string host);

        /// <summary>
        /// Performs the setup operations for the database if it does not already exist
        /// </summary>
        Task InitializeAsync();
    }
}
