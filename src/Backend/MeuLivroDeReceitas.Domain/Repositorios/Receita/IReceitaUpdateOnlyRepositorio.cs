namespace MeuLivroDeReceitas.Domain.Repositorios.Receita;

public interface IReceitaUpdateOnlyRepositorio
{
    void Update(Entidades.Receita receita);

    Task<Entidades.Receita> GetById(long idReceita);
}
