using Harmonizer.DTO;
using Harmonizer.Model;

namespace Harmonizer.Services.Interface
{
    public interface IAuthRepository
    {
        Task<ProfileDTO> GetUserDetails(int id);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
    }
}
