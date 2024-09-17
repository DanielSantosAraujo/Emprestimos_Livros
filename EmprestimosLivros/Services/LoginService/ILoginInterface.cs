using EmprestimosLivros.Dto;
using EmprestimosLivros.Models;

namespace EmprestimosLivros.Services.LoginService
{
    public interface ILoginInterface
    {
        Task<ResponseModel<UsuarioModel>> RegistrarUsuario(UsuarioRegisterDto usuarioRegisterDto);
    }
}
