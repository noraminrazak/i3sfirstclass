using SmartSchoolsV2.Models;
using System.Threading.Tasks;

namespace SmartSchoolsV2.Services
{
    public interface IFirebaseAuth
    {
        UserModel GetUser();
        Task<string> LoginFacebookAsync(string token);
        Task<string> LoginAsync(string email, string pwd);
        Task<bool> CreateUserAsync(string email, string pwd);

        void Logout();
    }
}
