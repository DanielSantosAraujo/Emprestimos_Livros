using EmprestimosLivros.Models;

namespace EmprestimosLivros.Services.SessaoService
{
    public interface ISessaoInterface
    {
        UsuarioModel BuscarSessao();
        void CriarSessao(UsuarioModel usuarioModel);
        void RemoveSessao();
    }
}
