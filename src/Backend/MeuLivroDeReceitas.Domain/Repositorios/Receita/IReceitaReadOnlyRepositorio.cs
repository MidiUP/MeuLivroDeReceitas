namespace MeuLivroDeReceitas.Domain.Repositorios.Receita;

public interface IReceitaReadOnlyRepositorio
{
    Task<IList<Entidades.Receita>> GetByUsuario(long userId);
    Task<Entidades.Receita> GetById(long idReceita);
}
