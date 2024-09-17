using System.ComponentModel.DataAnnotations;

namespace EmprestimosLivros.Dto
{
    public class UsuarioRegisterDto
    {
        [Required(ErrorMessage = "Digite o Nome!")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Digite o Sobrenome!")]
        public string Sobrenome { get; set; }
        [Required(ErrorMessage = "Digite o email!")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Digite a senha!")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "Digite a confirmação de senha!"),
            Compare("Senha", ErrorMessage = "As senhas não estão iguais")]
        public string ConfirmaSenha { get; set; }
    }
}
