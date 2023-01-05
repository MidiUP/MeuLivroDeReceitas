
namespace MeuLivroDeReceitas.Domain.Repositorios.Usuario;

public interface IUsuarioWriteOnlyRepositorio
{
    Task Adcionar(Entidades.Usuario usuario);
}
