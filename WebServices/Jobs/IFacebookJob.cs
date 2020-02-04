using System.Threading.Tasks;

namespace WebServices.Jobs
{
    public interface IFacebookJob
    {
        Task ExtendAccessToken();
        Task ExtendAccessToken(int id, string tokenExpired);

        Task UpdateFbPost();
        Task UpdateFbPost(int accountid, string username, int type = 1);

        Task UpdateFbInfo();
        Task UpdateFbInfo(int accountid);
    }
}