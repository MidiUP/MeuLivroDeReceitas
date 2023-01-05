using MeuLivroDeReceitas.Domain.Entidades;

namespace MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;

public interface IUsuarioLogado
{
    Task<Usuario> RecuperarUsuario();
}
