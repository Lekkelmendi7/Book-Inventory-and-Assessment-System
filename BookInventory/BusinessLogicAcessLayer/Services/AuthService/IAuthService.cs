using BookInventory.BusinessLogicAcessLayer.Helpers;
using BookInventory.BusinessLogicAcessLayer.Models.AccountModels;
using BookInventory.DataAccessLayer.Entities;

namespace BookInventory.BusinessLogicAcessLayer.Services.AuthService
{
    public interface IAuthService
    {
        Task<ServiceResponse<int>> Register(User user, string password);
        Task<ServiceResponse<string>> Login(string username, string password);
        Task<bool> UserExists(string username);
        Task<PaginatedResult<UserGetModel>> GetUsers(int page, int size);
        Task<UserGetModel> GetUserById(int id);
        Task UpdateUser(UserUpdateModel student, int id);
        Task DeleteUser(int id);
        Task<ServiceResponse<string>> ChangePassword(string username, string currentPassword, string newPassword);
        Task<ServiceResponse<string>> ForgotPassword(string username);
        Task<ServiceResponse<string>> ResetPassword(PasswordResetModel request);
    }
}
