using Services.Models.User;
using Services.Models.User.Requests;

namespace Services.ServiceInterfaces;

public interface IUserService
{
    public Task<UserModel> GetUserByEmail(string email);
    public Task<RecoveryModel> GetUserForRecovery(string email);
    public Task<bool> UserVerification(AuthUserRequest authUserRequest);
    public Task TryAddUser(AuthUserRequest authUserRequest);
    public Task TryUpdateEmail(UpdateEmailRequest updateEmailRequest, string oldEmail);
    public Task TryUpdatePassword(UpdatePasswordRequest updatePasswordRequest, string email);
    public Task TryDeleteUser(string email);
}