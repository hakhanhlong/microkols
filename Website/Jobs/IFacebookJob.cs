using System.Threading.Tasks;

namespace Website.Jobs
{
    public interface IFacebookJob
    {
        Task ExtendAccessToken();
        Task ExtendAccessToken(int id, string tokenExpired);

        Task UpdateFbPost(int accountid, string username);
    }
}